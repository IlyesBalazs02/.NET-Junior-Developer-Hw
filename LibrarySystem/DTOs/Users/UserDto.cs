using LibrarySystem.Models.Constraints;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Users
{
	public class UserDto
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string Email { get; set; } = null!;

		public DateTime RegisteredDate { get; set; }
	}
}
