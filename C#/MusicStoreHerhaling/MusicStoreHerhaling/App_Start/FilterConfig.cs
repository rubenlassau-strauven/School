using System.Web;
using System.Web.Mvc;

namespace MusicStoreHerhaling
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
