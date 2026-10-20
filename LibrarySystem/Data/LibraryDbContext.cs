using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data
{
	public class LibraryDbContext : DbContext
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<Loan> Loans { get; set; }
		public DbSet<User> Users { get; set; }
		public LibraryDbContext(DbContextOptions<LibraryDbContext> opt) : base(opt) 
		{ 

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
		}

	}
}
