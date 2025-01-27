﻿using BCrypt.Net;
using Pra_C3_Native.Data;
using Pra_C3_Native.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB_app
{
    internal class Helpers
    {
        internal static string Ask(string question)
        {
            string? userInput = "";

            while((userInput is null || userInput == ""))
            {
                Console.WriteLine(question);
                userInput = Console.ReadLine();
            }

            return userInput;
        }
        
        internal static int AskInt(string question)
        {
            int retVal;
            string userInput = "";

            while (!int.TryParse(userInput, out retVal))
            {
                userInput = Ask(question);
            }

            return retVal;
        }

        
        internal static void Pause()
        {
            Console.WriteLine("Press <ENTER> to continue...");
            Console.ReadLine();
        }
    }
}