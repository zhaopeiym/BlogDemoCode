using System.Web;
using System.Web.Mvc;

namespace _1_Forms身份认证
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
