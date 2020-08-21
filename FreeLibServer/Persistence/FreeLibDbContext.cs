using FreeLibServer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeLibServer.Persistence
{
    public class FreeLibDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public FreeLibDbContext(DbContextOptions<FreeLibDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.BookId, ba.AuthorId });
            modelBuilder.Entity<BookGenre>().HasKey(bg => new { bg.BookId, bg.GenreId });
        }
    }
}
