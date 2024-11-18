using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pra_C3_Native.Models
{
    internal class Match
    {
        public int id {  get; set; }
        public DateTime Time { get; set; }
        public string Team1 {  get; set; }
        public string Team2 { get; set; }

        public Match() 
        { 
        }
    }
}
