using LibrarySystem.DTOs.Books;
using LibrarySystem.DTOs.Users;
using LibrarySystem.Services.Implementations;
using LibrarySystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAllUser(CancellationToken cancellationToken)
			=> Ok(await _userService.ListAllAsync(cancellationToken));

		[HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken cancellationToken)
			=> Ok(await _userService.GetByIdAsync(id, cancellationToken));

		[HttpPost]
		[ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
		{
			var user = await _userService.RegisterAsync(registerUserDto, cancellationToken);

			return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
		}
	}
}
