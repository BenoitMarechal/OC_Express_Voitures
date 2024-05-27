namespace OC_Express_Voitures.Models;
   using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


    public class PhotoImportViewModel
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

    }

