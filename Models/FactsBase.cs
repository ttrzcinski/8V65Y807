using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Models
{
    /// <summary>
    /// Works as a dictionary of facts, where a key is just one word.
    /// </summary>
    public class FactsBase
    {
        /// <summary>
        /// Keeps facts as dictionary entries.
        /// </summary>
        private SortedDictionary<string, string> facts;

        /// <summary>
        /// Creates new Fact Base instance.
        /// </summary>
        public FactsBase()
        {
            // Assure presence of facts base.
            if (facts == null)
            {
                facts = new SortedDictionary<string, string>();
            }
        }

        /// <summary>
        /// Adds fact to pool of facts.
        /// </summary>
        /// <param name="key">name of fact</param>
        /// <param name="value">value of fact</param>
        /// <returns>true, if added, false otherwise</returns>
        public bool Add(string key, string value)
        {
            // If key is empty, there is no point in search
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) return false;
            // If facts base contains a key
            if (facts.ContainsKey(key))
            {
                // Override prior value
                facts[key] = value;
            }
            else
            {
                // Add to facts pool
                facts.Add(key, value);
            }
            return true;
        }

        /// <summary>
        /// Reads fact, if fact exists and returns it's value.
        /// </summary>
        /// <param name="key">name of fact</param>
        /// <returns>value of fact, if exist, null otherwise</returns>
        public string Read(string key)
        {
            return !string.IsNullOrEmpty(key) && facts.ContainsKey(key) ? facts[key] : null;
        }

        /// <summary>
        /// Checks, if given key with value exists as a fact.
        /// </summary>
        /// <param name="key">given name of fact</param>
        /// <param name="value">given value</param>
        /// <returns>true means exists, false otherwise</returns>
        public bool Exists(string key, string value)
        {
            return key != null && (facts.TryGetValue(key, out var actualValue) && actualValue == value);
        }

        /// <summary>
        /// Removes a fact marked with a key, if it exists. 
        /// </summary>
        /// <param name="key">name of fact</param>
        /// <returns>true means removed, false otherwise</returns>
        public bool Remove(string key)
        {
            return !string.IsNullOrEmpty(key) && facts.ContainsKey(key) && facts.Remove(key);
        }

        /// <summary>
        /// Returns count of facts pool.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return facts.Count;
        }
    }
}
