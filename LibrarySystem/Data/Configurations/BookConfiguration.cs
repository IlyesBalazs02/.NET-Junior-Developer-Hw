using LibrarySystem.Models;
using LibrarySystem.Models.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.Data.Configurations
{
	public class BookConfiguration : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Title)
				.IsRequired()
				.HasMaxLength(BookConstraints.TitleMaxLength);

			builder.Property(x => x.Author)
				.IsRequired()
				.HasMaxLength(BookConstraints.AuthorMaxLength);

			builder.Property(x => x.ISBN)
				.IsRequired()
				.HasMaxLength(BookConstraints.IsbnLength);				
		}
	}
}
