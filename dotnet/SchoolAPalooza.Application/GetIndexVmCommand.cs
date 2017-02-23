using SchoolAPalooza.Application.ViewModels;
using SchoolAPalooza.Infrastructure.Repositories;

namespace SchoolAPalooza.Application
{
    public class GetIndexVmCommand
    {
        readonly IApplicationStateRepository appStateRepository;

        public GetIndexVmCommand(IApplicationStateRepository appStateRepository)
        {
            this.appStateRepository = appStateRepository;
        }

        public IndexVm Execute()
        {
            return IndexVm.Build(appStateRepository.GetRequiredSetting("hosting"));
        }
    }
}
