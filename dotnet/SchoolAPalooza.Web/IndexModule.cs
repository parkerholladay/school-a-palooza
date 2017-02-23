using System;
using Nancy;
using SchoolAPalooza.Application;
using SchoolAPalooza.Infrastructure.External;
using SchoolAPalooza.Infrastructure.Repositories;

namespace SchoolAPalooza.Web
{
    public class IndexModule : NancyModule
    {
        public IndexModule(IPostgresConnection connection, IApplicationStateRepository appStateRepository)
        {
            var getIndexVmCommand = new GetIndexVmCommand(appStateRepository);
            var districtsRepository = new DistrictsRepository(connection);
            var schoolsRepository = new SchoolsRepository(connection);

            Get["/"] = _ => View["index", getIndexVmCommand.Execute()];

            Get["/districts"] = _ => GetAllDistricts(districtsRepository);

            Get["/districts/{districtId}"] = parameters => GetDistrict(districtsRepository, schoolsRepository, parameters);
        }

        dynamic GetAllDistricts(IDistrictsRepository districtsRepository)
        {
            var command = new GetAllDistrictsCommand(districtsRepository);
            var vm = command.Execute();

            return View["districts", vm];
        }

        dynamic GetDistrict(IDistrictsRepository districtsRepository, ISchoolsRepository schoolsRepository, dynamic parameters)
        {
            string idAsString = parameters.districtId;

            Guid districtId;
            if (!Guid.TryParse(idAsString, out districtId))
                return Response.AsJson(new { Error = $"Invalid district id '{idAsString}'" }, HttpStatusCode.BadRequest);

            var command = new GetDistrictCommand(districtsRepository, schoolsRepository);
            var vm = command.Execute(districtId);

            return View["schools", vm];
        }
    }
}