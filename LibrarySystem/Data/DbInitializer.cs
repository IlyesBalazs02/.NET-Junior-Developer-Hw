using LibrarySystem.Models;

namespace LibrarySystem.Data
{
	public static class DbInitializer
	{
		public static void Initialize(LibraryDbContext context)
		{
			var books = new[]
			{
				new Book("Dune", "Frank Herbert", "0123456789123", 1965),
				new Book("The Hobbit", "J.R.R. Tolkien", "9780547928227", 1937),
				new Book("Fahrenheit 451", "Ray Bradbury", "9780441569595", 1953)
			};

			var users = new[]
			{
				new User("User1", "user1@example.com", DateTime.UtcNow.AddDays(-30)),
				new User("User2", "user2@example.com", DateTime.UtcNow.AddDays(-10))
			};

			books[0].MarkAsBorrowed();
			var loan = new Loan(users[0].Id, books[0].Id, DateTime.UtcNow.AddDays(-3));

			context.Books.AddRange(books);
			context.Users.AddRange(users);
			context.Loans.Add(loan);

			context.SaveChanges();
		}
	}
}
