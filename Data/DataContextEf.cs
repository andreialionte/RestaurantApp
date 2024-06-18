using Microsoft.EntityFrameworkCore;
using RestaurantApp.Models;

namespace RestaurantApp.Data
{
    public class DataContextEf : DbContext
    {
        private readonly IConfiguration _config;
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Auth> Auths { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DataContextEf(DbContextOptions<DataContextEf> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        /*        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                {
                    if (!optionsBuilder.IsConfigured)
                    {
                        optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
                    }

                }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Orders", "RestaurantAppSchema").HasKey(k => k.OrderId);
            modelBuilder.Entity<Order>().HasOne(p => p.Payment).WithOne(o => o.Order).HasForeignKey<Order>(o => o.PaymentId); // Explicitly specify the foreign key property
            modelBuilder.Entity<Order>().HasOne(u => u.User).WithMany(o => o.Orders).HasForeignKey(o => o.CustomerId);


            modelBuilder.Entity<Payment>().ToTable("Payments", "RestaurantAppSchema").HasKey(k => k.PaymentId);
            modelBuilder.Entity<Payment>().HasOne(o => o.Order).WithOne(p => p.Payment).HasForeignKey<Payment>(p => p.OrderId);
            /*            modelBuilder.Entity<Payment>().Property(p => p.TotalAmount).HasColumnType("decimal(19,4)");*/

            modelBuilder.Entity<Item>().ToTable("Items", "RestaurantAppSchema").HasKey(k => k.ItemId);
            modelBuilder.Entity<Item>().HasMany(o => o.Orders).WithMany(i => i.Items);
            modelBuilder.Entity<Item>().HasOne(m => m.Category).WithMany(i => i.Items);
            /*            modelBuilder.Entity<Item>().Property(p => p.Price).HasColumnType("decimal(19, 4)");*/

            modelBuilder.Entity<Category>().ToTable("Categories", "RestaurantAppSchema").HasKey(k => k.CategoryId);
            modelBuilder.Entity<Category>().HasMany(m => m.Items).WithOne(m => m.Category);

            modelBuilder.Entity<User>().ToTable("Users", "RestaurantAppSchema").HasKey(k => k.UserId);

            modelBuilder.Entity<Auth>().ToTable("Auth", "RestaurantAppSchema");

            base.OnModelCreating(modelBuilder);
        }
    }


}
