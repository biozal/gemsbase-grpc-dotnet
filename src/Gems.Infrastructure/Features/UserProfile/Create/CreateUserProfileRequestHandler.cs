using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gems.Infrastructure.Features.UserProfile.Create
{
    public class CreateUserProfileRequestHandler
		: IRequestHandler<CreateUserProfileCommand, Core.Domain.UserProfile>	
	{
		private readonly ILogger<CreateUserProfileRequestHandler> _logger;
		private readonly IUserProfileRepository _repository;

		public CreateUserProfileRequestHandler(
			ILogger<CreateUserProfileRequestHandler> logger,
			IUserProfileRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}

        public async Task<Core.Domain.UserProfile> Handle (
			CreateUserProfileCommand request,
			CancellationToken cancellationToken)
        {
			return await _repository.Create(
					request.ConverToUserProfile());
        }
    }
}

