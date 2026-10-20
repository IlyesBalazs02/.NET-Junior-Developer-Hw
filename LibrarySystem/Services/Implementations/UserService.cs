using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.DTOs.Users;
using LibrarySystem.Mappings;
using LibrarySystem.Services.Interfaces;

namespace LibrarySystem.Services.Implementations
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<UserDto> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
		{
			var user = registerUserDto.ToEntity(DateTime.UtcNow);

			await _userRepository.CreateAsync(user, cancellationToken);

			return user.ToDto();
		}

		public async Task<IReadOnlyList<UserDto>> ListAllAsync(CancellationToken cancellationToken)
		{
			var users = await _userRepository.ReadAllAsync(cancellationToken);

			return users.Select(u => u.ToDto()).ToList();
		}
	}
}
