using Microsoft.Identity.Client;
using OC_Express_Voitures.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class VehicleEditViewModel
    {
        public int Id { get; set; }
        public string Vin { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }

        [CustomYearRange]
        public int Year { get; set; }
        [CustomDateRange]
        public DateOnly? SaleDate { get; set; }
        [CustomDateRange]
        public DateOnly PurchaseDate { get; set; }
        public double PurchasePrice { get; set; }

        public double RetailPrice { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public Photo? Photo { get; set; }
        public int RepairsCount { get; set; }


    }
}
