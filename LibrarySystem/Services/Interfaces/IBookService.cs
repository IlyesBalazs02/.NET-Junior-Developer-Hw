using LibrarySystem.DTOs.Books;

namespace LibrarySystem.Services.Interfaces
{
	public interface IBookService
	{
		Task<BookDto> CreateAsync(CreateBookDto createBookDto, CancellationToken cancellationToken);
		Task DeleteAsync(Guid id, CancellationToken cancellationToken);
		Task<IReadOnlyList<BookDto>> ListAllAsync(BookFilterDto filterDto, CancellationToken cancellationToken);
		Task<BookDto> UpdateAsync(Guid id, UpdateBookDto updateDto, CancellationToken cancellationToken);
		Task<BookDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	}
}