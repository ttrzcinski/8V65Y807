using EchoBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Utils
{
    // Represents central hub of bot capabilities and responses.
    /// <summary>
    /// Represents central hub of bot.
    /// </summary>
    public sealed class Mind
    {
        // TODO Should be externalized and references with relative path.

        /// <summary>
        /// Hardcoded path to fast retorts file.
        /// </summary>
        private const string RetortsFullPath = "fast_retorts.json";

        /// <summary>
        /// Retorts as quick responses to questions.
        /// </summary>
        private List<Retort> _retorts;

        /// <summary>
        /// The highest id of retorts.
        /// </summary>
        private int _retortsMaxId = -1;

        /// <summary>
        /// Holds the only instance of this class.
        /// </summary>
        private static Mind _instance;

        /// <summary>
        /// Thread-safe lock of initialization
        /// </summary>
        private static readonly object Padlock = new object();

        /// <summary>
        /// Serves as handle to kept base of facts
        /// </summary>
        private FactsBase _facts;

        public FactsBase Facts => _facts;

        /// <summary>
        /// Creates new instance of Mind.
        /// </summary>
        private Mind()
        {
            // Obtain retorts, if are not loaded
            if (_retorts == null)
            {
                LoadRetorts();
            }

            AssureNN_facts();
        }

        /// <summary>
        /// Returns handle to existing instance with prior constructing it, if lacks one.
        /// </summary>
        public static Mind Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new Mind());
                }
            }
        }

        /// <summary>
        /// Reads JSON file with retorts.
        /// </summary>
        public void LoadRetorts()
        {
            // TODO: Fix the path top search from within the project
            // TODO: Change to relative path

            using (StreamReader reader = new StreamReader(RetortsFullPath))
            {
                var json = reader.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<Retort>>(json);
                _retorts = items;

                //Refresh _retortsMaxId
                FindMaxRetortsId();
            }
        }

        /// <summary>
        /// Assures facts base being initialized.
        /// </summary>
        private void AssureNN_facts()
        {
            // Check, if facts base is null
            if (_facts == null)
            {
                _facts = new FactsBase();
            }
        }

        /// <summary>
        /// Finds the highest id from retorts.
        /// </summary>
        private void FindMaxRetortsId()
        {
            if (_retorts != null) _retortsMaxId = _retorts.Select(t => t.Id).OrderByDescending(t => t).FirstOrDefault();
        }

        private static string Now()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// Backups retorts file in order not to loose all those retorts.
        /// </summary>
        /// <returns>true means backup was created, false otherwise</returns>
        private bool BackupRetorts()
        {
            //Prepare name of backup file
            var backupPath = RetortsFullPath.Replace(".json", $"_{Now()}.json");
            //copy current file to backup
            File.Copy(RetortsFullPath, backupPath);

            //var endFlag = File.Exists(backupPath);
            /*using (var writer = new StreamWriter(backupPath))
            {
                writer.Write(_retorts);
                writer.Flush();
                endFlag = true;
                _retortsMaxId++;
            }*/
            return File.Exists(backupPath);
        }

        /// <summary>
        /// Adds new retort to retorts file.
        /// </summary>
        /// <param name="question">given question</param>
        /// <param name="answer">its answer</param>
        /// <returns>true means added, false means error</returns>
        public bool AddRetort(string question, string answer)
        {
            //Check, if params have content
            if (string.IsNullOrEmpty(question) || string.IsNullOrEmpty(answer))
            {
                return false;
            }
            // TODO check id before and after
            var added = new Retort { Id = _retortsMaxId + 1, Question = question, Answer = answer };

            // Backup retorts in order not to do something funky
            var endFlag = BackupRetorts();
            if (endFlag)
            {
                //Clear file of retorts
                File.WriteAllText(RetortsFullPath, string.Empty);

                // Opens file of retorts for edit and add it at the end
                using (var file = File.CreateText(RetortsFullPath))
                {
                    _retorts.Add(added);
                    var json = JsonConvert.SerializeObject(_retorts, Formatting.Indented);
                    file.Write(json);
                    _retortsMaxId++;
                }
            }
            else
            {
                Console.WriteLine("Couldn't make a backup of retorts.");
            }

            return endFlag;
        }

        /// <summary>
        /// Searches for given questions in list of retorts and returns answer, if it exists.
        /// </summary>
        /// <param name="question">given question to bot</param>
        /// <returns>answer, if exists, null otherwise</returns>
        public string Respond(string question)
        {
            //Obtain retorts, if are not loaded
            if (_retorts == null)
            {
                LoadRetorts();
            }
            //
            string response = null;
            // TODO covert it to lambda expression
            //response = _retorts.Find(item => item.Question.ToLower().Equals(question)).Answer;
            foreach (var retort in _retorts)
            {
                if (retort.Question.ToLower().Equals(question))
                {
                    response = retort.Answer;
                    break;
                }
            }
            //Return found response
            return response;
        }

        /// <summary>
        /// Returns count of retorts.
        /// </summary>
        /// <returns>counts, if there are some retorts, on null returns 0</returns>
        public int CountRetorts()
        {
            return _retorts?.Count ?? 0;
        }

        /// <summary>
        /// Returns top id of all retorts.
        /// </summary>
        /// <returns>top id, if there are some retorts, on null or empty returns 0</returns>
        public int RetortsMaxId()
        {
            return _retortsMaxId;
        }
    }
}
