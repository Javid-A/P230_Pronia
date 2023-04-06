using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using P230_Pronia.Entities;
using P230_Pronia.ViewModels;

namespace P230_Pronia.DAL
{
    public class ProniaDbContext : IdentityDbContext<User>
    {
        public ProniaDbContext(DbContextOptions<ProniaDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PlantDeliveryInformation> PlantDeliveryInformation { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PlantSizeColor> PlantSizeColors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var item in modelBuilder.Model
                                .GetEntityTypes()
                                        .SelectMany(e=>e.GetProperties()
                                                    .Where(p=>p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))))
            {
                item.SetColumnType("decimal(6,2)");
            }

            modelBuilder.Entity<Setting>()
                .HasIndex(s => s.Key)
                .IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            base.OnModelCreating(modelBuilder);
        }



    }
}
