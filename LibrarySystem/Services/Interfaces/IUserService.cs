using LibrarySystem.DTOs.Users;

namespace LibrarySystem.Services.Interfaces
{
	public interface IUserService
	{
		Task<IReadOnlyList<UserDto>> ListAllAsync(CancellationToken cancellationToken);
		Task<UserDto> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);
	}
}