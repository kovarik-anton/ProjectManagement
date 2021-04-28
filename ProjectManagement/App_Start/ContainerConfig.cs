using Autofac;
using Autofac.Integration.Mvc;
using ProjectManagement.Data.Services;
using System.Web.Mvc;

namespace ProjectManagement
{
    public class ContainerConfig
    {
        internal static void RegisterContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<ProjectRepository>()
                .As<IProjectRepository>()
                .InstancePerRequest();
            builder.RegisterType<ProjectDbContext>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}