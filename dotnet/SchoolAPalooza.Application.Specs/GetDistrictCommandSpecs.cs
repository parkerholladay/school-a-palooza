using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstituteAutoMocker;
using NUnit.Specifications;
using Ploeh.AutoFixture;
using SchoolAPalooza.Application.ViewModels;
using SchoolAPalooza.Infrastructure;
using SchoolAPalooza.Infrastructure.Domain;
using SchoolAPalooza.Infrastructure.Repositories;
using Should;

namespace SchoolAPalooza.Application.Specs
{
    public class GetDistrictCommandSpecs
    {
        [Component]
        public class When_getting_a_district : ContextSpecification
        {
            Establish context = () =>
            {
                var fixture = new Fixture();
                district = fixture.Create<District>();

                var schoolBuilder = new Fixture().Build<School>().With(s => s.DistrictId, district.Id);
                school1 = schoolBuilder.Create();
                school1.Level = SchoolLevel.High;
                school2 = schoolBuilder.Create();
                school3 = schoolBuilder.Create();
                school3.Level = SchoolLevel.Middle;

                automocker.Get<IDistrictsRepository>().Find(district.Id).Returns(district.ToMaybe());
                automocker.Get<ISchoolsRepository>().FindByDistrict(district.Id).Returns(new List<School> { school2, school1, school3 });
            };

            Because of = () => districtVm = automocker.ClassUnderTest.Execute(district.Id);

            It should_get_district = () => automocker.Get<IDistrictsRepository>().Received(1).Find(district.Id);
            It should_get_schools_by_district = () => automocker.Get<ISchoolsRepository>().Received(1).FindByDistrict(district.Id);

            It should_return_vm_with_expected_district = () =>
            {
                districtVm.DistrictId.ShouldEqual(district.Id.ToString());
                districtVm.Name.ShouldEqual(district.Name);
                districtVm.FullName.ShouldEqual(district.Name + " School District");
            };

            It should_order_schools_by_level = () => districtVm.Schools.First().Level.SortOrder.ShouldBeLessThan(districtVm.Schools.Last().Level.SortOrder);
            It should_return_vm_with_expected_schools = () =>
            {
                var one = districtVm.Schools.Single(s => s.SchoolId == school1.Id.ToString());
                one.Name.ShouldEqual(school1.Name);
                one.FullName.ShouldEqual($"{school1.Name} {school1.Level.Description}");
                one.Level.Name.ShouldEqual(school1.Level.Description);

                var two = districtVm.Schools.Single(s => s.SchoolId == school2.Id.ToString());
                two.Name.ShouldEqual(school2.Name);
                two.FullName.ShouldEqual($"{school2.Name} {school2.Level.Description}");
                two.Level.Name.ShouldEqual(school2.Level.Description);

                var three = districtVm.Schools.Single(s => s.SchoolId == school3.Id.ToString());
                three.Name.ShouldEqual(school3.Name);
                three.FullName.ShouldEqual($"{school3.Name} {school3.Level.Description}");
                three.Level.Name.ShouldEqual(school3.Level.Description);
            };

            static DistrictVm districtVm;
            static District district;
            static School school1;
            static School school2;
            static School school3;

            static readonly NSubstituteAutoMocker<GetDistrictCommand> automocker = new NSubstituteAutoMocker<GetDistrictCommand>();
        }

        [Component]
        public class When_a_district_is_not_found : ContextSpecification
        {
            Establish context = () =>
            {
                automocker.Get<IDistrictsRepository>().Find(Arg.Any<Guid>()).ReturnsForAnyArgs(Maybe<District>.None());
            };

            Because of = () => district = automocker.ClassUnderTest.Execute(Guid.Empty);

            It should_get_district = () => automocker.Get<IDistrictsRepository>().Received(1).Find(Guid.Empty);
            It should_not_get_schools_by_district = () => automocker.Get<ISchoolsRepository>().Received(0).FindByDistrict(Guid.Empty);

            It should_return_empty_vm = () =>
            {
                district.DistrictId.ShouldEqual(Guid.Empty.ToString());
                district.Name.ShouldEqual("");
                district.Schools.ShouldBeEmpty();
            };

            static DistrictVm district;

            static readonly NSubstituteAutoMocker<GetDistrictCommand> automocker = new NSubstituteAutoMocker<GetDistrictCommand>();
        }

        [Component]
        public class When_a_district_has_no_schools : ContextSpecification
        {
            Establish context = () =>
            {
                var fixture = new Fixture();
                district = fixture.Create<District>();

                automocker.Get<IDistrictsRepository>().Find(district.Id).Returns(district.ToMaybe());
                automocker.Get<ISchoolsRepository>().FindByDistrict(district.Id).Returns(new List<School>());
            };

            Because of = () => districtVm = automocker.ClassUnderTest.Execute(district.Id);

            It should_return_vm_with_expected_district = () =>
            {
                districtVm.DistrictId.ShouldEqual(district.Id.ToString());
                districtVm.Name.ShouldEqual(district.Name);
            };

            It should_return_empty_list_of_schools = () => districtVm.Schools.ShouldBeEmpty();

            static DistrictVm districtVm;
            static District district;

            static readonly NSubstituteAutoMocker<GetDistrictCommand> automocker = new NSubstituteAutoMocker<GetDistrictCommand>();
        }
    }
}
