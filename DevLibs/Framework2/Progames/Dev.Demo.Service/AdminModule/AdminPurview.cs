namespace Dev.Demo.Application.MainBoundedContext.AdminModule
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using Dev.Demo.ViewModels;

    /// <summary>
    /// 用户权限判断的
    /// </summary>
    public class AdminPurview
    {
        /// <summary>
        /// 超级管理员的Key
        /// </summary>
        public const string AdminKey = "admin_AllowAll";

        public static void SetConfigFile(string path)
        {
            _filepath = path;
        }

        private static string _filepath;//= System.Configuration.ConfigurationManager.AppSettings[""];// HttpContext.Current.Server.MapPath("~/App_Data/grouplist.txt");

        public static List<PurviewGroup> GetPurviews()
        {


            var listpg = new List<PurviewGroup>();

            //Server.MapPath("~/App_Data/grouplist.txt")
            var fileline = File.ReadAllLines(_filepath);
            var index = 0;
            PurviewGroup pg = null;
            foreach (var iline in fileline)
            {

                var line = iline.Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (Dev.Comm.RegexHelper.Preg_match(@"^>>", line))
                {
                    pg = new PurviewGroup
                             {
                                 GroupName = line.Replace(">>", ""),
                                 PurviewKeyValues = new List<KeyValuePair<string, string>>()
                             };

                    listpg.Add(pg);
                }
                else if (Dev.Comm.RegexHelper.Preg_match(@"^>", line))
                {
                    var kvs = line.Split(new char[] { '>' });

                    Debug.Assert(pg != null, "pg != null");
                    pg.PurviewKeyValues.Add(new KeyValuePair<string, string>(kvs[1], kvs[2]));
                }

            }

            return listpg;
        }

        public static List<KeyValuePair<string, string>> AllPurviewKeyValues
        {
            get
            {
                var purviews = AdminPurview.GetPurviews();
                var purviewKeyValues = new List<KeyValuePair<string, string>>();

                foreach (var purviewGroup in purviews)
                {
                    purviewKeyValues.AddRange(purviewGroup.PurviewKeyValues);
                }

                return purviewKeyValues;
            }
        }

        public static IEnumerable<string> AllKey
        {
            get { return AllPurviewKeyValues.Select(x => x.Key); }
        }

        public static IEnumerable<string> AllValue
        {
            get { return AllPurviewKeyValues.Select(x => x.Value); }
        }

        /// <summary>
        /// 解析标识为文本
        /// </summary>
        /// <param name="purviews"></param>
        /// <returns></returns>
        public static string Parse2Text(string purviews)
        {
            return Parse2Text(purviews.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }
        /// <summary>
        /// 解析标识为文本
        /// </summary>
        /// <param name="purviews"></param>
        /// <returns></returns>
        public static string Parse2Text(string[] purviews)
        {
            return Parse2Text(purviews, false);

        }


        /// <summary>
        ///  解析标识为文本
        /// </summary>
        /// <param name="purviews"></param>
        /// <param name="withKey"></param>
        /// <param name="Format"></param>
        /// <returns></returns>
        public static string Parse2Text(string[] purviews, bool withKey, string Format = @"({0})")
        {

            var all = GetPurviews();

            if (purviews.Any(y => y == AdminKey))
            {
                return "全部管理权限";
            }

            var strall = new List<string>();

            foreach (var purview in purviews)
            {
                var purviewname = FindPurviewName(all, purview);

                if (!string.IsNullOrWhiteSpace(purviewname))
                {
                    if (withKey)
                    {
                        purviewname += string.Format(Format, purview);
                    }

                    strall.Add(purviewname);
                }
            }

            return string.Join(",", strall);
        }




        private static string FindPurviewName(IEnumerable<PurviewGroup> purviewGrouplist, string purviewValue)
        {
            foreach (var purviewGroup in purviewGrouplist.Where(purviewGroup => purviewGroup.PurviewKeyValues.Any(x => x.Key == purviewValue)))
            {
                return purviewGroup.PurviewKeyValues.First(x => x.Key == purviewValue).Value;
            }

            return "";
        }

        ///// <summary>
        ///// 检测是否拥有权限
        ///// </summary>
        ///// <param name="purview"></param>
        ///// <param name="purviewlist"></param>
        ///// <returns></returns>
        //public static bool CheckUserPurview(string purview, string[] purviewlist)
        //{
        //    if (purviewlist == null || purviewlist.Length == 0)
        //        return false;

        //    return purview.Contains(purview);
        //}
    }
}