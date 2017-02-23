using NUnit.Specifications;
using SchoolAPalooza.Infrastructure.External;
using SchoolAPalooza.Infrastructure.Repositories;

namespace SchoolAPalooza.Infrastructure.IntegrationSpecs
{
    public class With_db_connection : ContextSpecification
    {
        Establish context = () =>
        {
            var settings = new ApplicationStateRepository();
            dbConnection = new PostgresConnection(settings.GetPrimaryConnectionString());
        };

        internal static IPostgresConnection dbConnection;
    }
}
