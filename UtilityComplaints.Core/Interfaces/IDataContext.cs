using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityComplaints.Core.Entities;

namespace UtilityComplaints.Core.Interfaces
{

    public interface IDataContext : IDisposable
    {
        DbSet<Complaint> Complaints { get; set; }
        DbSet<User> Users { get; set; }

        //IdentityDbContext<User> Instance { get; }

        //void Add(Complaint complaint);
        //void Update(Complaint complaint);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
