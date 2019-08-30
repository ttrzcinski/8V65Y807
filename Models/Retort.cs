using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Models
{
    // Represents a single retort item from responses tree.
    /// <summary>
    /// Represents a single retort item from responses tree.
    /// </summary>
    internal class Retort
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        // Returns concatenated form of retort to list it.
        /// <summary>
        /// Returns concatenated form of retort to list it.
        /// </summary>
        /// <returns>id) retort line</returns>
        public string AsStackEntry()
        {
            return $"{Id}) {Question}: {Answer}";
        }
    }
}
