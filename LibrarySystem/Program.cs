
using LibrarySystem.Data;
using LibrarySystem.Data.Repositories.Implementations;
using LibrarySystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<LibraryDbContext>(options => options.UseInMemoryDatabase("LibraryDb"));

			builder.Services.AddScoped<IBookRepository, BookRepository>();
			builder.Services.AddScoped<ILoanRepository, LoanRepository>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();

			var app = builder.Build();

			using (var scope = app.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
				DbInitializer.Initialize(context);
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
