

using CallCenterCRM.Data;
using CallCenterCRM.Interfaces;
using CallCenterCRM.Models;
using Microsoft.EntityFrameworkCore;

namespace CallCenterCRM.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly CallcentercrmContext _context;
        public ApplicationService(CallcentercrmContext context)
        {
            _context = context;
        }
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
                case ApplicationStatus.AskDelay:
                    return new StatusParam() { color = "warning", icon = "timer" };
                    break;
                case ApplicationStatus.RejectDelay:
                    return new StatusParam() { color = "danger", icon = "timer" };
                    break;
                case ApplicationStatus.GotMod:
                    return new StatusParam() { color = "info", icon = "check-all" };
                    break;
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

        public bool ShowReason(Application app, string role)
        {
            return (app.Reason != null || app.Reason != "")
            && (app.Status == ApplicationStatus.RejectOrg && role != Roles.CrmOperator.GetDisplayName()
            || app.Status == ApplicationStatus.RejectMod && role == Roles.CrmOperator.GetDisplayName());
        }

        public int AppCount(int userId, ApplicationStatus status)
        {
            User moderator = _context.Users.Where(a => a.ModeratorId == userId).FirstOrDefault();
            var apps = _context.Applications.Include(a => a.Recipient)
                .Where(a => (moderator != null ? a.Recipient.ModeratorId == userId : a.RecipientId == userId)
                && a.Status == status && a.IsGot == false).ToList();
            int count = apps.Count;

            return count;
        }

        public int AnswerCount(int userId, AnswerStatus status)
        {
            User moderator = _context.Users.Where(a => a.Id == userId).FirstOrDefault();

            int count = _context.Answers
                .Where(a => (a.AuthorId == userId || moderator.Id == userId) && a.Status == status && a.IsGot == false)
                .ToList().Count;
            return count;
        }

        public bool IsGot(Roles role, ApplicationStatus status)
        {
            return (role == Roles.CrmOperator && status == ApplicationStatus.RejectMod)
                || (role == Roles.CrmModerator && status == ApplicationStatus.SendMod)
                || (role == Roles.CrmModerator && status == ApplicationStatus.RejectOrg)
                || (role == Roles.CrmModerator && status == ApplicationStatus.AskDelay)
                || (role == Roles.CrmOrganization && status == ApplicationStatus.SendOrg)
                || (role == Roles.CrmOrganization && status == ApplicationStatus.Delay)
                || (role == Roles.CrmOrganization && status == ApplicationStatus.RejectDelay);
        }

        public bool IsGotAnswer(Roles role, AnswerStatus status)
        {
            return (role == Roles.CrmModerator && status == AnswerStatus.Send)
                || (role == Roles.CrmModerator && status == AnswerStatus.Edit)
                || (role == Roles.CrmOrganization && status == AnswerStatus.Confirm)
                || (role == Roles.CrmOrganization && status == AnswerStatus.Reject);
        }

        public List<ModeratorStats>? GetModeratorStats(int userId)
        {
            var stats = new List<ModeratorStats>();


            foreach (Regions region in Enum.GetValues(typeof(Regions)))
            {
                var applications = _context.Applications
                    .Include(u => u.Applicant)
                    .Include(u => u.Recipient)
                        .ThenInclude(r => r.Organizations)
                    .Where(a => a.Applicant.Region == region && a.Recipient.ModeratorId == userId);

                var user = _context.Users.Where(u => u.Id == userId).Include(u => u.Organizations);


                ModeratorStats stat = new ModeratorStats()
                {
                    Region = region,
                    BranchesCount = CountBranchesApps(user, applications),
                    DoneCount = GetStatusCount(applications).DoneCount,
                    ProcessCount = GetStatusCount(applications).ProcessCount,
                    RejectedCount = GetStatusCount(applications).RejectedCount,
                };
                stats.Add(stat);
            }

            return stats;
        }

        private static List<int> CountBranchesApps(IQueryable<User> branches, IQueryable<Application> apps)
        {
            List<int> branchesCount = new List<int>();

            foreach (var branch in branches.FirstOrDefault().Organizations)
            {
                branchesCount.Add(apps.Where(a => a.RecipientId == branch.Id).ToList().Count);
            }

            return branchesCount;
        }

        public List<OrganizationStats>? GetOrganizationStats(int userId)
        {
            var stats = new List<OrganizationStats>();

            foreach (Regions region in Enum.GetValues(typeof(Regions)))
            {
                var applications = _context.Applications
                    .Include(u => u.Applicant)
                    .Include(u => u.Recipient)
                    .Where(a => a.Applicant.Region == region && a.RecipientId == userId);

                OrganizationStats stat = new OrganizationStats()
                {
                    Region = region,

                    FizikCount = GetPersonCount(applications).FizikCount,
                    YurikCount = GetPersonCount(applications).YurikCount,
                    MaleCount = GetPersonCount(applications).MaleCount,
                    FemaleCount = GetPersonCount(applications).FemaleCount,

                    DoneCount = GetStatusCount(applications).DoneCount,
                    ProcessCount = GetStatusCount(applications).ProcessCount,
                    RejectedCount = GetStatusCount(applications).RejectedCount,
                };
                stats.Add(stat);
            }

            return stats;
        }

        private static Stats GetStatusCount(IQueryable<Application> apps)
        {
            return new Stats()
            {
                DoneCount = apps.Where(a => a.Answer.Status == AnswerStatus.Confirm).ToList().Count,
                ProcessCount = apps.Where(a => !(a.Answer.Status == AnswerStatus.Confirm
                    || a.Status == ApplicationStatus.RejectMod)).ToList().Count,
                RejectedCount = apps.Where(a => a.Status == ApplicationStatus.RejectMod).ToList().Count,
            };
        }

        private static OrganizationStats GetPersonCount(IQueryable<Application> apps)
        {
            return new OrganizationStats()
            {
                FizikCount = apps.Where(a => a.Applicant.Type == Types.Individual).ToList().Count,
                YurikCount = apps.Where(a => a.Applicant.Type == Types.Business).ToList().Count,
                MaleCount = apps.Where(a => a.Applicant.Gender == Genders.Male).ToList().Count,
                FemaleCount = apps.Where(a => a.Applicant.Gender == Genders.Female).ToList().Count,
            };
        }
    }
}

public class StatusParam
{
    public string color = "";
    public string icon = "";
    public string text = "";
}