using asp_mvc_crud.Models;
using Microsoft.EntityFrameworkCore;

namespace asp_mvc_crud.Data
{
    // DbContext
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Indexes
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique()
                .HasFilter("[ISBN] IS NOT NULL");

            // Configure Relationships
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrowing>()
                .HasOne(br => br.Member)
                .WithMany(m => m.Borrowings)
                .HasForeignKey(br => br.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrowing>()
                .HasOne(br => br.Book)
                .WithMany(b => b.Borrowings)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Fiction", Description = "Fictional books and novels" },
                new Category { CategoryId = 2, CategoryName = "Science", Description = "Science and technology books" },
                new Category { CategoryId = 3, CategoryName = "History", Description = "Historical books and biographies" },
                new Category { CategoryId = 4, CategoryName = "Programming", Description = "Computer programming and IT books" }
            );

            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, FirstName = "J.K.", LastName = "Rowling", Nationality = "British", BirthDate = new DateTime(1965, 7, 31) },
                new Author { AuthorId = 2, FirstName = "Stephen", LastName = "King", Nationality = "American", BirthDate = new DateTime(1947, 9, 21) },
                new Author { AuthorId = 3, FirstName = "Robert", LastName = "Martin", Nationality = "American", BirthDate = new DateTime(1952, 12, 5) },
                new Author { AuthorId = 4, FirstName = "Eric", LastName = "Evans", Nationality = "American" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Harry Potter and the Philosopher's Stone", ISBN = "9780747532699", AuthorId = 1, CategoryId = 1, PublicationYear = 1997, Publisher = "Bloomsbury", TotalCopies = 5, AvailableCopies = 3 },
                new Book { BookId = 2, Title = "The Shining", ISBN = "9780307743657", AuthorId = 2, CategoryId = 1, PublicationYear = 1977, Publisher = "Doubleday", TotalCopies = 3, AvailableCopies = 2 },
                new Book { BookId = 3, Title = "Clean Code", ISBN = "9780132350884", AuthorId = 3, CategoryId = 4, PublicationYear = 2008, Publisher = "Prentice Hall", TotalCopies = 4, AvailableCopies = 4 },
                new Book { BookId = 4, Title = "Domain-Driven Design", ISBN = "9780321125217", AuthorId = 4, CategoryId = 4, PublicationYear = 2003, Publisher = "Addison-Wesley", TotalCopies = 2, AvailableCopies = 1 }
            );

            // Seed Members
            modelBuilder.Entity<Member>().HasData(
                new Member { MemberId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", Phone = "081-234-5678", JoinDate = new DateTime(2025, 1, 5) },
                new Member { MemberId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", Phone = "081-876-5432", JoinDate = new DateTime(2025, 3, 5) },
                new Member { MemberId = 3, FirstName = "Mike", LastName = "Johnson", Email = "mike.j@email.com", JoinDate = new DateTime(2025, 5, 5) }
            );
        }
    }
}