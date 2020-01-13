using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwp_App
{
    public class DbModel : DbContext
    {

        
        public DbSet<Rezerwacja> TRezerwacja { get; set; }
        public DbSet<Meldunek> TMeldunki { get; set; }
        public DbSet<Atrakcje> TAtrakcje { get; set; }
        public DbSet<Basen> TBasen { get; set; }
        public DbSet<Spa> TSpa { get; set; }

        public DbSet<Users> TRecepcjonista { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
                => options.UseSqlite("Data Source=SystemV6.db");

//
      
    }
}
