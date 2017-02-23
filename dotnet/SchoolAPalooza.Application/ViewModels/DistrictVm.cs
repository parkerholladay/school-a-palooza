using System;
using System.Collections.Generic;
using System.Linq;
using SchoolAPalooza.Infrastructure.Domain;

namespace SchoolAPalooza.Application.ViewModels
{
    public class DistrictVm
    {
        public string DistrictId { get; private set; }
        public string Name { get; private set; }
        public List<SchoolVm> Schools { get; private set; }
        public string FullName => $"{Name} School District";

        public static DistrictVm Build(District district, List<School> schools)
        {
            return new DistrictVm
            {
                DistrictId = district.Id.ToString(),
                Name = district.Name,
                Schools = schools.OrderBy(s => s.Level.SortOrder).ThenBy(s => s.Name).Select(SchoolVm.Build).ToList()
            };
        }

        public static DistrictVm None()
        {
            return Build(new District(Guid.Empty, ""), new List<School>());
        }
    }
}
