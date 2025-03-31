using System.Web;
using System.Web.Mvc;

namespace ProjectPoliklinik_Servis_Final
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
