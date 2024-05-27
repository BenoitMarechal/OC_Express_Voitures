using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class VehicleDetailsViewModel
    {
        public int Id { get; set; }
        public string Vin { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Finish { get; set; }
        public int RepairsCount { get; set; }
        public double RetailPrice { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
        public Photo? Photo { get; set; }

    }
}
