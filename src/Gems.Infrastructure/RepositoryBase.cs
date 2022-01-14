using System;
using Couchbase;
using Couchbase.KeyValue;
using Gems.Core.Domain;
using Gems.Infrastructure.Services;

namespace Gems.Infrastructure
{
    public abstract class RepositoryBase<TDocument>
        : IRepository<TDocument>
        where TDocument : DomainBase
    {
        protected readonly CouchbaseService _couchbaseService;

        protected const PersistTo persistTo = PersistTo.Two;
        protected const ReplicateTo replicateTo = ReplicateTo.Two;
        protected const int timeOut = 3;
        protected string defaultCollection = "default";
        protected string defaultScope = "_default";

        public RepositoryBase(
            CouchbaseService couchbaseService)
        {
            _couchbaseService = couchbaseService;
        }

        public async Task<TDocument> Create(
            TDocument document)
        {
            var bucket = await _couchbaseService.GetBucket();
            var scope = await bucket.ScopeAsync(defaultScope);
            var collection = scope.Collection(this.defaultCollection);

            document.Created = DateTimeOffset.Now;
            document.Modified = DateTimeOffset.Now;

            var insertDoc = await collection.InsertAsync<TDocument>(
                    document.DocumentId,
                    document,
                    options =>
                    {
                        options.Durability(persistTo, replicateTo);
                    }).ConfigureAwait(false);

            return document;
        }

        public async Task Delete(
            string documentId)
        {
            var bucket = await _couchbaseService.GetBucket();
            var scope = await bucket.ScopeAsync(defaultScope);
            var collection = scope.Collection(this.defaultCollection);
            var document = await collection.GetAsync(documentId);

            await collection.RemoveAsync(
                documentId,
                options =>
                {
                    options.Cas(document.Cas);
                    options.Timeout(TimeSpan.FromSeconds(timeOut));
                });
        }

        public async Task<TDocument?> Get(
            string documentId)
        {
            var bucket = await _couchbaseService.GetBucket();
            var collection = bucket.Collection(this.defaultCollection);

            var documentResult = await collection.GetAsync(documentId);
            var document = documentResult.ContentAs<TDocument>();
            return document;

        }

        public async Task<IList<TDocument?>> List(
            bool isActive)
        {
            var bucket = await _couchbaseService.GetBucket();
            var scope = await bucket.ScopeAsync(defaultScope);
            var collection = scope.Collection(this.defaultCollection);
            throw new NotImplementedException();
        }

        public async Task Update(
            TDocument document)
        {
            var bucket = await _couchbaseService.GetBucket();
            var scope = await bucket.ScopeAsync(defaultScope);
            var collection = scope.Collection(this.defaultCollection);

            var previousResult = await collection.GetAsync(document.DocumentId);
            var result = await collection.ReplaceAsync(document.DocumentId, document,
                options =>
                {
                    options.Cas(previousResult.Cas);
                    options.Timeout(TimeSpan.FromSeconds(timeOut));
                }
            );
        }
    }
}

