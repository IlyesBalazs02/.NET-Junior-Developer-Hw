using LibrarySystem.Exceptions;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Tests.Models
{
	public class LoanTests
	{
		private static readonly DateTime LoanDate = new(2026, 10, 1, 12, 0, 0, DateTimeKind.Utc);

		private static Loan CreateLoan()
			=> new Loan(Guid.NewGuid(), Guid.NewGuid(), LoanDate);

		[Fact]
		public void Constructor_ValidValues_CreatesActiveLoan()
		{
			var loan = CreateLoan();

			Assert.NotEqual(Guid.Empty, loan.Id);
			Assert.Null(loan.ReturnDate);
		}

		[Fact]
		public void Constructor_EmptyUserId_Throws()
		{
			Assert.Throws<ModelValidationException>(
				() => new Loan(Guid.Empty, Guid.NewGuid(), LoanDate));
		}

		[Fact]
		public void Constructor_EmptyBookId_Throws()
		{
			Assert.Throws<ModelValidationException>(
				() => new Loan(Guid.NewGuid(), Guid.Empty, LoanDate));
		}

		[Fact]
		public void Return_ActiveLoan_SetsReturnDate()
		{
			var loan = CreateLoan();
			var returnDate = LoanDate.AddDays(5);

			loan.Return(returnDate);

			Assert.Equal(returnDate, loan.ReturnDate);
		}

		[Fact]
		public void Return_AlreadyReturned_ThrowsConflict()
		{
			var loan = CreateLoan();
			loan.Return(LoanDate.AddDays(1));

			Assert.Throws<ConflictException>(() => loan.Return(LoanDate.AddDays(2)));
		}

		[Fact]
		public void Return_DateBeforeLoanDate_Throws()
		{
			var loan = CreateLoan();

			Assert.Throws<ModelValidationException>(() => loan.Return(LoanDate.AddDays(-1)));
		}
	}
}
