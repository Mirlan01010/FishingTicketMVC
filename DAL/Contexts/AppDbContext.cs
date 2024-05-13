using DAL.Entities;
using DAL.Entities.Statistics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contexts
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<ExtendedIdentityUser> ExtendedIdentityUsers { get; set; }

        public DbSet<Region> Regions { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

        public DbSet<WaterBody> WaterBodies { get; set; }

        public DbSet<CitizenShip> CitizenShips  { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<ForDay> ForDays { get; set; }
        public DbSet<ForMonth> ForMonths { get; set; }
        public DbSet<Restrict> Restricts { get; set; }



    }
}
