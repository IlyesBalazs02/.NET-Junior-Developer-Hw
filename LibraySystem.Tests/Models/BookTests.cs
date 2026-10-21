using LibrarySystem.Models;
using LibrarySystem.Exceptions;
using LibrarySystem.Models.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Tests.Models
{
	public class BookTests
	{
		private static Book CreateBook()
			=> new Book("Dune", "Frank Herbert", "9780441172719", 1965);

		[Fact]
		public void Constructor_ValidValues_CreatesAvailableBook()
		{
			var book = CreateBook();

			Assert.NotEqual(Guid.Empty, book.Id);
			Assert.True(book.IsAvailable);
			Assert.Equal("Dune", book.Title);
		}

		[Fact]
		public void Constructor_TrimsTitleAndAuthor()
		{
			var book = new Book("  Dune  ", "  Frank Herbert ", "9780441172719", 1965);

			Assert.Equal("Dune", book.Title);
			Assert.Equal("Frank Herbert", book.Author);
		}

		[Theory]
		[InlineData("")]
		[InlineData("   ")]
		public void Constructor_BlankTitle_Throws(string title)
		{
			Assert.Throws<ModelValidationException>(
				() => new Book(title, "Frank Herbert", "9780441172719", 1965));
		}

		[Fact]
		public void Constructor_TitleTooLong_Throws()
		{
			var title = new string('a', BookConstraints.TitleMaxLength + 1);

			Assert.Throws<ModelValidationException>(
				() => new Book(title, "Frank Herbert", "9780441172719", 1965));
		}

		[Theory]
		[InlineData("")]
		[InlineData("123")]
		[InlineData("97804411727190")]
		[InlineData("97804411727AB")]
		public void Constructor_InvalidIsbn_Throws(string isbn)
		{
			Assert.Throws<ModelValidationException>(
				() => new Book("Dune", "Frank Herbert", isbn, 1965));
		}

		[Fact]
		public void Constructor_YearInTheFuture_Throws()
		{
			var year = DateTime.UtcNow.Year + 1;

			Assert.Throws<ModelValidationException>(
				() => new Book("Dune", "Frank Herbert", "9780441172719", year));
		}

		[Fact]
		public void Constructor_YearTooOld_Throws()
		{
			Assert.Throws<ModelValidationException>(
				() => new Book("Dune", "Frank Herbert", "9780441172719", BookConstraints.MinPublishedYear - 1));
		}

		[Fact]
		public void Update_ValidValues_ChangesProperties()
		{
			var book = CreateBook();

			book.Update("The Hobbit", "J.R.R. Tolkien", "9780547928227", 1937);

			Assert.Equal("The Hobbit", book.Title);
			Assert.Equal("J.R.R. Tolkien", book.Author);
			Assert.Equal("9780547928227", book.ISBN);
			Assert.Equal(1937, book.PublishedYear);
		}

		[Fact]
		public void Update_InvalidValue_ChangesNothing()
		{
			var book = CreateBook();

			Assert.Throws<ModelValidationException>(
				() => book.Update("The Hobbit", "J.R.R. Tolkien", "bad-isbn", 1937));

			// validation runs before assignment, so nothing is half-changed
			Assert.Equal("Dune", book.Title);
			Assert.Equal("Frank Herbert", book.Author);
		}

		[Fact]
		public void MarkAsBorrowed_AvailableBook_BecomesUnavailable()
		{
			var book = CreateBook();

			book.MarkAsBorrowed();

			Assert.False(book.IsAvailable);
		}

		[Fact]
		public void MarkAsBorrowed_AlreadyBorrowed_ThrowsConflict()
		{
			var book = CreateBook();
			book.MarkAsBorrowed();

			Assert.Throws<ConflictException>(() => book.MarkAsBorrowed());
		}

		[Fact]
		public void MarkAsReturned_BorrowedBook_BecomesAvailable()
		{
			var book = CreateBook();
			book.MarkAsBorrowed();

			book.MarkAsReturned();

			Assert.True(book.IsAvailable);
		}

		[Fact]
		public void MarkAsReturned_NotBorrowed_ThrowsConflict()
		{
			var book = CreateBook();

			Assert.Throws<ConflictException>(() => book.MarkAsReturned());
		}
	}
}
