using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FooApi.unittest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(true, "true");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.IsTrue(false, "false");
        }
    }
}
