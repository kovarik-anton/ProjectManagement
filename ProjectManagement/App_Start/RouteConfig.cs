using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectManagement
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Projects", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Solutions",
                url: "Projects/{projectId}/{controller}/{action}/{solutionId}",
                defaults: new { controller = "Projects", action = "Index", solutionId = UrlParameter.Optional }
            );
        }
    }
}
