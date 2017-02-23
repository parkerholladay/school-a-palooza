using System.Collections.Generic;
using System.Linq;
using SchoolAPalooza.Infrastructure.Domain;

namespace SchoolAPalooza.Application.ViewModels
{
    public class AllDistrictsVm
    {
        public SimpleDistrictVm[] Districts { get; private set; }

        public static AllDistrictsVm Build(List<District> districts)
        {
            return new AllDistrictsVm
            {
                Districts = districts.Select(d => new SimpleDistrictVm
                {
                    DistrictId = d.Id.ToString(),
                    Name = d.Name
                }).ToArray()
            };
        }
    }

    public class SimpleDistrictVm
    {
        public string DistrictId { get; set; }
        public string Name { get; set; }
        public string FullName => $"{Name} School District";
    }
}
