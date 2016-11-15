using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Test.Tests
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void ShouldPass()
        {
            Assert.AreEqual( 2, new Class1().Addition( 1, 1 ) );
        }

        [Test]
        public void ShouldFail()
        {
            Assert.AreEqual( 1, new Class1().Addition( 1, 1 ) );

        }
    }
}
