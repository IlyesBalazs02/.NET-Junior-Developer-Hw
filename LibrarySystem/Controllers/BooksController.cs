using LibrarySystem.DTOs.Books;
using LibrarySystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BooksController : ControllerBase
	{
		private readonly IBookService _bookService;

		public BooksController(IBookService bookService)
		{
			_bookService = bookService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyList<BookDto>), StatusCodes.Status200OK)]
		public async Task<ActionResult<IReadOnlyList<BookDto>>> GetAllBook([FromQuery] BookFilterDto filter, CancellationToken cancellationToken)
			=> Ok(await _bookService.ListAllAsync(filter, cancellationToken));

		[HttpPost]
		[ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BookDto>> Create(CreateBookDto dto, CancellationToken cancellationToken)
		{
			var book = await _bookService.CreateAsync(dto, cancellationToken);

			return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
		}

		[HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<BookDto>> GetById(Guid id, CancellationToken cancellationToken)
			=> Ok(await _bookService.GetByIdAsync(id, cancellationToken));


		[HttpPut("{id:guid}")]
		[ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<BookDto>> Update(Guid id, UpdateBookDto dto, CancellationToken cancellationToken)
		 => Ok(await _bookService.UpdateAsync(id, dto, cancellationToken));

		[HttpDelete("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
		{
			await _bookService.DeleteAsync(id, cancellationToken);
			return NoContent();
		}

	}
}
