using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Galt.AzureManager
{
    public class AzureManager
    {
        readonly string _connectionString;
        readonly string _usersTableName;

        CloudTable _usersTable;
        CloudStorageAccount _cloudStorageAccount;
        CloudTableClient _cloudTableClient;

        public AzureManager( IConfiguration conf )
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(conf["Data:Azure:ConnectionString"]);
            _cloudTableClient = _cloudStorageAccount.CreateCloudTableClient();
            _usersTable = _cloudTableClient.GetTableReference( "Data:Azure:UsersTable" );
            _usersTable.CreateIfNotExistsAsync();
        }


        ////////////////////////////////////////////
        //Requests
        ////////////////////////////////////////////

        public async Task<bool> AddIfNotExists( string email, string pseudo )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, pseudo );
            TableResult retrieved = await _usersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result != null ) return false;

            UserEntity u = new UserEntity( email, pseudo );
            TableOperation insertOperation = TableOperation.Insert( u );
            await _usersTable.ExecuteAsync( insertOperation );
            return true;
        }

        public async Task<bool> AddGitHubTokenIfExists(string email, string pseudo, string token)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, pseudo );
            TableResult retrieved = await _usersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            u.GitHubToken = token;
            TableOperation modifyOperation = TableOperation.Replace(u);
            await _usersTable.ExecuteAsync( modifyOperation );
            return true;
        }

        public async Task<bool> DeleteIfExists( string email, string pseudo )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(email, pseudo);
            TableResult retrieved = await _usersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            TableOperation removeOperation = TableOperation.Delete(u);
            await _usersTable.ExecuteAsync( removeOperation );
            return true;
        }

        ////////////////////////////////////////////
        //Entities
        ////////////////////////////////////////////

        public class UserEntity : TableEntity
        {
            public UserEntity( string email, string pseudo )
            {
                PartitionKey = email;
                RowKey = pseudo;
            }

            public UserEntity() { }

            public string Favorite { get; set; }

            public string GitHubToken { get; set; }
        }
    }
}
