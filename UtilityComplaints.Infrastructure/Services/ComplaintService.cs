using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityComplaints.Core.Entities;
using UtilityComplaints.Core.Interfaces;
using UtilityComplaints.Infrastructure.Data;

namespace UtilityComplaints.Infrastructure.Services
{
    public class ComplaintService : IComplaintService 
    {

        private readonly ApplicationDbContext _context;

        public ComplaintService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddComplaint(Complaint complaint)
        {
            throw new NotImplementedException();
        }

        public bool ComplaintExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteComplaint(Complaint complaint)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdateComplaint(Complaint complaint)
        {
            throw new NotImplementedException();
        }

        //solve(): change only status and commentary
    }
}
