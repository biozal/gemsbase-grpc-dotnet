using Couchbase;
using Couchbase.Core.Exceptions;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.KeyValue;
using Couchbase.Management.Buckets;
using Couchbase.Management.Collections;
using Couchbase.Query;
using Gems.Infrastructure.Providers;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gems.Infrastructure.Services
{
	public class CouchbaseService
	{
		private readonly ILogger<CouchbaseService> _logger;
		private readonly IGemsBucketProvider _gemsBucketProvider;
		private readonly IClusterProvider _clusterProvider;
		private readonly Gems.Core.Models.CouchbaseConfig _couchbaseConfig;

		public CouchbaseService(
			ILogger<CouchbaseService> logger,
			CouchbaseConfigService configService,
			IClusterProvider clusterProvider,
			IGemsBucketProvider gemsBucketProvider
		)
		{
			_logger = logger;
			_couchbaseConfig = configService.Config;
			_clusterProvider = clusterProvider;
			_gemsBucketProvider = gemsBucketProvider;
		}

		public async Task<IBucket> GetBucket()
		{
			return await _gemsBucketProvider
					.GetBucketAsync()
					.ConfigureAwait(false);	
		}

		public async Task SetupDatabase()
		{
            try
            {
                var cluster = await CreateBucket();
                if (cluster != null)
                {
                    var bucket = await _gemsBucketProvider.GetBucketAsync();
                    if (bucket != null)
                    {

                    /** Don't create default scope it comes for free with       bucket creation. Default scope required if you are using mobile/sync gateway as of Couchbase Lite 3.0 - LABEAUA 10/01/2021 */

                        await CreateScope(bucket);
                        await CreateCollection(bucket);

                    /** Try to create index, but need to add delay because if cluster takes time to create the bucket, so this might fail if the bucket hasn't already been created.  Known limitation in SDK and until resolved this delay needs to be added when trying to dynamically create indexes
                    */
                        await Task.Delay(5000);
                        await CreateIndexes(cluster);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(message: "Error: {Message} Stack Trace: {StackTrace}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private async Task<ICluster?> CreateBucket()
        {
            ICluster? cluster = null;
            //try to create bucket, if exists will just fail which is fine
            //designed for helping developers get up and running quickly
            //for production, bucket should be setup on the cluster in a way
            //that works for the estimated load
            try
            {
                cluster = await _clusterProvider.GetClusterAsync();
                if (cluster != null)
                {
                    var bucketSettings = new BucketSettings
                    {
                        Name = _couchbaseConfig.BucketName,
                        BucketType = BucketType.Couchbase,
                        RamQuotaMB = _couchbaseConfig.RamQuotaSize
                    };

                    await cluster
			                .Buckets
			                .CreateBucketAsync(bucketSettings)
			                .ConfigureAwait(false);
                }
                else
                    throw new Exception("Can't create bucket - cluster is null, please check database configuration.");
            }
            catch (BucketExistsException)
            {
                _logger.LogWarning("Bucket {BucketName} already exists", _couchbaseConfig.BucketName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error {Message} stack trace {StackTrace}", ex.Message, ex.StackTrace);
            }

            return cluster;
        }

        private async Task CreateScope(IBucket bucket)
        {
            if (_couchbaseConfig.ScopeName != null && 
		        !_couchbaseConfig.ScopeName.Equals("_default") && 
		        ! _couchbaseConfig.ScopeName.Contains(","))
            {
                try
                {
                    await bucket
			            .Collections
			            .CreateScopeAsync(_couchbaseConfig.ScopeName)
			            .ConfigureAwait(false);
                }
                catch (ScopeExistsException)
                {
                    _logger.LogWarning("Scope {ScopeName} already exists", _couchbaseConfig.ScopeName);
                }
                catch (HttpRequestException)
                {
                    _logger.LogWarning(
                        "HttpRequestException when creating Scope {ScopeName}", _couchbaseConfig.ScopeName);
                }
            }
            else if (_couchbaseConfig.ScopeName != null &&          _couchbaseConfig.ScopeName.Contains(","))
            {
                try
                {
                    var scopeNames = _couchbaseConfig.ScopeName.Split(",");
                    foreach (var scopeName in scopeNames) 
		            {
                        var trimScopeName = scopeName.Trim();
                        await bucket
			                .Collections
			                .CreateScopeAsync(trimScopeName)
			                .ConfigureAwait(false);
		            }
                }
                catch (ScopeExistsException)
                {
                    _logger.LogWarning("Scope {ScopeName} already exists", _couchbaseConfig.ScopeName);
                }
                catch (HttpRequestException)
                {
                    _logger.LogWarning(
                        "HttpRequestException when creating Scope {ScopeName}", _couchbaseConfig.ScopeName);
                }
            }
        }

        private async Task CreateCollection(IBucket bucket)
        {
            if (_couchbaseConfig.CollectionName != null && !_couchbaseConfig.CollectionName.Equals("_default"))
            {
                //try to create collection - if fails it's ok the collection probably exists
                try
                {
                    await bucket.Collections
                        .CreateCollectionAsync(
                            new CollectionSpec(
                                _couchbaseConfig.ScopeName,
                                _couchbaseConfig.CollectionName))
                        .ConfigureAwait(false);
                }
                catch (CollectionExistsException)
                {
                    _logger.LogWarning(
                        "Collection {CollectionName} already exists in {BucketName}",
                      _couchbaseConfig.CollectionName,
                                _couchbaseConfig.BucketName);
                }
                catch (HttpRequestException)
                {
                    _logger.LogWarning(
                        "HttpRequestEException when creating collection  {CollectionName}", _couchbaseConfig.CollectionName);
                }
            }
            else if (_couchbaseConfig.CollectionName != null &&
            _couchbaseConfig.ScopeName.Contains(","))
            {
                try
                {
                    var scopeNames = _couchbaseConfig.ScopeName.Split(",");
                    foreach (var scopeName in scopeNames)
                    {
                        var trimScopeName = scopeName.Trim();
                        await bucket.Collections
                            .CreateCollectionAsync(
                                new CollectionSpec(
                                trimScopeName,
                                _couchbaseConfig.CollectionName))
                            .ConfigureAwait(false);
                    }
                }
                catch (CollectionExistsException)
                {
                    _logger.LogWarning(
                        "Collection {CollectionName} already exists in {BucketName}",
                      _couchbaseConfig.CollectionName,
                                _couchbaseConfig.BucketName);
                }
                catch (HttpRequestException)
                {
                    _logger.LogWarning(
                        "HttpRequestEException when creating collection  {CollectionName}", _couchbaseConfig.CollectionName);
                }
            }

        }

        private async Task CreateIndexes(ICluster cluster)
        {
            try
            {
                //**
                //todo - create better indexes - these are a really bad hack and should never go into production like this
                //**
                var queries = new List<string>
            {
                $"CREATE PRIMARY INDEX delete_this_index_{_couchbaseConfig.BucketName} ON {_couchbaseConfig.BucketName}.{_couchbaseConfig.ScopeName}.{_couchbaseConfig.CollectionName}",
                $"CREATE Primary INDEX on {_couchbaseConfig.BucketName}"
            };
                foreach (var query in queries)
                {
                    var result = await cluster.QueryAsync<dynamic>(query);
                    if (result.MetaData?.Status != QueryStatus.Success)
                    {
                        throw new Exception($"Error create index didn't return proper results for index {query}");
                    }
                }
            }
            catch (IndexExistsException)
            {
                _logger.LogWarning("tried to create indexes, but they already exist");
            }
        }
    }
}

