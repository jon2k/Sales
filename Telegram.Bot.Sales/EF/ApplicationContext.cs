using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.EF
{

    public class ApplicationContext : DbContext
    {
        public ILogger<ApplicationContext> Logger { get; }
        // public string ConnString { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options, ILogger<ApplicationContext> logger) : base(options)
        {
            Logger = logger;
            //Create DB, it it doesn't exist
            Database.EnsureCreated();
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<OrderCurrent> OrdersCurrent { get; set; }
        public DbSet<OrderHistory> OrdersHistory { get; set; }
        public DbSet<ProductPriceHistory> ProductsPriceHistory { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<TypeNotification> TypesNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(b => b.Url)
                .IsUnique();

            modelBuilder.Entity<Shop>().HasData(
                new Shop[]
                {
                    new Shop { Id=1, Name="WB", Url="www.wildberries.ru"},
                    new Shop { Id=2, Name="Lamoda", Url="www.lamoda.ru"},
                    new Shop { Id=3, Name="Bask", Url="www.bask.ru"},
                    new Shop { Id=4, Name="PlanetaSport", Url="www.planeta-sport.ru"},
                    new Shop { Id=5, Name="TrialSport", Url="www.trial-sport.ru"},
                    new Shop { Id=6, Name="Velostrana", Url="www.velostrana.ru"},
                    new Shop { Id=7, Name="Velosklad", Url="www.velosklad.ru"}
                });
            modelBuilder.Entity<Currency>().HasData(
                new Currency[]
                {
                    new Currency{Id=1, CurrencyType="Rur"},
                    new Currency{Id=2, CurrencyType="$"},
                    new Currency{Id=3, CurrencyType="Euro"}
                });
            modelBuilder.Entity<TypeNotification>().HasData(
              new TypeNotification[]
              {
                    new TypeNotification{Id=1, Name="Telegram"},
                    new TypeNotification{Id=2, Name="Viber"},
                    new TypeNotification{Id=3, Name="WatsApp"},
                    new TypeNotification{Id=4, Name="sms"},
                    new TypeNotification{Id=5, Name="VK"},
                    new TypeNotification{Id=6, Name="email"}
              });

        }
    }
}
