using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.ViewEngines.Razor;
using SchoolAPalooza.Infrastructure;
using SchoolAPalooza.Infrastructure.External;
using SchoolAPalooza.Infrastructure.Repositories;

namespace SchoolAPalooza.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        IApplicationStateRepository appStateRepository;
        IPostgresConnection connection;

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            appStateRepository = new ApplicationStateRepository();
            container.Register(appStateRepository);

            connection = new PostgresConnection(appStateRepository.GetPrimaryConnectionString());
            container.Register(connection);

            StaticConfiguration.DisableErrorTraces = !appStateRepository.GetRequiredSetting("showErrorTraces").AsBool();
        }

        protected class RazorConfiguration : IRazorConfiguration
        {
            public IEnumerable<string> GetAssemblyNames()
            {
                return new List<string>
                {
                    "Nancy",
                    "Nancy.ViewEngines.Razor",
                    "SchoolAPalooza.Application",
                    "SchoolAPalooza.Web",
                    "System",
                    "System.Web.Mvc",
                };
            }

            public IEnumerable<string> GetDefaultNamespaces()
            {
                return new List<string>
                {
                    "Nancy",
                    "Nancy.ViewEngines.Razor",
                    "SchoolAPalooza.Application.ViewModels",
                    "System",
                    "System.Web.Mvc.Html",
                };
            }

            public bool AutoIncludeModelNamespace { get { return true; } }
        }
    }
}
