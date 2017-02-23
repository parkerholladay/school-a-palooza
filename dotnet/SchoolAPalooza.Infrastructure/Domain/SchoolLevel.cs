using System.Collections.Generic;
using System.Linq;

namespace SchoolAPalooza.Infrastructure.Domain
{
    public sealed class SchoolLevel
    {
        public static readonly SchoolLevel High = new SchoolLevel("high", 1, "High");
        public static readonly SchoolLevel JuniorHigh = new SchoolLevel("junior-high", 2, "Junior High");
        public static readonly SchoolLevel Middle = new SchoolLevel("middle", 3, "Middle");
        public static readonly SchoolLevel Elementary = new SchoolLevel("elementary", 4, "Elementary");

        public static readonly List<SchoolLevel> AllLevels = new List<SchoolLevel> { High, JuniorHigh, Middle, Elementary };

        SchoolLevel(string key, int sort, string description)
        {
            Key = key;
            SortOrder = sort;
            Description = description;
        }

        public string Key { get; }
        public int SortOrder { get; }
        public string Description { get; }

        public static implicit operator SchoolLevel(string key) => AllLevels.SingleOrDefault(l => l.Key == key) ?? Elementary;
    }
}
