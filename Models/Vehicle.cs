namespace OC_Express_Voitures.Models;
using OC_Express_Voitures.Utils;


    public class Vehicle
    {   

        public int Id { get; set; }       
        public string Vin   { get; set; }
        public string Brand  { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }

        [CustomYearRange]
        public int Year { get; set; }
        public string? Description{ get; set; }

        public  ICollection<Repair>? Repairs{ get; set; }       
       public virtual required Operation Operation { get; set; }
        public Photo? Photo { get; set; }


    }

