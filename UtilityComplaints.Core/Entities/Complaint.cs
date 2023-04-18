using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Operation.Distance;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BAMCIS.GeoJSON;


namespace UtilityComplaints.Core.Entities

{
    public class Complaint
    {
        public int Id { get; set; }
        public virtual User Author { get; set; }


        public string District { get; set; } //autofill from un-geocoding location
        public string? Address { get; set; } //autofill from un-geocoding location
        public double Lat { get; set; }
        public double Lon { get; set; }

        //public Point Location { get; set; }


        public string Description { get; set; }
        public DateTime Created { get; set; }

        public Status Status { get; set; }

        public DateTime? Solved { get; set; }
        public virtual User? Solver { get; set; }
        public string? UtilityCommentary { get; set; }



        [NotMapped]
        public virtual Feature Feature {
            get
            {
                if (Lat != null && Lon != null)
                {
                    var geometry = new Point(
                        new Position(
                            double.Parse(Lon.ToString()),
                            double.Parse(Lat.ToString())
                            )
                        );
                    var properties = new Dictionary<string, object>
                    {
                        {"Id", Id},
                        {"District", District},
                        {"Address", Address},
                        {"Description", Description},
                        {"Created", Created},
                        {"Status", Status},
                        {"Solved", Solved},
                        {"Solver", Solver},
                        {"UtilityCommentary", UtilityCommentary}
                    };

                    return new Feature(geometry, properties);
                }

                return null;
            }
        }


    }


    public enum Status
    {
        Active,
        Solved
    }

    public enum District
    {
        Дарницький, 
        Деснянський, 
        Дніпровський,
        Голосіївський, 
        Оболонський, 
        Печерський, 
        Подільский, 
        Святошинський,
        Соломяньский, 
        Шевченківський
    }

}
