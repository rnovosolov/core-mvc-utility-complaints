using Microsoft.AspNetCore.Identity;

namespace UtilityComplaints.Core.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<Complaint>? Complaints { get; set; }


    }
}
