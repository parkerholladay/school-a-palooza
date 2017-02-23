using System;
using SchoolAPalooza.Application.ViewModels;
using SchoolAPalooza.Infrastructure.Repositories;

namespace SchoolAPalooza.Application
{
    public class GetDistrictCommand
    {
        readonly IDistrictsRepository districtsRepository;
        readonly ISchoolsRepository schoolsRepository;

        public GetDistrictCommand(IDistrictsRepository districtsRepository, ISchoolsRepository schoolsRepository)
        {
            this.districtsRepository = districtsRepository;
            this.schoolsRepository = schoolsRepository;
        }

        public DistrictVm Execute(Guid id)
        {
            var maybeDistrict = districtsRepository.Find(id);
            if (!maybeDistrict.HasValue)
                return DistrictVm.None();

            var district = maybeDistrict.Value();
            var schools = schoolsRepository.FindByDistrict(district.Id);

            return DistrictVm.Build(district, schools);
        }
    }
}
