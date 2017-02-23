using System.Collections.Generic;
using System.Linq;
using SchoolAPalooza.Infrastructure.Domain;

namespace SchoolAPalooza.Application.ViewModels
{
    public class SchoolVm
    {
        public string SchoolId { get; private set; }
        public string Name { get; private set; }
        public SchoolLevelVm Level { get; private set; }
        public string FullName => $"{Name} {Level.Name}";
        public SchoolLevelVm[] AvailableLevels => SchoolLevel.AllLevels.Select(SchoolLevelVm.Build).ToArray();

        public static SchoolVm Build(School school)
        {
            return new SchoolVm
            {
                SchoolId = school.Id.ToString(),
                Name = school.Name,
                Level = SchoolLevelVm.Build(school.Level)
            };
        }
    }
}
