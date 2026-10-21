using LibrarySystem.Exceptions;
using LibrarySystem.Models.Constraints;
using System.Net.Mail;

namespace LibrarySystem.Models
{
	public class User
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; } = null!;
		public string Email { get; private set; } = null!;
		public DateTime RegisteredDate { get; private set; }

		private readonly List<Loan> _loans = new List<Loan>();
		public IReadOnlyCollection<Loan> Loans => _loans;

		private User() { }

		public User(string name, string email, DateTime registeredDate)
		{
			// Validate everything first
			if (string.IsNullOrWhiteSpace(name))
				throw new ModelValidationException("Name is required");
			name = name.Trim();

			if (name.Length > UserConstraints.NameMaxLength)
				throw new ModelValidationException($"Name can be at most {UserConstraints.NameMaxLength} characters");

			if (string.IsNullOrWhiteSpace(email))
				throw new ModelValidationException("Email is required");
			email = email.Trim().ToLower();
			if (email.Length > UserConstraints.EmailMaxLength)
				throw new ModelValidationException($"Email can be at most {UserConstraints.EmailMaxLength} characters");
			if (!MailAddress.TryCreate(email, out var parsed) || parsed.Address != email)
				throw new ModelValidationException("Email is not valid");

			if (registeredDate == default(DateTime))
				throw new ModelValidationException("Registered date is required");

			// Assign only if all checks passed
			Id = Guid.NewGuid();
			Name = name;
			Email = email;
			RegisteredDate = registeredDate;
		}
	}
}
