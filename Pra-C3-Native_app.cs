using BCrypt.Net;
using Pra_C3_Native.Data;
using Pra_C3_Native.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pra_C3_Native
{
    internal class Pra_C3_Native_app
    {
        NativeContext Datacontext;
        User session = new User();
        bool loggedIn = false;

        public Pra_C3_Native_app() 
        {
            Datacontext = new NativeContext();
        }
        public void Run()
        {
            string userInput = "";
      
            while (userInput.ToLower() != "x") 
            {
                userInput = ShowMenu(loggedIn);
                loggedIn = HandleInput(userInput, loggedIn);
            }
        }

        public bool HandleInput(string userInput, bool loggedIn)
        {
            if (loggedIn)
            {
                switch(userInput) 
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    default:
                        Console.WriteLine("kies een geldige keuze");
                        break;
                }
                return true;
            }
            else
            {
                switch (userInput)
                {
                    case "1":
                        loggedIn = RegisterAccount();
                        return loggedIn;
                    default:
                        Console.WriteLine("kies een geldige keuze");
                        return false;
                }
            }
        }

        public bool RegisterAccount()
        {
            Console.Clear();
            User user = new User();
            Console.WriteLine("enter Username");
            user.Name = Console.ReadLine();
            Console.WriteLine("enter email");
            user.Email = Console.ReadLine().ToLower();
            Console.WriteLine("enter password");
            string password = Console.ReadLine();
            Console.WriteLine("repeat password");
            string passwordRepeat = Console.ReadLine();
            if (password == passwordRepeat) {
                user.PasswordHash(password);
                Datacontext.Add(user);
                Datacontext.SaveChanges();
                Console.WriteLine("user has been added");
                return true;
                
            }
            else
            {
                Console.WriteLine("username was not the same");
                return false;
            }

        }

        

        public string ShowMenu(bool loggedIn)
        {
            Console.Clear();
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
