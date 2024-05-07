using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class VehicleOperationViewModel
    {   
        public int Id { get; set; }
        public string Vin { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }
        public int Year { get; set; }
        public double PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? SaleDate { get; set; }

    }
}