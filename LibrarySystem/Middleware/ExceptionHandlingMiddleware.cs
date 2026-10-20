using System.Text.Json;
using LibrarySystem.Exceptions;

namespace LibrarySystem.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				var statusCode = ex switch
				{
					ModelValidationException => StatusCodes.Status400BadRequest,
					NotFoundException => StatusCodes.Status404NotFound,
					ConflictException => StatusCodes.Status409Conflict,
					_ => StatusCodes.Status500InternalServerError
				};

				if (statusCode == StatusCodes.Status500InternalServerError)
					_logger.LogError(ex, "Unhandled exception");

				httpContext.Response.ContentType = "application/json";
				httpContext.Response.StatusCode = statusCode;

				await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
				{
					message = statusCode == StatusCodes.Status500InternalServerError ? "internal server error" : ex.Message, statusCode
				}));
			}
		}
	}
}