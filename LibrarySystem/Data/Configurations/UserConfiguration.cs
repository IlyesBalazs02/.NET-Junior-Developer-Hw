using LibrarySystem.Models;
using LibrarySystem.Models.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.Data.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(UserConstraints.NameMaxLength);

			builder.Property(x => x.Email)
				.IsRequired()
				.HasMaxLength(UserConstraints.EmailMaxLength);
		}
	}
}
