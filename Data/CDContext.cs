using CDCollectionApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDCollectionApp.Data
{
    public class CDContext : DbContext
    {
        public CDContext(DbContextOptions<CDContext> options) : base(options)
        {

        }
        public DbSet<CD> CDs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Track> Tracks { get; set; }


    }
}
