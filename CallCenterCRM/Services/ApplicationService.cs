

using CallCenterCRM.Data;
using CallCenterCRM.Interfaces;
using CallCenterCRM.Models;

namespace CallCenterCRM.Services
{
    public class ApplicationService : IApplicationService
    {
        public string GetAppNumber(Application item)
        {
            int Id = item.Id;
            int RefSrcCode = item.Applicant.ReferenceSource.GetHashCode();
            string Year = item.CreatedDate.GetValueOrDefault().Year.ToString().Substring(2);

            return $"{Id}-{RefSrcCode}/{Year}";
        }

        public StatusParam GetAppStatusParams(ApplicationStatus status)
        {
            switch (status)
            {
                case ApplicationStatus.SendMod:
                    return new StatusParam() { color = "info", icon = "send" };
                    break;
                case ApplicationStatus.RejectMod:
                    return new StatusParam() { color = "danger", icon = "close" };
                    break;
                case ApplicationStatus.SendOrg:
                    return new StatusParam() { color = "success", icon = "check-all" };
                    break;
                case ApplicationStatus.RejectOrg:
                    return new StatusParam() { color = "danger", icon = "close" };
                    break;
                case ApplicationStatus.Edit:
                    return new StatusParam() { color = "primary", icon = "pencil" };
                    break;
                case ApplicationStatus.Delay:
                    return new StatusParam() { color = "warning", icon = "timer" };
                    break;
                case ApplicationStatus.GotMod:
                    return new StatusParam() { color = "info", icon = "check" };
                default:
                    return new StatusParam() { color = "", icon = "" };
                    break;
            }
        }
    }
}

public class StatusParam
{
    public string color = "";
    public string icon = "";
}