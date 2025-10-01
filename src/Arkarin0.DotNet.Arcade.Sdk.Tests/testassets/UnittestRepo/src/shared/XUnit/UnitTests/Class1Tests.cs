using Xunit;
using ClassLib1;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLib1.Tests
{
    public class Class1Tests
    {
        [Fact()]
        public void FooTest()
        {
            Assert.Fail("This test needs an implementation");
        }

        [Fact()]
        public void FooTestPass()
        {
            Assert.True(true, "This test needs an implementation");
        }
    }
}
