using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
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
    }
}
