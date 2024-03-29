﻿namespace OC_Express_Voitures.Models
{
    public class Vehicle
    {     
        public int Id { get; set; } 
        public string Vin   { get; set; }
        public string Brand  { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }
        public int Year { get; set; }

        //NAvigation property
        public virtual ICollection<Repair>? Repairs{ get; set; }

    }
}
