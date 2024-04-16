using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class Operation
    {

        public int Id { get; set; }
        public int VehicleId { get; set; }
        
        public double PurchasePrice { get; set; }
        public double SellingPrice { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime SaleDate { get; set; }

        // Navigation Property
        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle? Vehicle { get; set; }



    }
}
