using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityComplaints.Core.Entities
{
    internal class Complaint
    {
        public int Id { get; set; }
        
        [Display(Name = "Створив")]
        public virtual IdentityUser Author { get; set; }


        public string District { get; set; } //autofill from un-geocoding location
        public string Address { get; set; } //autofill from un-geocoding location
        public DbGeography Location { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Solved { get; set; }

        public virtual IdentityUser? Solver { get; set; }
        public string? UtilityCommentary { get; set; }



    }
}
