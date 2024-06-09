using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class G14DB : DbContext
    {
        public G14DB(DbContextOptions<G14DB> options) : base(options) { }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Paper> Papers { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Card)
                .WithOne(c => c.Owner)
                .HasForeignKey<Card>(c => c.OwnerId);

            // Add initial Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "Xavier",
                    LastName = "Barmaz",
                    Email = "xavier.barmaz@example.com",
                    Username = "bax",
                    Password = "bax",
                    UserType = "Professor"
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Arnaud",
                    LastName = "Auchere",
                    Email = "arnaud.auchere@example.com",
                    Username = "arnoauch",
                    Password = "arnoauch",
                    UserType = "Student"
                },
                new User
                {
                    UserId = 3,
                    FirstName = "Maria",
                    LastName = "McKinsey",
                    Email = "maria.mckinsey@example.com",
                    Username = "mariamc",
                    Password = "mariamc",
                    UserType = "Employee"
                },
                new User
                {
                    UserId = 4,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@admin.com",
                    Username = "admin",
                    Password = "admin",
                    UserType = "Admin"
                }
            );

            modelBuilder.Entity<Card>().HasData(
                new Card
                {
                    CardId = 1,
                    CardNumber = "272727272",
                    Balance = 100,
                    OwnerId = 1
                },
                new Card
                {
                    CardId = 2,
                    CardNumber = "383838383",
                    Balance = 200,
                    OwnerId = 2
                },
                new Card
                {
                    CardId = 3,
                    CardNumber = "5757575757",
                    Balance = 150,
                    OwnerId = 3
                },
                new Card
                {
                    CardId = 4,
                    CardNumber = "191919191",
                    Balance = 15,
                    OwnerId = 4
                }
            );

            modelBuilder.Entity<Printer>()
                .HasMany(p => p.Papers)
                .WithOne(p => p.Printer)
                .HasForeignKey(p => p.PrinterId);

            // Add initial Printers
            modelBuilder.Entity<Printer>().HasData(
                new Printer
                {
                    PrinterId = 1,
                    Name = "Floor 1"
                },
                new Printer
                {
                    PrinterId = 2,
                    Name = "Floor 2"
                },
                new Printer
                {
                    PrinterId = 3,
                    Name = "Floor 3"
                },
                new Printer
                {
                    PrinterId = 4,
                    Name = "Floor 4"
                }
            );

            // Add initial Papers
            modelBuilder.Entity<Paper>().HasData(
                new Paper
                {
                    PaperId = 1,
                    Type = "A4",
                    Amount = 1000,
                    Value = 0.15m,
                    PrinterId = 1
                },
                new Paper
                {
                    PaperId = 2,
                    Type = "A3",
                    Amount = 1000,
                    Value = 0.30m,
                    PrinterId = 1
                },
                new Paper
                {
                    PaperId = 3,
                    Type = "A4",
                    Amount = 1000,
                    Value = 0.15m,
                    PrinterId = 2
                },
                new Paper
                {
                    PaperId = 4,
                    Type = "A3",
                    Amount = 250,
                    Value = 0.30m,
                    PrinterId = 2
                },
                new Paper
                {
                    PaperId = 5,
                    Type = "A4",
                    Amount = 1000,
                    Value = 0.15m,
                    PrinterId = 3
                },
                new Paper
                {
                    PaperId = 6,
                    Type = "A3",
                    Amount = 1000,
                    Value = 0.30m,
                    PrinterId = 3
                },
                new Paper
                {
                    PaperId = 7,
                    Type = "A3",
                    Amount = 1000,
                    Value = 0.30m,
                    PrinterId = 4
                },
                new Paper
                {
                    PaperId = 8,
                    Type = "A4",
                    Amount = 1000,
                    Value = 0.15m,
                    PrinterId = 4
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}