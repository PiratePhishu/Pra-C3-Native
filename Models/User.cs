using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pra_C3_Native.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Credits { get; set; }

        public bool Admin {  get; set; }

        public User() 
        {
            Admin = false;
            Credits = 50;
        }

        public void PasswordHash(string password)
        {
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

    }
}
