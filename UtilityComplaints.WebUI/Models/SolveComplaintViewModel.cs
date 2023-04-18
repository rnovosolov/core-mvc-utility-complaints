using UtilityComplaints.Core.Entities;

namespace UtilityComplaints.WebUI.Models
{
    public class SolveComplaintViewModel
    {
        public int Id { get; set; }
        public string District { get; set; }
        public string? Address { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }


        public string Description { get; set; }
        public DateTime Created { get; set; }
        
        public Status Status { get; set; }

        public DateTime? Solved { get; set; }
        public virtual User? Solver { get; set; }
        public string? UtilityCommentary { get; set; }
    }
}
