using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.Data.Configurations
{
	public class LoanConfiguration : IEntityTypeConfiguration<Loan>
	{
		public void Configure(EntityTypeBuilder<Loan> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.User)
				.WithMany(y => y.Loans)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.Book)
				.WithMany(y => y.Loans)
				.HasForeignKey(x => x.BookId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
