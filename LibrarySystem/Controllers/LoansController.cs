using LibrarySystem.DTOs.Books;
using LibrarySystem.DTOs.Loans;
using LibrarySystem.DTOs.Users;
using LibrarySystem.Services.Implementations;
using LibrarySystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LoansController : ControllerBase
	{
		private readonly ILoanService _loanService;

		public LoansController(ILoanService loanService)
		{
			_loanService = loanService;
		}

		[HttpGet("active")]
		[ProducesResponseType(typeof(IReadOnlyList<LoanDto>), StatusCodes.Status200OK)]
		public async Task<ActionResult<IReadOnlyList<LoanDto>>> GetAllActive(CancellationToken cancellationToken)
			=> Ok(await _loanService.ListActiveAsync(cancellationToken));

		[HttpGet("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<LoanDto>> GetById(Guid id, CancellationToken cancellationToken)
			=> Ok(await _loanService.GetByIdAsync(id, cancellationToken));

		[HttpPost]
		[ProducesResponseType(typeof(LoanDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult<LoanDto>> Borrow(BorrowDto borrowDto, CancellationToken cancellationToken)
		{
			var loan = await _loanService.BorrowAsync(borrowDto, cancellationToken);

			return CreatedAtAction(nameof(GetById), new {id =  loan.Id}, loan);
		}

		[HttpPut("{id:guid}/return")]
		[ProducesResponseType(typeof(LoanDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult<LoanDto>> Return(Guid id, CancellationToken cancellationToken)
			=> Ok(await _loanService.ReturnAsync(id, cancellationToken));
	}
}
