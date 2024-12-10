using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pra_C3_Native.Models
{
    internal class Bet
    {
        public Match Match { get; set; }
        public User User { get; set; }
        public int id {  get; set; }
        public string Winner { get; set; }
        public int amount { get; set; }
        public bool PayedOut { get; set; }

        public Bet() 
        {
            PayedOut = false;
        }
    }
}
