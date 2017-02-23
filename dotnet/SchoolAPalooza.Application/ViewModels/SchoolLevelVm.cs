using SchoolAPalooza.Infrastructure.Domain;

namespace SchoolAPalooza.Application.ViewModels
{
    public class SchoolLevelVm
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public int SortOrder{ get; private set; }

        public static SchoolLevelVm Build(SchoolLevel level)
        {
            return new SchoolLevelVm
            {
                Key = level.Key,
                Name = level.Description,
                SortOrder = level.SortOrder
            };
        }
    }
}
