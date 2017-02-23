using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstituteAutoMocker;
using NUnit.Specifications;
using Ploeh.AutoFixture;
using SchoolAPalooza.Application.ViewModels;
using SchoolAPalooza.Infrastructure.Domain;
using SchoolAPalooza.Infrastructure.Repositories;
using Should;

namespace SchoolAPalooza.Application.Specs
{
    public class GetAllDistrictsCommandSpecs
    {
        [Component]
        public class When_getting_all_districts : ContextSpecification
        {
            Establish context = () =>
            {
                var fixture = new Fixture();
                district1 = fixture.Create<District>();
                district2 = fixture.Create<District>();
                district3 = fixture.Create<District>();

                automocker.Get<IDistrictsRepository>().FindAll().Returns(new List<District> { district1, district2, district3 });
            };

            Because of = () => vm = automocker.ClassUnderTest.Execute();

            It should_get_district = () => automocker.Get<IDistrictsRepository>().Received(1).FindAll();

            It should_return_vm_with_expected_schools = () =>
            {
                var one = vm.Districts.Single(d => d.DistrictId == district1.Id.ToString());
                one.Name.ShouldEqual(district1.Name);
                one.FullName.ShouldEqual(district1.Name + " School District");

                var two = vm.Districts.Single(d => d.DistrictId == district2.Id.ToString());
                two.Name.ShouldEqual(district2.Name);
                two.FullName.ShouldEqual(district2.Name + " School District");

                var three = vm.Districts.Single(d => d.DistrictId == district3.Id.ToString());
                three.Name.ShouldEqual(district3.Name);
                three.FullName.ShouldEqual(district3.Name + " School District");
            };

            static AllDistrictsVm vm;
            static District district1;
            static District district2;
            static District district3;

            static readonly NSubstituteAutoMocker<GetAllDistrictsCommand> automocker = new NSubstituteAutoMocker<GetAllDistrictsCommand>();
        }

        [Component]
        public class When_no_districts_are_found : ContextSpecification
        {
            Establish context = () =>
            {
                automocker.Get<IDistrictsRepository>().FindAll().ReturnsForAnyArgs(new List<District>());
            };

            Because of = () => vm = automocker.ClassUnderTest.Execute();

            It should_get_all_districts = () => automocker.Get<IDistrictsRepository>().Received(1).FindAll();
            It should_return_empty_vm = () => vm.Districts.ShouldBeEmpty();

            static AllDistrictsVm vm;

            static readonly NSubstituteAutoMocker<GetAllDistrictsCommand> automocker = new NSubstituteAutoMocker<GetAllDistrictsCommand>();
        }
    }
}
