using LibrarySystem.Models.Constraints;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Books
{
	public class UpdateBookDto
	{
		[Required, StringLength(BookConstraints.TitleMaxLength)]
		public string Title { get; set; } = null!;

		[Required, StringLength(BookConstraints.AuthorMaxLength)]
		public string Author { get; set; } = null!;

		[Required, RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN must be 13 digits.")]
		public string ISBN { get; set; } = null!;

		[Range(BookConstraints.MinPublishedYear, 2100)]
		public int PublishedYear { get; set; }
	}
}
