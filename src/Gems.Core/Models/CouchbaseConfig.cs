using System;
namespace Gems.Core.Models
{
    public class CouchbaseConfig
    {
        //authentication information
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConnectionString { get; set; } = "";

        //bucket information
        public string BucketName { get; set; } = "";
        public string ScopeName { get; set; } = "";
        public string CollectionName { get; set; } = "";
        public int RamQuotaSize { get; set; }

        //sync gateway information
        public string SyncGatewayUri { get; set; } = "";
        public string SyncGatewayUsername { get; set; } = "";
        public string SyncGatewayPassword { get; set; } = "";
        public bool SyncGatewayUseSsl { get; set; } = false;
    }
}

