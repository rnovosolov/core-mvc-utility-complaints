using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Operation.Distance;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UtilityComplaints.Core.Entities

{
    public class Complaint
    {
        public int Id { get; set; }
        
        public virtual User Author { get; set; }


        public string District { get; set; } //autofill from un-geocoding location
        public string Address { get; set; } //autofill from un-geocoding location

        [Column(TypeName = "decimal(8,6)")]
        public decimal Lon { get; set; }
        [Column(TypeName = "decimal(8,6)")]
        public decimal Lat { get; set; }

        [NotMapped]
        public Point Location { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Solved { get; set; }

        public virtual UtilityRepresentative? Solver { get; set; }
        public string? UtilityCommentary { get; set; }

    }
}
