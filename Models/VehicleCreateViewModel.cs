using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class VehicleCreateViewModel
    {   
        public int Id { get; set; }
        public string Vin { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }
        public int Year { get; set; }
        public double PurchasePrice { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public DateOnly? SaleDate { get; set; }
        public string? Description { get; set; }


    }
}