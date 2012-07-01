using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using FunWithSignalR.Domain.Repository;
using FunWithSignalR.Data.Repository;
using System.Configuration;

namespace FunWithSignalR.CompositionRoot
{
    public class DependencyMapper : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBlogPostRepository>()
                .To<SqlBlogPostRepository>()
                .WithConstructorArgument("connectionName",
                    ConfigurationManager
                        .ConnectionStrings["FunWithSignalR"]
                            .ConnectionString)
                .WithConstructorArgument("schemaName",
                    ConfigurationManager
                        .AppSettings["schemaName"]);
        }
    }
}
