using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Code;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private void SetupResolveRules(ContainerBuilder builder)
        {
            builder.RegisterType<PersonRepository>().As<IPersonRepository>();
            builder.RegisterType<CMS>().As<ICMS>();
        }

        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            SetupResolveRules(builder);
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
