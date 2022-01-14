using System;
namespace Gems.Core.Domain
{
	public class DomainBase
	{
        public DomainBase()
        {
			DocumentId = Guid.NewGuid().ToString();
			Created = DateTimeOffset.Now;
			Modified = DateTimeOffset.Now;
	
        }

		//system managed fields
		public string DocumentId { get; set; }
		public DateTimeOffset Created { get; set; }
		public DateTimeOffset Modified { get; set; }
	}
}

