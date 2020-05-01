using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Sales.Models;

namespace Telegram.Bot.Sales.EF
{

    public class ApplicationContext : DbContext
    {
        public string ConnString { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            //Recieve connection string to DB
            //ConnString = GetConnectionStringToDB.Execute();
            //ConnString = "Data Source=SQL6001.site4now.net;Initial Catalog=DB_A4878F_mysite;User Id=DB_A4878F_mysite_admin;Password=823537148Jon2k";
            // ConnString = "Data Source=EVGENIY-PC\\SQLEXPRESS;Initial Catalog=discount2; Integrated Security=true";
            //ConnString = "workstation id=discount.mssql.somee.com;packet size=4096;user id=jon2k_SQLLogin_1;pwd=k5vn16q426;data source=discount.mssql.somee.com;persist security info=False;initial catalog=discount;multipleActiveResultSets=True";

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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //Use provider to MSSQL
        //    optionsBuilder.UseSqlServer(ConnString);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(b => b.Url)
                .IsUnique();

            modelBuilder.Entity<Shop>().HasData(
                new Shop[]
                {
                    new Shop { Id=1, Name="WB", Url="www.wildberries.ru"},
                    new Shop { Id=2, Name="Ozon", Url="www.ozon.ru"},
                    new Shop { Id=3, Name="Amazon", Url="www.amazon.com"}
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
