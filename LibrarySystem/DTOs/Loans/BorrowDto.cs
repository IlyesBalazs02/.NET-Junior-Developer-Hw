using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Loans
{
	public class BorrowDto
	{
		[Required]
		public Guid UserId { get; set; }

		[Required]
		public Guid BookId { get; set; }
	}
}
