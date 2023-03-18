using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityComplaints.Core.Entities;

namespace UtilityComplaints.Core.Interfaces
{
    internal interface IComplaintService
    {

        //--------------- CRUD Methods -----------------------
        public bool AddComplaint(Complaint complaint);
        public bool UpdateComplaint(Complaint complaint);
        public bool DeleteComplaint(Complaint complaint);
        public bool Save();


        //----------------- Existion checks ------------------
        public bool ComplaintExists(int id);
    }
}
