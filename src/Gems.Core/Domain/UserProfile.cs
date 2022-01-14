using System;

namespace Gems.Core.Domain
{
	public class UserProfile
		: DomainBase
	{
		public UserProfile() 
			: base()
        {
			GivenName = string.Empty;
			Surname = string.Empty;
			DisplayName = string.Empty;
			Email = string.Empty;
			IsActive = true;
		}

		//required fields
		public string UserId { get; set; }
		public string GivenName { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public bool IsActive { get; set; }
	
		//optional fields
		public string? Metadata { get; set; }
		public string? CreatedBy { get; set; }
		public string? ModifiedBy { get; set; }
	}
}

