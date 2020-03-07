using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TelefonosConDetalle.Entidades;

namespace TelefonosConDetalle.DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Personas> personas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = TelefonosDetalle.db");
        }
    }
}
