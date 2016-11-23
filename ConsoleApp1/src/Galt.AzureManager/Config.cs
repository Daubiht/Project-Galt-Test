using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Galt.AzureManager
{
    public class Config
    {
        public IConfiguration ConfigurationAzure()
        {
           IConfiguration conf = new ConfigurationBuilder()
                .SetBasePath( Directory.GetCurrentDirectory() )
                .AddJsonFile( "appsettings.json", optional: true )
                .AddEnvironmentVariables()
                .Build();

            return conf;
        }
    }
}
