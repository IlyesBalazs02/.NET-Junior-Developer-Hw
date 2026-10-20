using LibrarySystem.DTOs.Loans;

namespace LibrarySystem.Services.Interfaces
{
	public interface ILoanService
	{
		Task<LoanDto> BorrowAsync(BorrowDto borrowDto, CancellationToken cancellationToken);
		Task<IReadOnlyList<LoanDto>> ListActiveAsync(CancellationToken cancellationToken);
		Task<LoanDto> ReturnAsync(Guid id, CancellationToken cancellationToken);
		Task<LoanDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	}
}