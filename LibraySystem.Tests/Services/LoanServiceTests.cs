using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.DTOs.Loans;
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
	public class LoanServiceTests
	{
		private readonly Mock<ILoanRepository> _loans = new();
		private readonly Mock<IUserRepository> _users = new();
		private readonly Mock<IBookRepository> _books = new();
		private readonly LoanService _service;

		public LoanServiceTests()
			=> _service = new LoanService(_loans.Object, _users.Object, _books.Object);

		private static User CreateUser() => new User("Anna", "anna@example.com", DateTime.UtcNow);
		private static Book CreateBook() => new Book("Dune", "Frank Herbert", "9780441172719", 1965);

		[Fact]
		public async Task BorrowAsync_UserNotFound_ThrowsNotFound()
		{
			var book = CreateBook();
			_users.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
			_books.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);

			var dto = new BorrowDto { UserId = Guid.NewGuid(), BookId = book.Id };

			await Assert.ThrowsAsync<NotFoundException>(() => _service.BorrowAsync(dto, default));
			_loans.Verify(r => r.CreateAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task BorrowAsync_BookNotFound_ThrowsNotFound()
		{
			var user = CreateUser();
			_users.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
			_books.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Book?)null);

			var dto = new BorrowDto { UserId = user.Id, BookId = Guid.NewGuid() };

			await Assert.ThrowsAsync<NotFoundException>(() => _service.BorrowAsync(dto, default));
			_loans.Verify(r => r.CreateAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task BorrowAsync_BookAlreadyOnLoan_ThrowsConflictAndSavesNothing()
		{
			var user = CreateUser();
			var book = CreateBook();
			book.MarkAsBorrowed();
			_users.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
			_books.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);

			var dto = new BorrowDto { UserId = user.Id, BookId = book.Id };

			await Assert.ThrowsAsync<ConflictException>(() => _service.BorrowAsync(dto, default));
			_loans.Verify(r => r.CreateAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task ReturnAsync_LoanNotFound_ThrowsNotFound()
		{
			_loans.Setup(r => r.ReadByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Loan?)null);

			await Assert.ThrowsAsync<NotFoundException>(() => _service.ReturnAsync(Guid.NewGuid(), default));
			_loans.Verify(r => r.UpdateAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task ListActiveAsync_NoActiveLoans_ReturnsEmptyList()
		{
			_loans.Setup(r => r.ReadAllActiveAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync((IReadOnlyList<Loan>)new List<Loan>());

			var result = await _service.ListActiveAsync(default);

			Assert.Empty(result);
		}
	}
}
