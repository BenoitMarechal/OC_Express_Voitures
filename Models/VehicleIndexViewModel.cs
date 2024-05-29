using Microsoft.Identity.Client;
using OC_Express_Voitures.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class VehicleIndexViewModel
    {
        public int Id { get; set; }
        public string Vin { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }
        [CustomYearRange]
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
        public double RetailPrice { get; set; }
        public string Status { get; set; }

        public Photo? Photo { get; set; }

    }
}
