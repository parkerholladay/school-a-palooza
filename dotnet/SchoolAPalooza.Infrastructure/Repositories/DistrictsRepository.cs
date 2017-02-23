using System;
using System.Collections.Generic;
using System.Linq;
using SchoolAPalooza.Infrastructure.Domain;
using SchoolAPalooza.Infrastructure.External;

namespace SchoolAPalooza.Infrastructure.Repositories
{
    public interface IDistrictsRepository
    {
        void Save(Guid id, string Name);
        List<District> FindAll();
        Maybe<District> Find(Guid id);
    }

    public class DistrictsRepository : IDistrictsRepository
    {
        const string Upsert = @"insert into districts (id, name) values (@id, @name)
            on conflict (id) do update set name = @name;";
        const string SelectAll = "select id, name from districts;";
        const string Select = "select id, name from districts where id = @id;";
        const string Delete = "delete from districts where id = @id;";

        readonly IPostgresConnection connection;

        public DistrictsRepository(IPostgresConnection connection)
        {
            this.connection = connection;
        }

        public void Save(Guid id, string name)
        {
            connection.WriteData(Upsert, new { id, name });
        }

        public List<District> FindAll()
        {
            return connection.ReadData<District>(SelectAll).ToList();
        }

        public Maybe<District> Find(Guid id)
        {
            return connection.ReadData<District>(Select, new { id }).SingleOrDefault().ToMaybe();
        }

        public void Remove(Guid id)
        {
            connection.WriteData(Delete, new { id });
        }
    }
}
