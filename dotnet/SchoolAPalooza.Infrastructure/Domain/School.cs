using System;

namespace SchoolAPalooza.Infrastructure.Domain
{
    public class School
    {
        public School(Guid districtId, Guid schoolId, string name, string level)
        {
            DistrictId = districtId;
            Id = schoolId;
            Name = name;
            Level = level;
        }

        public Guid DistrictId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SchoolLevel Level { get; set; }
    }
}
