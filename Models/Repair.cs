using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class Repair
    {
        public int Id { get; set; }
        [ForeignKey ("VehicleId")]
        public int VehicleId { get; set; }
        public string Title { get; set; }  
        public double Cost { get; set; }
        
        public DateTime Date { get; set; }
              

        // Navigation Property
        public virtual Vehicle? Vehicle { get; set; }

    }
}
