using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UtilityComplaints.Core.Entities;
using UtilityComplaints.Core.Interfaces;

namespace UtilityComplaints.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>, IDataContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UtilityRepresentative> Representatives { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
