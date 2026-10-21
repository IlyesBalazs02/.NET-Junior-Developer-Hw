using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.DTOs.Users;
using LibrarySystem.Exceptions;
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

			if (await _userRepository.ExistsByEmailAsync(user.Email, cancellationToken))
				throw new ConflictException("A user with this email already exists.");

			await _userRepository.CreateAsync(user, cancellationToken);

			return user.ToDto();
		}

		public async Task<IReadOnlyList<UserDto>> ListAllAsync(CancellationToken cancellationToken)
		{
			var users = await _userRepository.ReadAllAsync(cancellationToken);

			return users.Select(u => u.ToDto()).ToList();
		}

		public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByIdAsync(id, cancellationToken);

			if (user == null) throw new NotFoundException("User not found");

			return user.ToDto();
		}
	}
}
