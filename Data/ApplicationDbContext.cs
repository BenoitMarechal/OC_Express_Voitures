using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OC_Express_Voitures.Models;

namespace OC_Express_Voitures.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<OC_Express_Voitures.Models.Repair> Repair { get; set; } = default!;
        public DbSet<OC_Express_Voitures.Models.Vehicle> Vehicle { get; set; } = default!;
    }
}
