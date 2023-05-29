using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAcceess
{
    public class DataAccessContext : DbContext
    {
        public DataAccessContext()
        { }

        public DataAccessContext(DbContextOptions<DataAccessContext> options) :
            base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users")
                .HasOne(u => u.Person)
                .WithOne(p => p.User)
                .HasForeignKey<Person>(p => p.IdUser);
            modelBuilder.Entity<User>()
                .Property(x => x.Id).IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.UserName).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Person>()
                .Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Person>()
                .Property(x => x.IdUser).IsRequired();
            modelBuilder.Entity<Person>()
                .Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Person>()
                .Property(x => x.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Person>()
                .Property(x => x.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Person>()
                .Property(x => x.Address).HasMaxLength(2000);
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Products)
                .WithOne(b => b.Seller)
                .HasForeignKey(b => b.SellerId);
            modelBuilder.Entity<Person>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Product>()
            .Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Product>()
                .Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Product>()
                .Property(x => x.Price).IsRequired();
            modelBuilder.Entity<Product>()
                .Property(x => x.Quantity).IsRequired();

            modelBuilder.Entity<Order>()
                .Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Order>()
                .Property(x => x.OrderDate).IsRequired();
            modelBuilder.Entity<Order>()
                .Property(x => x.DeliveryAddress).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>()
                .Property(x => x.Quantity).IsRequired();
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Web2-PersonDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        // }
    }
}