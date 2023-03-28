using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityComplaints.Core.Entities
{
    public class UtilityRepresentative : IdentityUser
    {
        public string District { get; set; }

        public virtual ICollection<Complaint>? Complaints { get; set; }
    }
}
