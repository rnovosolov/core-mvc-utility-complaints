using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UtilityComplaints.Core.Entities;
using UtilityComplaints.Core.Interfaces;
using NetTopologySuite.Geometries;

namespace UtilityComplaints.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>, IDataContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Author)
                .WithMany(a => a.CreatedComplaints);
                //.HasForeignKey(cc => cc.Author.Id);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Solver)
                .WithMany(s => s.SolvedComplaints);
                //.HasForeignKey(sc => sc.Solver);
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

        }
    }
}