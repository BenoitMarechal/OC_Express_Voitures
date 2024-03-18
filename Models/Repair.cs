using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class Repair
    {
        public int Id { get; set; }
        public string Title { get; set; }  
        public double Cost { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey ("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

    }
}
