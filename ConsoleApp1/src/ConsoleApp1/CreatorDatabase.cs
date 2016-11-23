using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApp1
{
    public class CreatorDatabase
    {
        public CreatorDatabase( IConfiguration conf)
        {
            // Retrieve the storage  from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conf["Data:Azure:ConnectionString"]);

            // Create the table client.
            CloudTableClient cTableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable cTable = cTableClient.GetTableReference("Users");

            //Create the table if it doesn't exist.
            cTable.CreateIfNotExistsAsync();
        }
    }
}
