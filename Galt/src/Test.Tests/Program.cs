using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnitLite;
using NUnit.Framework;
using System.Reflection;
using NUnit.Common;

namespace Test.Tests
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return new AutoRun( typeof( Program ).GetTypeInfo().Assembly )
                .Execute( args, new ExtendedTextWrapper( Console.Out ), Console.In );
        }
    }
}
