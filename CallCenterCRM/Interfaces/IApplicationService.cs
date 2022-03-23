using CallCenterCRM.Models;

namespace CallCenterCRM.Interfaces
{
    public interface IApplicationService
    {
        public string GetAppNumber(Application item);

        public StatusParam GetAppStatusParams(ApplicationStatus status);
    }
}
