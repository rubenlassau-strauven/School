using System.Web;
using System.Web.Mvc;

namespace Wild_and_Wacky_Vegetables
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
