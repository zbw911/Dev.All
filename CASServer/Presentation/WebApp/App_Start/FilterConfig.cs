using System.Web;
using System.Web.Mvc;
using Dev.Comm.Web.Mvc.Filter;

namespace CASServer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
           
            filters.Add(new ErrorFilter());
        }
    }
}