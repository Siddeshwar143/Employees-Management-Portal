using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MVCDHProject.Models
***REMOVED***
    public class MVCCoreDbContext : IdentityDbContext<IdentityUser>
    ***REMOVED***
        public MVCCoreDbContext(DbContextOptions<MVCCoreDbContext> options) : base(options)
        ***REMOVED***
      ***REMOVED***

        public DbSet<Customer> Customers ***REMOVED*** get; set; ***REMOVED***

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        ***REMOVED***
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Customer>(entity =>
            ***REMOVED***
                entity.Property(e => e.Custid)
                      .UseIdentityColumn(105, 1);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
                entity.Property(e => e.photo).HasColumnName("Photo");
          ***REMOVED***);
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                ***REMOVED***
                    Custid = 101,
                    Name = "Sai",
                    Balance = 50000.00m,
                    City = "Delhi",
                    Status = true,
                    photo = null
              ***REMOVED***
                new Customer
                ***REMOVED***
                    Custid = 102,
                    Name = "Sonia",
                    Balance = 40000.00m,
                    City = "Mumbai",
                    Status = true,
                    photo = null
              ***REMOVED***
                new Customer
                ***REMOVED***
                    Custid = 103,
                    Name = "Pankaj",
                    Balance = 30000.00m,
                    City = "Chennai",
                    Status = true,
                    photo = null
              ***REMOVED***
                new Customer
                ***REMOVED***
                    Custid = 104,
                    Name = "Samuels",
                    Balance = 25000.00m,
                    City = "Bengaluru",
                    Status = true,
                    photo = null
              ***REMOVED***
            );
      ***REMOVED***
  ***REMOVED***
***REMOVED***
