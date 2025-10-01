using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLib1;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLib1.Tests
{
    [TestClass]
    public class Class1Tests
    {
        [TestMethod()]
        public void FooTest()
        {
            Assert.IsFalse(true, "This test needs an implementation");
        }

        [TestMethod()]
        public void FooTestPass()
        {
            Assert.IsTrue(true, "This test needs an implementation");
        }
    }
}
