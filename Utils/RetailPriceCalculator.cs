using Microsoft.EntityFrameworkCore;
using OC_Express_Voitures.Models;

namespace OC_Express_Voitures.Utils
{
    public class RetailPriceCalculator
    {
        private const double fixMargin = 500;
        public static double CalculateRetailPrice(Operation operation, List<Repair> repairs)
        {
            double price = operation.PurchasePrice +fixMargin;
            if (repairs.Count>0)
            {
                foreach (Repair repair in repairs)
                {
                    price += repair.Cost;
                }
            }                      
            return price;            
        }
    }
}
