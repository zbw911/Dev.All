using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.CasClient.Utils
{

    public class UserFace
    {
        public static string GetByUserid(int userid, int type)
        {
            if (type > 4 || type < 1)
                throw new Exception("只能在1至4之间");

            return Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl +
                   "/Avatar/AvataUrlByUserid?type=" + type + "&userid=" + userid;
        }
        public static string Get(decimal uid, int type)
        {
            if (type > 4 || type < 1)
                throw new Exception("只能在1至4之间");

            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl +
                   "/Avatar/AvataUrl?type=" + type + "&uid=" + uid;

            return (url);
        }


    }
}
