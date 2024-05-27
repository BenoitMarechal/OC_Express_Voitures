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
        public DbSet<OC_Express_Voitures.Models.Operation> Operation { get; set; } = default!;
        public DbSet<OC_Express_Voitures.Models.Photo> Photo { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
              .HasOne(r => r.Operation)
              .WithOne(r => r.Vehicle);

            modelBuilder.Entity<Vehicle>()
            .HasMany(r => r.Repairs)
            .WithOne(r => r.Vehicle);             



            modelBuilder.Entity<Repair>()
                .HasOne(r => r.Vehicle)
                .WithMany(v => v!.Repairs) 
                .HasForeignKey(r => r.VehicleId);

            
            modelBuilder.Entity<Operation>()
                .HasOne(o => o.Vehicle)
                .WithOne(v => v.Operation)
                .HasForeignKey<Operation>(o => o.VehicleId)
                .IsRequired();

            modelBuilder.Entity<Photo>()
           .HasOne(p => p.Vehicle)
           .WithOne(v => v.Photo)
           .HasForeignKey<Photo>(p => p.VehicleId);
           



        }


    }
}
