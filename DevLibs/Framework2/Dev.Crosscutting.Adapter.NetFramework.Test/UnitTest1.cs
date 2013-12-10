using System;
using System.Collections.Generic;
using AutoMapper;
using Dev.Crosscutting.Adapter.Adapter;
using Dev.Crosscutting.Adapter.NetFramework.Adapter;
using Dev.Crosscutting.Adapter.NetFramework.Test.ConsoleApplication1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Crosscutting.Adapter.NetFramework.Test
{
    class MyObj
    {
        public string a { get; set; }
        public int b { get; set; }
    }


    class MyObj2
    {
        public string a { get; set; }
        public int b { get; set; }

        public string c { get; set; }
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


        [TestMethod]
        public void MyTestMethod()
        {


            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());

            AutoMapper.Mapper.CreateMap<MyObj2, MyObj>();

            var t = new MyObj2 { a = "aaa", b = 1 };

            var x = AutoMapper.Mapper.Map<MyObj>(t);
            var my = t.ProjectedAs<MyObj>();


            //AutoMapper.Mapper.map

            Assert.AreEqual(my.b, 1);
        }


        [TestMethod]
        public void test2()
        {
            Name name1 = new Name() { FirstName = "L", LastName = "jz" };
            Mapper.CreateMap<Name, NameDto>()
                .BeforeMap((name, nameDto) => Console.WriteLine("hello world before"))
                .AfterMap((name, nameDto) => Console.WriteLine("hello world after"));
            NameDto nameDto1 = Mapper.Map<NameDto>(name1);
            Console.WriteLine("1");
            Console.WriteLine(nameDto1.FirstName + nameDto1.LastName);
            Console.WriteLine();
        }


        [TestMethod]
        public void TestDic()
        {
            Mapper.CreateMap<Dictionary<string, object>, Name>();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("FirstName", "L");
            //Name name15 = Mapper.DynamicMap<Dictionary<string, object>, Name>(dict);
            Name name15 = Mapper.Map<Dictionary<string, object>, Name>(dict);
            Console.WriteLine("15");
            Console.WriteLine(name15.FirstName);
            Console.WriteLine();
          
        }

    }
}
