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
                        ShowAllMatches();
                        break;
                    case "2":
                        ShowUsersBets();
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
                        ShowAllMatches();
                        break;
                    case "2":
                        ShowUsersBets();
                        break;
                    case "3":
                        GetAllMatches();
                        break;
                    case "4":
                        GetAllScores();
                        break;
                    case "5": 
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

        public void ShowUsersBets()
        {
            List<Bet> bets = Datacontext.Bets.ToList();
            List<Match> matches = Datacontext.Matches.ToList();
            int total = 0;
            int count = 0;
            Console.Clear();
            Console.WriteLine($"user: {session.Name}");
            Console.WriteLine($"Credits: {session.Credits}\n");
            foreach (Bet bet in bets)
            {
                if (bet.User == session)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                Console.WriteLine("er zijn geen weddenschappen gevonden");
                Helpers.Pause();
                return;
            }
            Console.WriteLine($"--------------------------------------");
            Console.WriteLine("Geplaatste weddenschappen:\n");
           
            foreach(Bet bet in bets)
            {
                if (bet.User == session)
                {
                    if (!bet.PayedOut)
                    {
                        foreach (Match match in matches) 
                        {
                            if (bet.Match == match)
                            {
                                Console.WriteLine($"   id: {match.id} | {match.Team1} vs {match.Team2}");
                                Console.WriteLine($"   Team: {bet.Winner} | credits: {bet.amount}\n");
                            }
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("Verlopen weddenschappen:\n");
            foreach (Bet bet in bets)
            {
                if (bet.User == session)
                {
                    if (bet.PayedOut)
                    {
                        foreach (Match match in matches)
                        {
                            if (bet.Match == match)
                            {
                                if (bet.Won == true)
                                {
                                    Console.WriteLine("   Gewonnen:");
                                }

                                else if (bet.Won == false)
                                {
                                    Console.WriteLine("   verlooren");
                                }
                                Console.WriteLine($"   id: {match.id} | {match.Team1} vs {match.Team2}");
                                Console.WriteLine($"   Team: {bet.Winner} | credits: {bet.amount}");
                                if (bet.Won == true)
                                {
                                    Console.WriteLine($"   credits gewonnen: {bet.amount}\n");
                                    total += bet.amount;
                                }

                                else if (bet.Won == false)
                                {
                                    Console.WriteLine($"   credits verloren: {bet.amount}\n");
                                    total -= bet.amount;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"--------------------------------------");
            Console.WriteLine();
            Console.WriteLine($"totale winst: {total} credits");

            Helpers.Pause();
        }

        public void AddBet(Match match)
        {
            Console.Clear();
            Bet bet = new Bet();
            int amount;
            if (match == null)
            {
                Console.WriteLine("wedstrijd is niet gevonden");
                Helpers.Pause();
                return;
            }
            Console.WriteLine($"id: {match.id} | {match.Team1} vs {match.Team2}\n");
            Console.WriteLine($"1.{match.Team1}\n2.{match.Team2}");
            Console.Write("vul team nummer in:");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    bet.Winner = match.Team1;
                    break;
                case "2":
                    bet.Winner = match.Team2;
                    break;
                default:
                    Console.WriteLine("Team niet gevonden");
                    Helpers.Pause();
                    return;
            }

            amount = Helpers.AskInt("hoeveel wil je inzetten (vul 0 in om te geen bot te zetten)");
            if (amount <= 0)
            {
                Console.Clear();
                Console.WriteLine("bot geännuleert");
                Helpers.Pause();
            }
            else if (amount > session.Credits)
            {
                Console.WriteLine("niet genoeg credits");
                Helpers.Pause();
            }
            else
            {
                bet.amount = amount;
                bet.User = session;
                bet.Match = match;
                session.Credits -= amount;
                Datacontext.Add(bet);
                Datacontext.SaveChanges();
            }
        }

        public void ShowAllMatches()
        {
            Console.Clear();
            List<Match> matches = Datacontext.Matches.ToList();
            foreach (Match match in matches)
            {
                if (match.Winner == null)
                {
                    Console.WriteLine($"id: {match.id} | {match.Team1} vs {match.Team2}");
                }
            }
            Console.WriteLine("voer wedstrijd id in om een bot te plaatsen | druk op enter om door te gaan");
            string selectedMatch = Console.ReadLine();

            if (int.TryParse(selectedMatch, out int selected))
            {
                Match match = Datacontext.Matches.Find(selected);
                if (match.Winner == null)
                {
                    AddBet(match);
                }
            
            }
        }

        public void GetAllMatches()
        {
            Console.Clear();
            List<Match> db_Matches = Datacontext.Matches.ToList(); 
            List<MatchApi> matches = reader.GetMatch();
            Console.WriteLine("Matches added:");
            foreach (MatchApi match in matches)
            {
                bool found = false;
                foreach (Match db_match in db_Matches)
                {
                    if (db_match.Match_id == match.id)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    Console.WriteLine($"Match ID: {match.id}, {match.team1.name} vs {match.team2.name}");
                    Match newMatch = new Match(match.id, match.team1_id, match.team2_id, match.team1.name, match.team2.name);
                    if(match.team1_score != null && match.team2_score != null)
                    {
                        newMatch.Team1_Score = match.team1_score;
                        newMatch.Team2_Score = match.team2_score;
                        if (match.team1_score > match.team2_score)
                        {
                            newMatch.Winner = match.team1.name;
                        }
                        else if (match.team1_score < match.team2_score)
                        {
                            newMatch.Winner = match.team2.name;
                        }
                        else newMatch.Winner = "none";
                    }
                    Datacontext.Add(newMatch);
                    Datacontext.SaveChanges();
                }
            }
            Helpers.Pause();
        }

        public void GetAllScores()
        {
            GetAllMatches();
            UpdateAllScore();
            List<Bet> bets = Datacontext.Bets.ToList();
            foreach (Bet bet in bets) 
            {
                Match match = bet.GetMatch();
                User user = bet.GetUser();
                int teamchoice = 0;
                //gets the team of choce
                if (match.Team1 == bet.Winner)
                {
                    teamchoice = 1;
                }
                else
                {
                    teamchoice = 2;
                }
                // checks wat team winns
                if (match.Team1_Score > match.Team2_Score)
                {
                    if(teamchoice == 1)
                    {
                        bet.PayedOut = true;
                        bet.Won = true;
                        user.Credits += bet.amount * 2;
                    }
                    else
                    {
                        bet.PayedOut= false;
                        bet.Won = false;
                    }
                }
                else if(match.Team1_Score < match.Team2_Score)
                {
                    if (teamchoice == 2)
                    {
                        bet.PayedOut = true;
                        bet.Won = true;
                        user.Credits += bet.amount * 2;
                    }
                    else
                    {
                        bet.PayedOut = false;
                        bet.Won = false;
                    }
                }
                Datacontext.Bets.Update(bet);
                Datacontext.Users.Update(user);
                Datacontext.SaveChanges();
            }
        }

        public void UpdateAllScore()
        {
            List<Match> db_Matches = Datacontext.Matches.ToList();
            List<MatchApi> matches = reader.GetMatch();

            foreach (MatchApi match in matches)
            {
                foreach (Match db_match in db_Matches)
                {
                    if (db_match.Match_id == match.id)
                    {
                        db_match.Team1_Score = match.team1_score;
                        db_match.Team2_Score = match.team2_score;
                        Datacontext.Matches.Update(db_match);
                    }
                }

            }
            Datacontext.SaveChanges();
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
                Console.WriteLine($"user: {session.Name}");
                Console.WriteLine($"Credits: {session.Credits}");
                Console.WriteLine("1. show all matches");
                Console.WriteLine("2. weddenschap overzicht\n");
                if (session.Admin == true)
                {
                    Console.WriteLine("3. haal wedstrijden op");
                    Console.WriteLine("4. haal de score op");
                    Console.WriteLine("5. log out");

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
