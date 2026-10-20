using LibrarySystem.Models.Constraints;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Books
{
	public class BookDto
	{
		public Guid Id { get; set; }

		public string Title { get; set; } = null!;

		public string Author { get; set; } = null!;

		public string ISBN { get; set; } = null!;

		public int PublishedYear { get; set; }

		public bool IsAvailable { get; set; }
	}
}
