namespace UtilityComplaints.WebUI.Models
{
    public class EditComplaintViewModel
    {
        public int Id { get; set; }
        public string District { get; set; }
        public string? Address { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

        public string Description { get; set; }

    }
}
