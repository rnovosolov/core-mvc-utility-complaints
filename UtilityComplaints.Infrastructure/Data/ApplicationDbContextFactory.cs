using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using UtilityComplaints.Core.Entities;
using BAMCIS.GeoJSON;
using System.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace UtilityComplaints.Infrastructure.Data
{
    /*public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private IConfiguration Configuration { get; }
        private string ConfigKey { get; }

        public ApplicationDbContextFactory() { }

        public ApplicationDbContextFactory(string configKey)
        {
            this.ConfigKey = configKey ?? throw new ArgumentNullException(nameof(configKey));
            ConfigurationBuilder cb = new ConfigurationBuilder();

            cb.AddUserSecrets(Assembly.GetCallingAssembly())
                   .AddEnvironmentVariables();
            //AddConfigurationSources(cb, Assembly.GetCallingAssembly());
            Configuration = cb.Build();
        }

        /*protected virtual void AddConfigurationSources(ConfigurationBuilder builder,
                                                       Assembly asm)
        {
            builder.AddUserSecrets(asm)
                   .AddEnvironmentVariables();
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddUserSecrets("USER_SECRETS_ID");
            var config = configBuilder.Build();
            var connectionString = config["ConnectionStrings:DefaultConnection"];

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString(ConfigKey),
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-MHHT5K2\SQLEXPRESS;Initial Catalog=UtilityComplaintsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", 
            o => o.UseNetTopologySuite());

            var modelBuilder = new ModelBuilder();
            modelBuilder.Entity<Complaint>()
                .HasOne(g => g.Author)
                .WithMany(t => t.CreatedComplaints);

            modelBuilder.Entity<Complaint>()
                .HasOne(g => g.Solver)
                .WithMany(t => t.SolvedComplaints);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
        
    }*/
}
