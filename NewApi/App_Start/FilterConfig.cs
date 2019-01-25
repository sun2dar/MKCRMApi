using NewAPi.Filters;
using System.Web;
using System.Web.Mvc;

namespace NewApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TrackExecutingTime());
        }
    }
}
