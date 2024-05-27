using System.ComponentModel.DataAnnotations.Schema;

namespace OC_Express_Voitures.Models
{
    public class Photo
    {
        public int Id { get; set; }
        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        public byte[] ImageData { get; set; }  
        public string ImageFileName { get; set; }

        // Navigation Property
        public virtual Vehicle? Vehicle { get; set; }
    }
}
