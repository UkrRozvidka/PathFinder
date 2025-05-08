using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Context.Config;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace DAL.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<HikeMember> HikeMembers { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Hike> Hikes { get; set; }
        public DbSet<Track> Tracks { get; set; }

        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
            Users = Set<User>();
            HikeMembers = Set<HikeMember>();
            Points = Set<Point>();
            Hikes = Set<Hike>();
            Tracks = Set<Track>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(HikeConfig).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
