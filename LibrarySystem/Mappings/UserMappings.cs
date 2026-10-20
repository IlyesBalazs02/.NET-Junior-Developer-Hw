using LibrarySystem.DTOs.Users;
using LibrarySystem.Models;

namespace LibrarySystem.Mappings
{
	public static class UserMappings
	{
		public static UserDto ToDto(this User user) => new()
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			RegisteredDate = user.RegisteredDate
		};

		public static User ToEntity(this RegisterUserDto registerUserDto, DateTime registeredDate)
			=> new User(registerUserDto.Name, registerUserDto.Email, registeredDate);
	}
}
