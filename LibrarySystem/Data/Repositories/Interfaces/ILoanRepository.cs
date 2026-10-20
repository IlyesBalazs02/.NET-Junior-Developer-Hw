using LibrarySystem.Models;

namespace LibrarySystem.Data.Repositories.Interfaces
{
	public interface ILoanRepository
	{
		Task CreateAsync(Loan loan, CancellationToken cancellationToken);
		Task<IReadOnlyList<Loan>> ReadAllActiveAsync(CancellationToken cancellationToken);
		Task<Loan?> ReadByIdAsync(Guid id, CancellationToken cancellationToken);
		Task UpdateAsync(Loan loan, CancellationToken cancellationToken);
	}
}