using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class VehicleOperationViewModel
    {

        //  public class Vehicle
        //  {
       // public int Id { get; set; }
        public string Vin { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }
        public int Year { get; set; }

        //Navigation properties
        // public ICollection<Repair>? Repairs { get; set; }
        // [ForeignKey(nameof(OperationId))]
        //  public virtual required Operation? Operation { get; set; }
        //  }

        // public class Operation
        // {
        //   public int Id { get; set; }

        public int VehicleId { get; set; }

        public double PurchasePrice { get; set; }
        public double SellingPrice { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime SaleDate { get; set; }

        // Navigation Property
        //   [ForeignKey(nameof(VehicleId))]
        //   public virtual Vehicle? Vehicle { get; set; }
        //}



    }
}