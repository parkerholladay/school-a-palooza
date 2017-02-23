using System.Collections.Generic;
using System.Linq;
using NUnit.Specifications;
using Ploeh.AutoFixture;
using SchoolAPalooza.Infrastructure.Domain;
using SchoolAPalooza.Infrastructure.Repositories;
using Should;

namespace SchoolAPalooza.Infrastructure.IntegrationSpecs
{
    public class DistrictRepositorySpecs
    {
        [Component]
        public class When_saving_and_loading_a_district : With_db_connection
        {
            Establish context = () =>
            {
                district = new Fixture().Create<District>();

                repository = new DistrictsRepository(dbConnection);
            };

            Because of = () =>
            {
                repository.Save(district.Id, district.Name);
                savedDistrict = repository.Find(district.Id);
            };

            It should_find_district = () => savedDistrict.HasValue.ShouldBeTrue();
            It should_save_id = () => savedDistrict.Value().Id.ShouldEqual(district.Id);
            It should_save_name = () => savedDistrict.Value().Name.ShouldEqual(district.Name);

            Cleanup after = () => repository.Remove(district.Id);

            static District district;
            static Maybe<District> savedDistrict;
            static DistrictsRepository repository;
        }

        [Component]
        public class When_loading_all_districts : With_db_connection
        {
            Establish context = () =>
            {
                var districtBuilder = new Fixture().Build<District>();
                district1 = districtBuilder.Create();
                district2 = districtBuilder.Create();
                district3 = districtBuilder.Create();

                repository = new DistrictsRepository(dbConnection);
            };

            Because of = () =>
            {
                repository.Save(district1.Id, district1.Name);
                repository.Save(district2.Id, district2.Name);
                repository.Save(district3.Id, district3.Name);
                savedDistricts = repository.FindAll();
            };

            It should_find_districts = () => savedDistricts.Count.ShouldEqual(3);
            It should_find_expected_districts = () =>
            {
                var one = savedDistricts.FirstOrDefault(d => d.Id == district1.Id);
                one.ShouldNotBeNull();
                one.Name.ShouldEqual(district1.Name);

                var two = savedDistricts.FirstOrDefault(d => d.Id == district2.Id);
                two.ShouldNotBeNull();
                two.Name.ShouldEqual(district2.Name);

                var three = savedDistricts.FirstOrDefault(d => d.Id == district3.Id);
                three.ShouldNotBeNull();
                three.Name.ShouldEqual(district3.Name);
            };

            Cleanup after = () =>
            {
                repository.Remove(district1.Id);
                repository.Remove(district2.Id);
                repository.Remove(district3.Id);
            };

            static District district1;
            static District district2;
            static District district3;
            static List<District> savedDistricts;
            static DistrictsRepository repository;
        }
    }
}
