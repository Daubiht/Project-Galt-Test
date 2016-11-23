using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main( string[] args )
        {

            IConfiguration conf = new ConfigurationBuilder()
                .SetBasePath( Directory.GetCurrentDirectory() )
                .AddJsonFile( "appsettings.json", optional: true )
                .AddEnvironmentVariables()
                .Build();

            CreatorDatabase d = new CreatorDatabase(conf);

            Request r = new Request(conf["Data:Azure:ConnectionString"]);

            Test(r);

            Console.ReadKey();
        }

        public static async void Test(Request r)
        {
            Console.Write( await r.DeleteIfExists( "plop@plop", "plop" ) );
            Console.Write( await r.AddIfNotExists( "plop@plop", "plop" ) );
        }
    }
}
