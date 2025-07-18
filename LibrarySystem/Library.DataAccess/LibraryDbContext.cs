
using Microsoft.EntityFrameworkCore;
using Library.Entities;

namespace Library.DataAccess
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions options) : base(options)
        {
        }

        // DbSet ile veritabanında her bir entity için tablo oluşturuyoruz.
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //HasOne ile loan sınıfı içerisinde bir user nesnesi var, WithMany ile User sınıfında birden çok Loan vardır, HasForeignKey ise Loan tablosundaki UserId ile User.Id' ye referans oluyor.
            modelBuilder.Entity<Loan>().HasOne(l => l.User).WithMany(u => u.Loans).HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Loan>().HasOne(l => l.Book).WithMany(b=>b.Loans).HasForeignKey(l => l.BookId);

        }
    }
}
