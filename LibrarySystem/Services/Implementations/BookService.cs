using LibrarySystem.Data.Repositories.Interfaces;
using LibrarySystem.DTOs.Books;
using LibrarySystem.Exceptions;
using LibrarySystem.Mappings;
using LibrarySystem.Services.Interfaces;

namespace LibrarySystem.Services.Implementations
{
	public class BookService : IBookService
	{
		private readonly IBookRepository _bookRepo;

		public BookService(IBookRepository bookRepo)
		{
			_bookRepo = bookRepo;
		}

		public async Task<IReadOnlyList<BookDto>> ListAllAsync(BookFilterDto filterDto, CancellationToken cancellationToken)
		{
			var author = string.IsNullOrWhiteSpace(filterDto.Author) ? null : filterDto.Author.Trim();

			var books = await _bookRepo.ReadAllAsync(filterDto.IsAvailable, author, cancellationToken);

			return books.Select(b => b.ToDto()).ToList();
		}

		public async Task<BookDto> CreateAsync(CreateBookDto createBookDto, CancellationToken cancellationToken)
		{
			var newBook = createBookDto.ToEntity();

			await _bookRepo.CreateAsync(newBook, cancellationToken);

			return newBook.ToDto();
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			var book = await _bookRepo.GetByIdAsync(id, cancellationToken);

			if (book == null) throw new NotFoundException("Book not found.");
			if (!book.IsAvailable) throw new ConflictException("A book that is on loan cannot be deleted.");

			await _bookRepo.DeleteAsync(book, cancellationToken);
		}

		public async Task<BookDto> UpdateAsync(Guid id, UpdateBookDto updateDto, CancellationToken cancellationToken)
		{
			var book = await _bookRepo.GetByIdAsync(id, cancellationToken);

			if (book == null) throw new NotFoundException("Book not found.");

			book.Update(updateDto.Title, updateDto.Author, updateDto.ISBN, updateDto.PublishedYear);

			await _bookRepo.UpdateAsync(book, cancellationToken);

			return book.ToDto();
		}
	}
}
