using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.DTOs.Books;
using LibrarySystem.Exceptions;
using LibrarySystem.Models;
using LibrarySystem.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Tests.Services
{
	public class BookServiceTests
	{
		private readonly Mock<IBookRepository> _books = new();
		private readonly BookService _service;

		public BookServiceTests() => _service = new BookService(_books.Object);

		private static Book CreateBook()
			=> new Book("Dune", "Frank Herbert", "9780441172719", 1965);

		[Fact]
		public async Task ListAllAsync_TrimsAuthorBeforeCallingRepository()
		{
			_books.Setup(r => r.ReadAllAsync(It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((IReadOnlyList<Book>)new List<Book>());

			await _service.ListAllAsync(new BookFilterDto { IsAvailable = true, Author = "  Tolkien " }, default);

			_books.Verify(r => r.ReadAllAsync(true, "Tolkien", It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task ListAllAsync_BlankAuthor_IsTreatedAsNoFilter()
		{
			_books.Setup(r => r.ReadAllAsync(It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((IReadOnlyList<Book>)new List<Book>());

			await _service.ListAllAsync(new BookFilterDto { Author = "   " }, default);

			_books.Verify(r => r.ReadAllAsync(null, null, It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task ListAllAsync_MapsBooksToDtos()
		{
			var book = CreateBook();
			_books.Setup(r => r.ReadAllAsync(It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((IReadOnlyList<Book>)new List<Book> { book });

			var result = await _service.ListAllAsync(new BookFilterDto(), default);

			var dto = Assert.Single(result);
			Assert.Equal(book.Id, dto.Id);
			Assert.Equal("Dune", dto.Title);
		}

		[Fact]
		public async Task DeleteAsync_BookNotFound_ThrowsNotFound()
		{
			_books.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Book?)null);

			await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(Guid.NewGuid(), default));
		}

		[Fact]
		public async Task DeleteAsync_BookOnLoan_ThrowsConflictAndDeletesNothing()
		{
			var book = CreateBook();
			book.MarkAsBorrowed();
			_books.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);

			await Assert.ThrowsAsync<ConflictException>(() => _service.DeleteAsync(book.Id, default));

			_books.Verify(r => r.DeleteAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task DeleteAsync_AvailableBook_DeletesIt()
		{
			var book = CreateBook();
			_books.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);

			await _service.DeleteAsync(book.Id, default);

			_books.Verify(r => r.DeleteAsync(book, It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task UpdateAsync_BookNotFound_ThrowsNotFound()
		{
			_books.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Book?)null);

			var dto = new UpdateBookDto { Title = "x", Author = "y", ISBN = "9780441172719", PublishedYear = 1965 };

			await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(Guid.NewGuid(), dto, default));
		}

		[Fact]
		public async Task UpdateAsync_ValidDto_ChangesBookAndSaves()
		{
			var book = CreateBook();
			_books.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);

			var dto = new UpdateBookDto { Title = "The Hobbit", Author = "J.R.R. Tolkien", ISBN = "9780547928227", PublishedYear = 1937 };

			var result = await _service.UpdateAsync(book.Id, dto, default);

			Assert.Equal("The Hobbit", result.Title);
			_books.Verify(r => r.UpdateAsync(book, It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}
