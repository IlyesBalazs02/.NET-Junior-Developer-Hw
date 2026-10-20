namespace LibrarySystem.DTOs.Loans
{
	public class LoanDto
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public string UserName { get; set; } = null!;

		public Guid BookId { get; set; }

		public string BookTitle { get; set; } = null!;

		public DateTime LoanDate { get; set; }

		public DateTime? ReturnDate { get; set; }
	}
}
