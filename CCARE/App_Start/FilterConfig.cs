using System.Web;
using System.Web.Mvc;
using CCARE.Filters;

namespace CCARE
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LoginFilter());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TrackExecutingTime());
        }
    }
}