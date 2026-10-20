using LibrarySystem.DTOs.Books;
using LibrarySystem.Models;

namespace LibrarySystem.Mappings
{
	public static class BookMappings
	{
		public static BookDto ToDto(this Book book) => new()
		{
			Id = book.Id,
			Title = book.Title,
			Author = book.Author,
			ISBN = book.ISBN,
			PublishedYear = book.PublishedYear,
			IsAvailable = book.IsAvailable
		};

		public static Book ToEntity(this CreateBookDto createBookDto)
			=> new Book(createBookDto.Title, createBookDto.Author, createBookDto.ISBN, createBookDto.PublishedYear);
	}
}
