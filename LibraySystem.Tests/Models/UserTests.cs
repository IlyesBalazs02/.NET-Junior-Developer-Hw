using LibrarySystem.Exceptions;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Tests.Models
{
	public class UserTests
	{
		[Fact]
		public void Constructor_ValidValues_NormalizesNameAndEmail()
		{
			var user = new User("  User Name ", " User@Example.COM ", DateTime.UtcNow);

			Assert.Equal("User Name", user.Name);
			Assert.Equal("user@example.com", user.Email);
			Assert.NotEqual(Guid.Empty, user.Id);
		}

		[Theory]
		[InlineData("")]
		[InlineData("   ")]
		public void Constructor_BlankName_Throws(string name)
		{
			Assert.Throws<ModelValidationException>(
				() => new User(name, "user@example.com", DateTime.UtcNow));
		}

		[Theory]
		[InlineData("")]
		[InlineData("not-an-email")]
		[InlineData("user@")]
		public void Constructor_InvalidEmail_Throws(string email)
		{
			Assert.Throws<ModelValidationException>(
				() => new User("User", email, DateTime.UtcNow));
		}
	}
}
