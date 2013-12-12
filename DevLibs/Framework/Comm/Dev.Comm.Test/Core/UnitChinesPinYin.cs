using System;
using System.Collections;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Comm.Test.Core
{
    [TestClass]
    public class UnitChinesPinYin
    {
        private string yx = "游戏";
        private string cs = "传说";

        [TestMethod]
        public void TestMethod1()
        {
            //var a = Dev.Comm.ChineseCode.GetGbkX(yx);
            var a = Dev.Comm.StringUtil.GetChineseSpell(yx);
            Assert.AreEqual(a, "YX");
        }

        [TestMethod]
        public void GetAllPinyin()
        {
            var pn = PinyinHelper.GetPinyin(yx);

            Assert.AreEqual("youxi", pn);
        }

        [TestMethod]
        public void TestChuanshuo()
        {
            var pn = PinyinHelper.GetPinyin(cs);

            Assert.AreEqual("chuanshuo", pn);
        }


    }


}
