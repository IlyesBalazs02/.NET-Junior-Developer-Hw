using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LibrarySystem.Data.Repositories.Implementations
{
	public class UserRepository : IUserRepository
	{
		private readonly LibraryDbContext _context;

		public UserRepository(LibraryDbContext context) => _context = context;

		public async Task CreateAsync(User user, CancellationToken cancellationToken)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task<IReadOnlyList<User>> ReadAllAsync(CancellationToken cancellationToken)
		{
			return await _context.Users
				.AsNoTracking()
				.ToListAsync(cancellationToken);
		}

		public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

	}
}
