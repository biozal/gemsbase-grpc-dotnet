using FluentValidation;

namespace Gems.Infrastructure.Features.UserProfile.Create
{
	public class CreateUserProfileValidator
		: AbstractValidator<CreateUserProfileCommand>
	{
        public CreateUserProfileValidator()
        {
			RuleFor(r => r.DisplayName)
				.NotEmpty()
				.MinimumLength(2)
				.MaximumLength(200);

			RuleFor(r => r.GivenName)
				.NotEmpty()
				.MinimumLength(2)
				.MaximumLength(100);

			RuleFor(r => r.Surname)
				.NotEmpty()
				.MinimumLength(2)
				.MaximumLength(100);

			RuleFor(r => r.Email)
				.NotEmpty()
				.EmailAddress();
        }
	}
}

