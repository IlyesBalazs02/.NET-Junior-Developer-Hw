using LibrarySystem.Exceptions;

namespace LibrarySystem.Models
{
	public class Loan
	{
		public Guid Id { get; private set; }

		public Guid UserId { get; private set; }
		public User User { get; private set; } = null!;

		public Guid BookId { get; private set; }
		public Book Book { get; private set; } = null!;

		public DateTime LoanDate { get; private set; }
		public DateTime? ReturnDate { get; private set; }

		private Loan() { }

		public Loan(Guid userId, Guid bookId, DateTime loanDate)
		{
			if (userId == Guid.Empty)
				throw new ModelValidationException("User id is required");
			if (bookId == Guid.Empty)
				throw new ModelValidationException("Book id is required");
			if (loanDate == default(DateTime))
				throw new ModelValidationException("Loan date is required");

			Id = Guid.NewGuid();
			UserId = userId;
			BookId = bookId;
			LoanDate = loanDate;
		}

		public void Return(DateTime returnDate)
		{
			if (ReturnDate is not null)
				throw new Exception("This loan has already been returned");
			if (returnDate < LoanDate)
				throw new ModelValidationException("Return date can't be earlier than the loan date");

			ReturnDate = returnDate;
		}
	}
}
