using OC_Express_Voitures.Migrations;
using OC_Express_Voitures.Models;

namespace OC_Express_Voitures.Utils
{
    public static class StatusHelper
    {
        public static string ReturnStatus(Operation operation)
        {
            if (operation.SaleDate != null)
            {
                operation.IsAvailable = false;
                return "Sold";
            }
            if (operation.IsAvailable == false)
            {
                return "Available soon";
            }

            return "In stock";
        }
    }
}
