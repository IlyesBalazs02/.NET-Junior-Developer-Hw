using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task CreateAsync(User user, CancellationToken cancellationToken);
		Task<IReadOnlyList<User>> ReadAllAsync(CancellationToken cancellationToken);
	}
}