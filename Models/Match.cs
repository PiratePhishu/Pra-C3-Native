﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pra_C3_Native.Models
{
    internal class Match
    {
        public int id {  get; set; }
        public string Team1 {  get; set; }
        public string Team2 { get; set; }

        public int Team1_id { get; set; }
        public int Team2_id { get; set; }

        public Match(int team1_id, int team2_id, string team1, string team2) 
        { 
            Team1 = team1;
            Team2 = team2;
            Team1_id = team1_id;
            Team2_id = team2_id;
        }

        public Match() { }  
    }
}
