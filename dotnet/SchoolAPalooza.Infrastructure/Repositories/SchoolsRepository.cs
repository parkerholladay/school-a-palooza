using System;
using System.Collections.Generic;
using System.Linq;
using SchoolAPalooza.Infrastructure.Domain;
using SchoolAPalooza.Infrastructure.External;

namespace SchoolAPalooza.Infrastructure.Repositories
{
    public interface ISchoolsRepository
    {
        void Save(Guid districtId, Guid id, string name, string level);
        Maybe<School> Find(Guid id);
        List<School> FindByDistrict(Guid districtId);
    }

    public class SchoolsRepository : ISchoolsRepository
    {
        const string Upsert = @"insert into schools (id, district_id, name, level) values (@id, @districtId, @name, @level)
            on conflict (id) do update set (district_id, name, level) = (@districtId, @name, @level);";
        const string Select = "select id, district_id as districtId, name, level from schools where id = @id;";
        const string SelectByDistrict = "select id, district_id as districtId, name, level from schools where district_id = @districtId group by level, id order by name;";
        const string Delete = "delete from schools where id = @id;";

        readonly IPostgresConnection connection;

        public SchoolsRepository(IPostgresConnection connection)
        {
            this.connection = connection;
        }

        public void Save(Guid districtId, Guid id, string name, string level)
        {
            connection.WriteData(Upsert, new { id, districtId, name, level });
        }

        public Maybe<School> Find(Guid id)
        {
            var dtos = connection.ReadData<SchoolDto>(Select, new { id });
            return MapSchool(dtos).SingleOrDefault().ToMaybe();
        }

        IEnumerable<School> MapSchool(IEnumerable<SchoolDto> dtos)
        {
            return dtos.Select(d => new School(d.DistrictId, d.Id, d.Name, d.Level));
        }

        public List<School> FindByDistrict(Guid districtId)
        {
            var dtos = connection.ReadData<SchoolDto>(SelectByDistrict, new { districtId });
            return MapSchool(dtos).ToList();
        }

        public void Remove(Guid id)
        {
            connection.WriteData(Delete, new { id });
        }
    }

    public class SchoolDto
    {
        public Guid Id { get; set; }
        public Guid DistrictId { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
    }
}
