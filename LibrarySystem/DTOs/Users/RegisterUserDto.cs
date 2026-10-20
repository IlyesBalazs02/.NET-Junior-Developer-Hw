using LibrarySystem.Models.Constraints;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Users
{
	public class RegisterUserDto
	{
		[Required, StringLength(UserConstraints.NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required, EmailAddress, StringLength(UserConstraints.EmailMaxLength)]
		public string Email { get; set; } = null!;
	}
}
