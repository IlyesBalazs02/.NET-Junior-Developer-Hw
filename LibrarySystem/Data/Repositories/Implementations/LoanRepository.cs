using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data.Repositories.Implementations
{
	public class LoanRepository : ILoanRepository
	{
		private readonly LibraryDbContext _context;

		public LoanRepository(LibraryDbContext context) => _context = context;

		public async Task CreateAsync(Loan loan, CancellationToken cancellationToken)
		{
			_context.Loans.Add(loan);
			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task<IReadOnlyList<Loan>> ReadAllActiveAsync(CancellationToken cancellationToken)
		{
			var query = _context.Loans
				.Include(l => l.Book)
				.Include(l => l.User)
				.Where(l => l.ReturnDate == null)
				.OrderBy(l => l.LoanDate);

			return await query.AsNoTracking().ToListAsync(cancellationToken);
		}

		public async Task<Loan?> ReadByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _context.Loans
			.Include(l => l.Book)
			.Include(l => l.User)
			.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

		public async Task UpdateAsync(Loan loan, CancellationToken cancellationToken)
		{
			_context.Loans.Update(loan);
			await _context.SaveChangesAsync(cancellationToken);
		}

	}
}
