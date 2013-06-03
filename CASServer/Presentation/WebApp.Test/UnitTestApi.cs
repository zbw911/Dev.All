using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApp.Test
{
    [TestClass]
    public class UnitTestApi
    {
        [TestMethod]
        public void TestMethod1GetUserInfoList()
        {
            var url = "http://localhost:34382/api/User/GetUserInfoList?uids=10011838&uids=10011839";

            var result = Dev.Comm.Net.Http.GetUrl(url);

            Console.WriteLine(result);
        }


        [TestMethod]
        public void TestMethod1()
        {
            var url = "http://localhost:34382/api/User/GetUserInfo?uid=10011838";

            var result = Dev.Comm.Net.Http.GetUrl(url);

            Console.WriteLine(result);
        }


        [TestMethod]
        public void TestMethod12()
        {
            var url = "http://localhost:34382/api/User/GetUserInfoByNickname?nickname=张保维";

            var result = Dev.Comm.Net.Http.GetUrl(url);

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestMethodGetUserProfileByNickNames()
        {
            var url = "http://localhost:34382/api/User/GetUserInfoListByNickNames?nicknames=张保维&nicknames=zbw911";

            var result = Dev.Comm.Net.Http.GetUrl(url);

            Console.WriteLine(result);
        }
        [TestMethod]
        public void TestMethodCheckByNickNames()
        {
            var url = "http://localhost:34382/api/User/CheckNick?nickname=张保维";

            var result = Dev.Comm.Net.Http.GetUrl(url);

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestMethodCheckByNickNames2()
        {
            var url = "http://localhost:34382/api/User/CheckNick?nickname=张保维1";

            var result = Dev.Comm.Net.Http.GetUrl(url);

            Console.WriteLine(result);
        }
        [TestMethod]
        public void TestMethodGetUserInfoByUserName()
        {
            var url = "http://localhost:34382/api/User/GetUserInfoByUserName?username=zbw911@qq.com";

            var result = Dev.Comm.Net.Http.GetUrl(url);

            Console.WriteLine(result);
        }


        [TestMethod]
        public void MyTestMethod_GetReg()
        {
            var url = "http://localhost:34382/api/User/GetRegDateTime?uid=111111111";

            var result = Dev.Comm.Net.Http.GetUrl(url);

            Console.WriteLine(result);

            var time = Dev.Comm.JsonConvert.ToJsonObject<DateTime?>(result);

            Console.WriteLine(time ?? System.DateTime.MinValue);
        }

    }
}
