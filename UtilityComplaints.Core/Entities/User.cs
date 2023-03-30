using Microsoft.AspNetCore.Identity;

namespace UtilityComplaints.Core.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<Complaint>? CreatedComplaints { get; set; }
        public virtual ICollection<Complaint>? SolvedComplaints { get; set; }

    }
}
