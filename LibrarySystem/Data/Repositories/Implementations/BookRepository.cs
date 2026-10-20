using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Repositories.Implementations
{
	public class BookRepository : IBookRepository
	{
		private readonly LibraryDbContext _context;

		public BookRepository(LibraryDbContext context) => _context = context;

		public async Task CreateAsync(Book book, CancellationToken cancellationToken)
		{
			_context.Books.Add(book);
			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task<IReadOnlyList<Book>> ReadAllAsync(bool? isAvailable, string? author, CancellationToken cancellationToken)
		{
			var query = _context.Books.AsQueryable();

			if (isAvailable.HasValue) query = query.Where(b => b.IsAvailable == isAvailable.Value);
			if (!string.IsNullOrWhiteSpace(author)) query = query.Where(b => b.Author.ToLower().Contains(author.ToLower()));

			query = query.OrderBy(b => b.Title);

			return await query.AsNoTracking().ToListAsync(cancellationToken);
		}

		public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _context.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

		public async Task UpdateAsync(Book book, CancellationToken cancellationToken)
		{
			_context.Books.Update(book);
			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task DeleteAsync(Book book, CancellationToken cancellationToken)
		{
			_context.Books.Remove(book);
			await _context.SaveChangesAsync(cancellationToken);
		}

	}
}
