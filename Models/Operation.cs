﻿using OC_Express_Voitures.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OC_Express_Voitures.Utils;

namespace OC_Express_Voitures.Models
{
    public class Operation
    {
        private const int minYear = Constants.OldestYear;        


        public int Id { get; set; }
        public int VehicleId { get; set; }
        
        public double PurchasePrice { get; set; }

        public double SellingPrice { get; set; }
        public bool IsAvailable { get; set; }

        [CustomDateRange]
        public DateOnly PurchaseDate { get; set; }

        [CustomDateRange]
        public DateOnly? SaleDate { get; set; }
        

        // Navigation Property
        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle Vehicle { get; set; }


    }
}
