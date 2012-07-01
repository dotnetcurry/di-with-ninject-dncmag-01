using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FunWithSignalR;
using System.Web.WebPages;
using Ninject.Web.Common;
using Ninject;
using System.Reflection;
using Ninject.Parameters;
using System.Configuration;

namespace FunWithSignalRDI.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : NinjectHttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "BlogPost", action = "Index", id = UrlParameter.Optional }
            );
        }

        //protected void Application_Start()
        //{
        //    RegisterDependencies();
        //    AreaRegistration.RegisterAllAreas();

        //    RegisterGlobalFilters(GlobalFilters.Filters);
        //    RegisterRoutes(RouteTable.Routes);

        //    
        //    //RegisterDependencies();
        //}

        protected override IKernel CreateKernel() 
        { 
            var kernel = new StandardKernel(); 
            kernel.Load(Assembly.GetExecutingAssembly(), 
                Assembly
                    .Load("FunWithSignalR.Domain"),
                Assembly
                    .Load("FunWithSignalR.Data"),
                Assembly
                    .Load("FunWithSignalR.CompositionRoot"));
            return kernel; 
        }

        protected override void OnApplicationStarted() 
        {
            base.OnApplicationStarted(); 
            BundleTable.Bundles.RegisterTemplateBundles();
            AreaRegistration.RegisterAllAreas(); 
            RegisterGlobalFilters(GlobalFilters.Filters); 
            RegisterRoutes(RouteTable.Routes);
            RegisterDependencies();
        }

        static void RegisterDependencies()
        {
            GlobalConfiguration.Configuration
                .ServiceResolver
                .SetResolver(DependencyResolver.Current.ToServiceResolver());
            DisplayModeProvider
                .Instance.Modes.Insert(0, new DefaultDisplayMode("iphone")
            {
                ContextCondition = (context => 
                    context.GetOverriddenUserAgent().IndexOf
                    ("iPhone", StringComparison.OrdinalIgnoreCase) >= 0)
            });
        }
    }
}