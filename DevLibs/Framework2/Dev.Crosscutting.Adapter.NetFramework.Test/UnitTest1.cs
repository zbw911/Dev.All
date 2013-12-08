using System;
using Dev.Crosscutting.Adapter.Adapter;
using Dev.Crosscutting.Adapter.NetFramework.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Crosscutting.Adapter.NetFramework.Test
{
    class MyObj
    {
        public string a { get; set; }
        public int b { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {



        [TestMethod]
        public void TestMethod1()
        {
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());
            var t = new { a = "aaa", b = 1 };

            var my = t.DynProjectedAs<MyObj>();

            Assert.AreEqual(my.b, 1);
        }
    }
}
