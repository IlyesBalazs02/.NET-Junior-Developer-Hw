using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.DTOs.Loans;
using LibrarySystem.Exceptions;
using LibrarySystem.Mappings;
using LibrarySystem.Models;
using LibrarySystem.Services.Interfaces;

namespace LibrarySystem.Services.Implementations
{
	public class LoanService : ILoanService
	{
		private readonly ILoanRepository _loanRepository;
		private readonly IUserRepository _userRepository;
		private readonly IBookRepository _bookRepository;

		public LoanService(ILoanRepository loanRepository, IUserRepository userRepository, IBookRepository bookRepository)
		{
			_loanRepository = loanRepository;
			_userRepository = userRepository;
			_bookRepository = bookRepository;
		}

		public async Task<LoanDto> BorrowAsync(BorrowDto borrowDto, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByIdAsync(borrowDto.UserId, cancellationToken);
			var book = await _bookRepository.GetByIdAsync(borrowDto.BookId, cancellationToken);

			if (user == null) throw new NotFoundException("User not found.");
			if (book == null) throw new NotFoundException("Book not found.");

			book.MarkAsBorrowed();
			var newLoan = new Loan(user.Id, book.Id, DateTime.UtcNow);

			await _loanRepository.CreateAsync(newLoan, cancellationToken);

			return newLoan.ToDto();
		}

		public async Task<LoanDto> ReturnAsync(Guid id, CancellationToken cancellationToken)
		{
			var loan = await _loanRepository.ReadByIdAsync(id, cancellationToken);

			if (loan == null) throw new NotFoundException("Loan not found.");

			loan.Return(DateTime.UtcNow);
			loan.Book.MarkAsReturned();

			await _loanRepository.UpdateAsync(loan, cancellationToken);

			return loan.ToDto();
		}

		public async Task<IReadOnlyList<LoanDto>> ListActiveAsync(CancellationToken cancellationToken)
		{
			var loans = await _loanRepository.ReadAllActiveAsync(cancellationToken);
			return loans.Select(l => l.ToDto()).ToList();
		}
	}
}
