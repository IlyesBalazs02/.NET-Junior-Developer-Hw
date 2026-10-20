using LibrarySystem.Exceptions;
using LibrarySystem.Models.Constraints;

namespace LibrarySystem.Models
{
	public class Book
	{
		public Guid Id { get; private set; }

		public string Title { get; private set; } = null!;

		public string Author { get; private set; } = null!;

		public string ISBN { get; private set; } = null!;

		public int PublishedYear { get; private set; }

		public bool IsAvailable { get; private set; }


		private readonly List<Loan> _loans = new List<Loan>();
		public IReadOnlyCollection<Loan> Loans => _loans;

		private Book() { }

		public Book(string title, string author, string isbn, int publishedYear)
		{
			Id = Guid.NewGuid();
			IsAvailable = true;
			SetProperties(title, author, isbn, publishedYear);
		}

		public void Update(string title, string author, string isbn, int publishedYear)
		=> SetProperties(title, author, isbn, publishedYear);

		public void Borrowed()
		{
			if (!IsAvailable)
				throw new ModelValidationException("Book is already borrowed");
			IsAvailable = false;
		}

		public void Returned()
		{
			if (IsAvailable)
				throw new ModelValidationException("Book is not on loan");
			IsAvailable = true;
		}

		private void SetProperties(string title, string author, string isbn, int publishedYear)
		{
			// Validate everything first
			if (string.IsNullOrWhiteSpace(title))
				throw new ModelValidationException("Title is required");
			if (title.Length > BookConstraints.TitleMaxLength)
				throw new ModelValidationException($"Title can be at most {BookConstraints.TitleMaxLength} characters");

			if (string.IsNullOrWhiteSpace(author))
				throw new ModelValidationException("Author is required");
			if (author.Length > BookConstraints.AuthorMaxLength)
				throw new ModelValidationException($"Author can be at most {BookConstraints.AuthorMaxLength} characters");

			if (string.IsNullOrWhiteSpace(isbn) || isbn.Length != BookConstraints.IsbnLength || !isbn.All(char.IsDigit))
				throw new ModelValidationException($"ISBN must be {BookConstraints.IsbnLength} characters");

			if (publishedYear < BookConstraints.MinPublishedYear || publishedYear > DateTime.UtcNow.Year)
				throw new ModelValidationException("Published year is out of range");

			// Assign only if all checks passed
			Title = title.Trim();
			Author = author.Trim();
			ISBN = isbn.Trim();
			PublishedYear = publishedYear;
		}
	}

}
