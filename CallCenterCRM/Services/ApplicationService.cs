

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
            int Id = _context.Applications.OrderBy(a => a.Id).Last().Id + 1;
            int RefSrcCode = (int)item.Applicant.ReferenceSource;
            string Year = DateTime.Now.Year.ToString().Substring(2);

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
            && ((app.Status == ApplicationStatus.RejectOrg || app.Status == ApplicationStatus.RejectDelay) && role != Roles.CrmOperator.GetDisplayName()
            || app.Status == ApplicationStatus.RejectMod && role == Roles.CrmOperator.GetDisplayName());
        }

        public int AppCount(int userId, ApplicationStatus status)
        {
            User moderator = _context.Users.Where(a => a.ModeratorId == userId).FirstOrDefault();
            User userOperator = _context.Users.Where(a => a.Id == userId && a.Role == Roles.CrmOperator).FirstOrDefault();

            var apps = _context.Applications.Include(a => a.Recipient)
                .Where(a => (userOperator != null || (moderator != null && status != ApplicationStatus.SendMod ? a.Recipient.ModeratorId == userId : a.RecipientId == userId))
                && a.Status == status && a.IsGot == false).ToList();
            int count = apps.Count;

            return count;
        }

        public int AnswerCount(int userId, AnswerStatus status)
        {
            User moderator = _context.Users.Where(a => a.ModeratorId == userId).FirstOrDefault();

            int count = _context.Answers.Include(a => a.Author)
                .Where(a => (moderator != null ? a.Author.ModeratorId == userId : a.AuthorId == userId) && a.Status == status && a.IsGot == false)
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

        public List<ModeratorStats>? GetModeratorStats(int userId, int? branchId, DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            var stats = new List<ModeratorStats>();
            if (toDate != null)
                toDate.Value.AddDays(1);

            foreach (Regions region in Enum.GetValues(typeof(Regions)))
            {
                var applications = _context.Applications
                    .Include(u => u.Applicant)
                    .Include(u => u.Recipient)
                    //.ThenInclude(r => r.Organizations)
                    .Where(a => a.Applicant.Region == region
                    && (branchId != null ? a.RecipientId == branchId : (a.Recipient.ModeratorId == userId || a.RecipientId == userId))
                    && (fromDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)fromDate) >= 0)
                    && (toDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)toDate) <= 0));

                var user = _context.Users.Where(u => u.Id == userId).Include(u => u.Organizations);

                ModeratorStats stat = new ModeratorStats()
                {
                    Region = region,
                    //BranchesCount = CountBranchesApps(user, applications),
                    DoneCount = GetStatusCount(applications).DoneCount,
                    ProcessCount = GetStatusCount(applications).ProcessCount,
                    RejectedCount = GetStatusCount(applications).RejectedCount,
                    AllCount = applications.Count(),
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
                List<Application> branchApps = apps.Where(a => a.RecipientId == branch.Id).ToList();
                branchesCount.Add(branchApps.Count);
            }

            return branchesCount;
        }

        public List<OrganizationStats>? GetOrganizationStats(int userId, DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            var stats = new List<OrganizationStats>();
            if (toDate != null)
                toDate.Value.AddDays(1);

            foreach (Regions region in Enum.GetValues(typeof(Regions)))
            {
                var applications = _context.Applications
                    .Include(u => u.Applicant)
                    .Include(u => u.Recipient)
                    .Where(a => a.Applicant.Region == region && a.RecipientId == userId
                    && (fromDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)fromDate) >= 0)
                    && (toDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)toDate) <= 0));

                OrganizationStats stat = new OrganizationStats()
                {
                    Region = region,

                    FizikCount = GetPersonCount(applications).FizikCount,
                    YurikCount = GetPersonCount(applications).YurikCount,
                    MaleCount = GetPersonCount(applications).MaleCount,
                    FemaleCount = GetPersonCount(applications).FemaleCount,

                    DoneCount = GetStatusCount(applications).DoneCount,
                    ProcessCount = GetStatusCount(applications).ProcessCount,
                    RejectedCount = GetStatusCount(applications, true).RejectedCount,

                    AllCount = applications.Count(),
                };
                stats.Add(stat);
            }

            return stats;
        }

        public List<Application> FiredApps(int userId, int takeCount)
        {
            IQueryable<Application> applicationsQuery = _context.Applications.Include(a => a.Applicant).Include(a => a.Answer)
                .Where(a => a.RecipientId == userId && a.ExpireTime <= DateTime.Now.AddDays(1) && a.Answer == null)
                .OrderBy(a => a.ExpireTime);

            if(takeCount != 0)
            {
                return applicationsQuery.Take(takeCount).ToList();
            }

            return applicationsQuery.ToList();
        }


        private static Stats GetStatusCount(IQueryable<Application> apps, bool? isBranch = false)
        {

            List<Application> appsDone = apps.Where(a => a.Answer.Status == AnswerStatus.Confirm).ToList();
            List<Application> appsProcess = apps.Where(a => !(a.Answer.Status == AnswerStatus.Confirm || a.Status == ApplicationStatus.RejectMod)).ToList();
            List<Application> appReject = apps.Where(a => (isBranch ?? false) ? a.Status == ApplicationStatus.RejectOrg : a.Status == ApplicationStatus.RejectMod).ToList();

            return new Stats()
            {
                DoneCount = appsDone.Count,
                ProcessCount = appsProcess.Count,
                RejectedCount = appReject.Count,
            };
        }

        private static OrganizationStats GetPersonCount(IQueryable<Application> apps)
        {
            List<Application> appsFizik = apps.Where(a => a.Applicant.Type == Types.Individual).ToList();
            List<Application> appsYurik = apps.Where(a => a.Applicant.Type == Types.Business).ToList();
            List<Application> appsMale = apps.Where(a => a.Applicant.Gender == Genders.Male).ToList();
            List<Application> appsFemale = apps.Where(a => a.Applicant.Gender == Genders.Female).ToList();

            return new OrganizationStats()
            {
                FizikCount = appsFizik.Count,
                YurikCount = appsYurik.Count,
                MaleCount = appsMale.Count,
                FemaleCount = appsFemale.Count,
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