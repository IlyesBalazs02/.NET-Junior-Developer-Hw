using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories.Interfaces
{
	public interface IBookRepository
	{
		Task CreateAsync(Book book, CancellationToken cancellationToken);
		Task DeleteAsync(Book book, CancellationToken cancellationToken);
		Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
		Task<IReadOnlyList<Book>> ReadAllAsync(bool? isAvailable, string? author, CancellationToken cancellationToken);
		Task UpdateAsync(Book book, CancellationToken cancellationToken);
	}
}