using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pra_C3_Native
{
    internal class Pra_C3_Native_app
    {

        
        public void Run()
        {
            bool loggedIn = false;
            string userInput = "";
            while (userInput.ToLower() != "x") 
            {
                userInput = ShowMenu(loggedIn);
            }
        }

        

        public string ShowMenu(bool loggedIn)
        {
            if (loggedIn)
            {
                Console.WriteLine("1. show all matches");
                Console.WriteLine("2. show user account");
            }
            else
            {
                Console.WriteLine("1. registreer account");
                Console.WriteLine("2. log in");
            }
            string userInput = Console.ReadLine();
            return userInput;
        }

    }
}
