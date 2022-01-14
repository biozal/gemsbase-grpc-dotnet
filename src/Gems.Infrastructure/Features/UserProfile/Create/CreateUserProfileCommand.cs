using System;
using MediatR;

namespace Gems.Infrastructure.Features.UserProfile.Create
{
	public class CreateUserProfileCommand
		: IRequest<Core.Domain.UserProfile>
	{
		public string GivenName { get; set; } = "";
		public string Surname { get; set; } = "";
		public string Email { get; set; } = "";
		public string DisplayName { get; set; } = "";
		public string Metadata { get; set; } = "";
		public string UserId { get; set; } = "";


		public Core.Domain.UserProfile ConverToUserProfile()
		{
			return new Core.Domain.UserProfile()
			{
				GivenName = this.GivenName,
				Surname = this.Surname,
				Email = this.Email,
				DisplayName = this.DisplayName,
				Metadata = this.Metadata,
				UserId = this.UserId,
			};
		}
	}
}

