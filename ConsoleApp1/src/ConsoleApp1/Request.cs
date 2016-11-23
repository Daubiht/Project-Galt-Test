using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApp1
{
    public class Request
    {
        readonly string _connectionString;
        readonly string _usersTableName;

        CloudTable _usersTable;
        CloudStorageAccount _cloudStorageAccount;
        CloudTableClient _cloudTableClient;

        public Request(string connectionString, string usersTableName = "Users")
        {
            _connectionString = connectionString;
            _usersTableName = usersTableName;
        }

        CloudStorageAccount CloudStorageAccount
        {
            get { return _cloudStorageAccount ?? (_cloudStorageAccount = CloudStorageAccount.Parse( _connectionString )); }
        }

        CloudTable UsersTable
        {
            get { return _usersTable ?? (_usersTable = CloudTableClient.GetTableReference(_usersTableName)); }
        }

        CloudTableClient CloudTableClient
        {
            get { return _cloudTableClient ?? (_cloudTableClient = CloudStorageAccount.CreateCloudTableClient()); }
        }

        //
        //Requests
        //

        public async Task<bool> AddIfNotExists( string email, string pseudo )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve( email, pseudo );
            TableResult retrieved = await UsersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result != null ) return false;

            UserEntity u = new UserEntity( email, pseudo );
            u.GitHubToken = "blblbl";
            TableOperation insertOperation = TableOperation.Insert( u );
            await UsersTable.ExecuteAsync( insertOperation );
            return true;
        }

        public async Task<bool> DeleteIfExists( string email, string pseudo )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(email, pseudo);
            TableResult retrieved = await UsersTable.ExecuteAsync( retrieveOperation );
            if( retrieved.Result == null ) return false;

            UserEntity u = (UserEntity)retrieved.Result;
            TableOperation removeOperation = TableOperation.Delete(u);
            await UsersTable.ExecuteAsync( removeOperation );
            return true;
        }

        //
        //Entities
        //

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
