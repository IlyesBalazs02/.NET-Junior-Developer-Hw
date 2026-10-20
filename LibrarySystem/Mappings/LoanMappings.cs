using LibrarySystem.DTOs.Loans;
using LibrarySystem.Models;

namespace LibrarySystem.Mappings
{
	public static class LoanMappings
	{
		public static LoanDto ToDto(this Loan loan) => new()
		{
			Id = loan.Id,
			UserId = loan.UserId,
			UserName = loan.User.Name,
			BookId = loan.BookId,
			BookTitle = loan.Book.Title,
			LoanDate = loan.LoanDate,
			ReturnDate = loan.ReturnDate
		};

		public static Loan ToEntity(this BorrowDto borrowDto, DateTime loanDate)
			=> new Loan(borrowDto.UserId, borrowDto.BookId, loanDate);
	}
}
