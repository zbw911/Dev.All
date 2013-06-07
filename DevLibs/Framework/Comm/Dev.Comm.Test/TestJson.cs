using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Comm.Test
{
    [TestClass]
    public class TestJson
    {
        string str =
                  @"{""About"":"""",""ActionNum"":4,""BbsNum"":2,""Contact"":""啊"",""CreateDate"":""2013-04-25T14:45:17.653"",""GameId"":""896"",""GameName"":""梦幻魔兽"",""GroupIco"":""http://192.168.0.247:8001/2013/04/26/31/5/e/315e1ff61a101e44fd8df573278ed72a.jpg"",""GroupId"":10517,""GroupName"":""郑莉杰测试第一团产品团"",""GroupState"":1,""GroupTab"":"",联运平台,网络游戏团,移动游戏团,掌机游戏团,单机游戏团,"",""GroupTypeId"":1,""GroupTypeName"":""网络游戏"",""IdCard"":""阿斯蒂芬"",""IdentIco"":"""",""IsReview"":false,""LikeNum"":0,""MemberNum"":2,""Notice"":""ww"",""ProGroupId"":"""",""ProGroupName"":"""",""UserId"":null,""UserName"":""地方法"",""IdentState"":""""}";

        [TestMethod]
        public void TestMethod1()
        {

            var obj = Dev.Comm.JsonConvert.ToJsonObject(str);

            var t = obj.CreateDate;


        }

        [TestMethod]
        public void MyTestMethod()
        {
            var obj = Dev.Comm.JsonConvert.ToJsonObject<ViewModel>(str);

            Console.WriteLine(obj.CreateDate);
        }


        [TestMethod]
        public void MyTestMethod2()
        {
            var jsonString = @"""CreateDate"":""2013-04-25T14:45:17.653"",";
            string p = @"\d{4}-\d{2}-\d{2}[T\s]\d{1,2}:\d{1,2}:\d{1,2}[\.]\d{0,3}";

            var result = JsonString(jsonString, p);

            Console.WriteLine(result);
            p = @"\d{4}-\d{2}-\d{2}[T\s]\d{1,2}:\d{1,2}:\d{1,2}";
            result = JsonString(jsonString, p);

            Console.WriteLine(result);

        }

        [TestMethod]
        public void MyTestMethod3()
        {
            var jsonString = @"""CreateDate"":""2013-04-25 14:45:17"",";
            string p = @"\d{4}-\d{2}-\d{2}[T\s]\d{1,2}:\d{1,2}:\d{1,2}[\.]?\d{0,3}";

            var result = JsonString(jsonString, p);

            Console.WriteLine(result);
            //p = @"\d{4}-\d{2}-\d{2}[T\s]\d{1,2}:\d{1,2}:\d{1,2}";
            //result = JsonString(jsonString, p);

            //Console.WriteLine(result);

        }

        private static string JsonString(string jsonString, string p)
        {

            MatchEvaluator matchEvaluator = ConvertDateStringToJsonDate;
            var reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }

        /// <summary> 
        /// 将时间字符串转为Json时间 
        /// </summary>  
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        public class ViewModel
        {
            #region Instance Properties

            public string About { get; set; }
            public Nullable<int> ActionNum { get; set; }
            public Nullable<int> BbsNum { get; set; }
            public string Contact { get; set; }
            public Nullable<System.DateTime> CreateDate { get; set; }
            public string GameId { get; set; }
            public string GameName { get; set; }
            public string GroupIco { get; set; }
            public int GroupId { get; set; }
            public string GroupName { get; set; }
            public Nullable<int> GroupState { get; set; }
            public string GroupTab { get; set; }
            public Nullable<int> GroupTypeId { get; set; }
            public string GroupTypeName { get; set; }
            public string IdCard { get; set; }
            public string IdentIco { get; set; }
            public Nullable<bool> IsReview { get; set; }
            public Nullable<int> LikeNum { get; set; }
            public Nullable<int> MemberNum { get; set; }
            public string Notice { get; set; }
            public string ProGroupId { get; set; }
            public string ProGroupName { get; set; }
            public Nullable<decimal> UserId { get; set; }
            public string UserName { get; set; }
            public string IdentState { get; set; }
            #endregion

            //public Nullable<decimal> UserId { get; set; }
        }

    }
}
