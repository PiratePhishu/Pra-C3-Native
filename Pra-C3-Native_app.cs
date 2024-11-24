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
        User? session = null;

        public Pra_C3_Native_app()
        {
            Datacontext = new NativeContext();
        }
        public void Run()
        {
            string userInput = "";

            while (userInput.ToLower() != "x")
            {
                userInput = ShowMenu();
                HandleInput(userInput);
            }
        }

        public void HandleInput(string userInput)
        {
            if (session != null)
            {
                switch (userInput)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        session = null;
                        break;
                    default:
                        Console.WriteLine("kies een geldige keuze");
                        break;
                }
            }
            else
            {
                switch (userInput)
                {
                    case "1":
                        RegisterAccount();
                        break;
                    case "2":
                        logIn();
                        break;
                    default:
                        Console.WriteLine("kies een geldige keuze");
                        break;
                }
            }
        }

        public void RegisterAccount()
        {
            bool found = false;
            while(session == null) 
            {
                found = false;
                Console.Clear();
                User user = new User();
                Console.WriteLine("enter Username:");
                user.Name = Console.ReadLine();
                List<User> users = Datacontext.users.ToList();
                foreach (User userName in users)
                {
                    if(userName.Name == user.Name)
                    {
                        Console.WriteLine("username is al in gebruik");
                        Console.ReadLine();
                        found = true;
                    }
                }
                if (found)
                {
                    continue;
                }
                Console.WriteLine("\nenter email:");
                user.Email = Console.ReadLine().ToLower();
                Console.WriteLine("\nenter password:");
                string password = Console.ReadLine();
                Console.WriteLine("\nrepeat password:");
                string passwordRepeat = Console.ReadLine();
                if (password == passwordRepeat)
                {
                    user.PasswordHash(password);
                    Datacontext.Add(user);
                    Datacontext.SaveChanges();
                    Console.WriteLine("user has been added");
                    Console.ReadLine();
                    session = user;
                }
                else
                {
                    Console.WriteLine("username was not the same");
                    Console.ReadLine();
                }
            }
        }

        public void logIn()
        {
            string userinput;
            List<User> userList = Datacontext.users.ToList();

            Console.Clear();
            Console.WriteLine("please enter your username:");
            userinput = Console.ReadLine();
            foreach(User user in userList)
            {
                if (user.Name == userinput)
                {
                    Console.WriteLine("please enter your password");
                    string password = Console.ReadLine();
                    if (BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                    {
                        session = user;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("onjuist wachtwoord");
                        Console.ReadLine();
                    }
                }
            }
            Console.WriteLine("gebruiker niet gevonden");
            Console.ReadLine();

        }

        public string ShowMenu()
        {
            Console.Clear();
            if (session != null)
            {
                Console.WriteLine(session.Name);
                Console.WriteLine(session.Credits);
                Console.WriteLine("1. show all matches");
                Console.WriteLine("2. show user account");
                Console.WriteLine("3. log out");
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
