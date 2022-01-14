using System;

namespace Gems.Infrastructure.Features.UserProfile
{
	public interface IUserProfileRepository
		: IRepository<Core.Domain.UserProfile>
	{
		Task Disable(
			string userProfileId);
		Task Enable(
			string userProfileId);
	}
}

