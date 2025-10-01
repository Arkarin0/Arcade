using NUnit.Framework;
using ClassLib1;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLib1.Tests
{
    [TestFixture]
    public class Class1Tests
    {
        [SetUp()]
        public void SetUp()
        {

        }

        [Test()]
        public void FooTest()
        {
            Assert.Fail("This test needs an implementation");
        }

        [Test()]
        public void FooTestPass()
        {
            Assert.Pass("This test allwys passes.");
        }
    }
}
