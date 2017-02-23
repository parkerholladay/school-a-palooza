using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Specifications;
using Ploeh.AutoFixture;
using SchoolAPalooza.Infrastructure.Domain;
using SchoolAPalooza.Infrastructure.Repositories;
using Should;

namespace SchoolAPalooza.Infrastructure.IntegrationSpecs
{
    public class SchoolRepositorySpecs
    {
        [Component]
        public class When_saving_and_loading_a_school : With_db_connection
        {
            Establish context = () =>
            {
                school = new Fixture().Create<School>();

                repository = new SchoolsRepository(dbConnection);
            };

            Because of = () =>
            {
                repository.Save(school.DistrictId, school.Id, school.Name, school.Level.Key);
                savedSchool = repository.Find(school.Id);
            };

            It should_find_school = () => savedSchool.HasValue.ShouldBeTrue();
            It should_save_id = () => savedSchool.Value().Id.ShouldEqual(school.Id);
            It should_save_district_id = () => savedSchool.Value().DistrictId.ShouldEqual(school.DistrictId);
            It should_save_name = () => savedSchool.Value().Name.ShouldEqual(school.Name);
            It should_save_level = () => savedSchool.Value().Level.ShouldEqual(school.Level);

            Cleanup after = () => repository.Remove(school.Id);

            static School school;
            static Maybe<School> savedSchool;
            static SchoolsRepository repository;
        }

        [Component]
        public class When_getting_schools_by_district : With_db_connection
        {
            Establish context = () =>
            {
                districtId = Guid.NewGuid();

                var schoolBuilder = new Fixture().Build<School>().With(s => s.DistrictId, districtId);
                school1 = schoolBuilder.Create();
                school2 = schoolBuilder.Create();

                repository = new SchoolsRepository(dbConnection);
            };

            Because of = () =>
            {
                repository.Save(school1.DistrictId, school1.Id, school1.Name, school1.Level.Key);
                repository.Save(school2.DistrictId, school2.Id, school2.Name, school2.Level.Key);
                savedSchools = repository.FindByDistrict(districtId);
            };

            It should_find_schools = () => savedSchools.Count.ShouldEqual(2);
            It should_find_expected_schools = () =>
            {
                var one = savedSchools.FirstOrDefault(s => s.Id == school1.Id);
                one.ShouldNotBeNull();
                one.DistrictId.ShouldEqual(districtId);
                one.Name.ShouldEqual(school1.Name);
                one.Level.ShouldEqual(school1.Level);

                var two = savedSchools.FirstOrDefault(s => s.Id == school2.Id);
                two.ShouldNotBeNull();
                two.DistrictId.ShouldEqual(districtId);
                two.Name.ShouldEqual(school2.Name);
                two.Level.ShouldEqual(school2.Level);
            };

            Cleanup after = () =>
            {
                repository.Remove(school1.Id);
                repository.Remove(school2.Id);
            };

            static Guid districtId;
            static School school1;
            static School school2;
            static List<School> savedSchools;
            static SchoolsRepository repository;
        }
    }
}
