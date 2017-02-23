using System;

namespace SchoolAPalooza.Infrastructure.Domain
{
    public class District
    {
        public District(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
