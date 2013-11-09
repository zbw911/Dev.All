using System;
using DS.Web.UCenter.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DS.Web.UCenter.Test
{
    [TestClass]
    public class UnitTest1
    {
        private UcClient client = new UcClient();

        //string email = "hello@hello.com";
        //string username = "hello";
        //string password = "hello";

        string email = "sdfasdfasdfffff@hello.com";
        string username = "admin1";
        string password = "admin1";

        private int questionid = 1;
        private string answer = "aaaaaa";
        [TestMethod]
        public void TestMethod1()
        {
            var r = client.UserLogin(username, password);
            Assert.IsTrue(r.Result == LoginResult.Success);

        }

        [TestMethod]
        public void testSendMessage()
        {

            var send = client.PmSend(1, 0, "aaaaaaaaa", "aaaaaaafsdf", "hello");

            Console.WriteLine(send.Result);

            Assert.IsTrue(send.Result == PmSendResult.Success);
        }



        [TestMethod]
        public void Register()
        {

            var check = client.UserCheckEmail(email);

            if (check.Result != UserCheckEmailResult.Success)
            {
                Console.WriteLine("Exist ");
                return;

            }


            var userinfo = client.UserInfo(username);
            if (userinfo.Uid > 0)
            {
                Console.WriteLine("exist username");
                return;
            }

            var result = client.UserRegister(username, password, email, questionid, answer);

            Assert.IsTrue(result.Result == RegisterResult.Success);
        }

        [TestMethod]
        public void GetPostMessage()
        {
            var user = client.UserInfo(username);
            if (user == null)
                throw new NullReferenceException();

            var news = client.PmCheckNew(user.Uid);



        }


        [TestMethod]
        public void UserNotExist()
        {
            var user = client.UserInfo("llllllllllllll");
            Assert.IsTrue(user.Uid == 0);
        }
    }
}
