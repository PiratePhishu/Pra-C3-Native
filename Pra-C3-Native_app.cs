using BCrypt.Net;
using CarDB_app;
using Microsoft.EntityFrameworkCore.Storage;
using Pra_C3_Native.Data;
using Pra_C3_Native.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pra_C3_Native.Data.ApiReader;

namespace Pra_C3_Native
{
    internal class Pra_C3_Native_app
    {
        public ApiReader reader;
        NativeContext Datacontext;
        User? session = null;

        public Pra_C3_Native_app()
        {
            reader = new ApiReader();
            Datacontext = new NativeContext();
        }
        public void Run()
        {
            AdminAdd();
            string userInput = "";

            while (userInput.ToLower() != "x")
            {
                userInput = ShowMenu();
                HandleInput(userInput);
            }
        }

        public void HandleInput(string userInput)
        {
            if (session != null && session.Admin == false)
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
            
            else if (session == null) 
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

            else if (session.Admin == true)
            {
                switch (userInput)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        GetAllMatches();
                        break;
                    case "4":
                        session = null;
                        break;
                    default:
                        Console.WriteLine("kies een geldige keuze");
                        break;
                }
            }
        }
        
        public void AdminAdd()
        {
            if (Datacontext.Users.Count() == 0) // check if there are any users if not make a new admin account
            {
                Console.WriteLine("no users found, creating admin account");
                string password = "admin";
                string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
                Datacontext.Add(new User() { Name = "admin", Email = "admin@admin.com", Password = passwordHash, Admin = true });
                Datacontext.SaveChanges();
            }
        }

        public void ClearMatches()
        {
            List<Match> matches = Datacontext.Matches.ToList();
            foreach (Match match in matches)
            {
                Datacontext.Remove(match);
            }
        }

        public void GetAllMatches()
        {
            ClearMatches();
            Console.Clear();
            List<Match> matches = new List<Match>();
            for (int i = 1; i > 0; i++)
            {
                MatchApi match = reader.GetMatch(i.ToString());
                if (match.team1_id == 0)
                {
                    break;
                }
                else
                {
                    
                    Match newMacht = new Match(match.team1_id, match.team2_id, match.team1_name, match.team2_name);
                    matches.Add(newMacht);
                }
            }
            foreach (Match match in matches) 
            {
                Console.WriteLine($"{match.Team1_id}:{match.Team1} vs {match.Team2_id}:{match.Team2}\n");
                Datacontext.Add(match);
            }
            Datacontext.SaveChanges();  
            Helpers.Pause();
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
                List<User> users = Datacontext.Users.ToList();
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
            List<User> userList = Datacontext.Users.ToList();

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
                Console.WriteLine("2. show user account \n");
                if (session.Admin == true)
                {
                    Console.WriteLine("3. haal wedstrijden op");
                    Console.WriteLine("4. log out");
                }
                else
                {
                    Console.WriteLine("3. log out");
                }
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
