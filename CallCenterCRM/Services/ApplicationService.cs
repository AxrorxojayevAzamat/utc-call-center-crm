

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
                    return new StatusParam() { color = "info", icon = "check" };
                    break;
                case ApplicationStatus.RejectMod:
                    return new StatusParam() { color = "danger", icon = "close" };
                    break;
                case ApplicationStatus.SendOrg:
                    return new StatusParam() { color = "success", icon = "send" };
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
                    return new StatusParam() { color = "info", icon = "check-all" };
                default:
                    return new StatusParam() { color = "", icon = "" };
                    break;
            }
        }

        public StatusParam GetAnswerStatusParams(AnswerStatus status)
        {
            switch (status)
            {
                case AnswerStatus.Send:
                    return new StatusParam() { color = "info", icon = "check" };
                    break;
                case AnswerStatus.Reject:
                    return new StatusParam() { color = "danger", icon = "close" };
                    break;
                case AnswerStatus.Edit:
                    return new StatusParam() { color = "primary", icon = "pencil" };
                    break;
                case AnswerStatus.Confirm:
                    return new StatusParam() { color = "success", icon = "checkbox-marked-circle-outline" };
                    break;
                case AnswerStatus.GotMod:
                    return new StatusParam() { color = "info", icon = "check-all" };
                    break;
                default:
                    return new StatusParam() { color = "", icon = "" };
                    break;
            }
        }

        public StatusParam GetStatusForOperator(Application application)
        {
            if (application.Answer != null && application.Answer.Status == AnswerStatus.Confirm)
            {
                return new StatusParam()
                {
                    color = "success",
                    icon = "checkbox-marked-circle-outline",
                    text = "обработан"
                };
            }

            if (application.Status == ApplicationStatus.RejectMod)
            {
                return new StatusParam()
                {
                    color = "danger",
                    icon = "close",
                    text = "отклонен"
                };
            }
            else if (application.Status == ApplicationStatus.SendMod)
            {
                return new StatusParam()
                {
                    color = "warning",
                    icon = "send",
                    text = "отправлено"
                };
            }

            return new StatusParam()
            {
                color = "info",
                icon = "timetable",
                text = "в процессе"
            };
        }

        public Boolean ShowReason(Application app, string role)
        {
            return (app.Reason != null || app.Reason != "")
            && (app.Status == ApplicationStatus.RejectOrg && role != Roles.CrmOperator.GetDisplayName()
            || app.Status == ApplicationStatus.RejectMod && role == Roles.CrmOperator.GetDisplayName());
        }
    }
}

public class StatusParam
{
    public string color = "";
    public string icon = "";
    public string text = "";
}