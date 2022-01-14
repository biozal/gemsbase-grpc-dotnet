using System;
using Gems.Core.Domain;
namespace Gems.Infrastructure
{
	public interface IRepository<TDocument>
		where TDocument : DomainBase
	{
		Task<TDocument> Create(
			TDocument document);

		Task Delete(
			string documentId);

		Task<TDocument> Get(
			string documentId);

		Task<IList<TDocument>> List(
			bool isActive);

		Task Update(
			TDocument document);
	}
}

