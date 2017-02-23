using SchoolAPalooza.Application.ViewModels;
using SchoolAPalooza.Infrastructure.Repositories;

namespace SchoolAPalooza.Application
{
    public class GetAllDistrictsCommand
    {
        readonly IDistrictsRepository districtsRepository;

        public GetAllDistrictsCommand(IDistrictsRepository districtsRepository)
        {
            this.districtsRepository = districtsRepository;
        }

        public AllDistrictsVm Execute()
        {
            var districts = districtsRepository.FindAll();

            return AllDistrictsVm.Build(districts);
        }
    }
}
