using System.Web.Mvc;

namespace hkkf.web.Areas.Service
{
    public class ServiceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Service";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Service_default",
                "Service/{controller}/{action}/{id}",
                new {controller="Home",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
