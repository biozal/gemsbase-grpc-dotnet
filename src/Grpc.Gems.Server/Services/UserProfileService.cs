using System;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Gems.Server.Protos;

namespace Grpc.Gems.Server.Services
{
    public class UserProfileService
        : Protos.UserProfileService.UserProfileServiceBase
    {
        public UserProfileService()
        {
        }

        public override Task<UserProfile> CreateUserProfile(CreateUserProfileRequest request, ServerCallContext context)
        {
            return base.CreateUserProfile(request, context);
        }

        public override Task<Empty> DeleteUserProfile(DeleteUserProfileRequest request, ServerCallContext context)
        {
            return base.DeleteUserProfile(request, context);
        }

        public override Task<Empty> DisableUserProfile(DisableUserProfileRequest request, ServerCallContext context)
        {
            return base.DisableUserProfile(request, context);
        }

        public override Task<Empty> EnableUserProfile(EnableUserProfileRequest request, ServerCallContext context)
        {
            return base.EnableUserProfile(request, context);
        }

        public override Task<UserProfile> GetUserProfile(GetUserProfileRequest request, ServerCallContext context)
        {
            return base.GetUserProfile(request, context);
        }

        public override Task<ListUserProfilesReponse> ListUserProfiles(ListUserProfilesRequest request, ServerCallContext context)
        {
            return base.ListUserProfiles(request, context);
        }

        public override Task<UserProfile> UpdateUserProfile(UpdateUserProfileRequest request, ServerCallContext context)
        {
            return base.UpdateUserProfile(request, context);
        }
    }
}

