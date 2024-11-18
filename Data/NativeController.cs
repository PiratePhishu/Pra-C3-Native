using Microsoft.EntityFrameworkCore;
using Pra_C3_Native.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pra_C3_Native.Data
{
    internal class NativeController : DbContext
    {
        public DbSet<User> users {  get; set; }
        public DbSet<Match> matches { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +                     // Server name
                "port=3306;" +                            // Server port
                "user=root;" +                     // Username
                "password=;" +                 // Password
                "database=Native_Voetball;"       // Database name
                , Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.21-mysql") // Version
                );
        }
    }
}
