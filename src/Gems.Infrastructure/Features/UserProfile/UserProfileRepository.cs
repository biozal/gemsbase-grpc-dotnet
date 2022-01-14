using System;
using Gems.Infrastructure.Services;

namespace Gems.Infrastructure.Features.UserProfile
{
	public class UserProfileRepository
		: RepositoryBase<Core.Domain.UserProfile>
		, IUserProfileRepository
	{
		public UserProfileRepository(
	        CouchbaseService couchbaseService)
	        :base(couchbaseService)
		{
            defaultCollection = "UserProfile";
		}

        public Task Disable(string userProfileId)
        {
            throw new NotImplementedException();
        }

        public Task Enable(string userProfileId)
        {
            throw new NotImplementedException();
        }
    }
}

