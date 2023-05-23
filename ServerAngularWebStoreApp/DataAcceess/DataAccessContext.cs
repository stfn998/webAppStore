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
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Web2-PersonDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        // }
    }
}