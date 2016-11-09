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

        }

        [Test]
        public void ShouldFail()
        {
            Assert.Fail();
        }
    }
}
