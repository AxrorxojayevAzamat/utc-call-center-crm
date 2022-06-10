using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenterCRM.Data;
using CallCenterCRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using CallCenterCRM.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace CallCenterCRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CallcentercrmContext _context;
        private readonly IApplicationService _applicationService;
        private readonly IUserService _userService;
        private const string homeUrl = "/";

        public HomeController(ILogger<HomeController> logger, CallcentercrmContext context, IApplicationService applicationService, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _applicationService = applicationService;
            _userService = userService;
        }

        public IActionResult Index(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            string nameIdentityId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var userIdentity = User.Identities.First().Claims.First(c => c.Type == nameIdentityId).Value;

            Roles userRole = _userService.GetRole(userIdentity);
            int userId = _userService.GetUserId(userIdentity);

            List<int> allCount = new List<int>();
            List<int> doneCount = new List<int>();
            List<int> doughnutCount = new List<int>();

            if (toDate != null)
                toDate = toDate.Value.AddDays(1);

            IQueryable<Application> applications = _context.Applications.Include(a => a.Applicant).Include(a => a.Recipient).Include(a => a.Answer)
                .Where(a => ((userRole == Roles.CrmModerator) ? (a.RecipientId == userId || a.Recipient.ModeratorId == userId)
                : (userRole == Roles.CrmOrganization) ? a.RecipientId == userId : true)
                && (fromDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)fromDate) >= 0)
                && (toDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)toDate) <= 0));

            foreach (Regions region in Enum.GetValues(typeof(Regions)))
            {
                int all = applications.Where(a => a.Applicant.Region == region).Count();
                int done = applications.Where(a => a.Applicant.Region == region && a.Answer.Status == AnswerStatus.Confirm).Count();
                allCount.Add(all);
                doneCount.Add(done);
            }

            foreach (DoughnutStatus doughnut in Enum.GetValues(typeof(DoughnutStatus)))
            {
                switch (doughnut)
                {
                    case DoughnutStatus.Done:
                        doughnutCount.Add(applications.Where(a => a.Answer.Status == AnswerStatus.Confirm).Count()); break;
                    case DoughnutStatus.Rejected:
                        doughnutCount.Add(applications.Where(a => (userRole == Roles.CrmOrganization ? a.Status == ApplicationStatus.RejectOrg
                            : a.Status == ApplicationStatus.RejectMod)).Count()); break;
                    case DoughnutStatus.Delayed:
                        doughnutCount.Add(applications.Where(a => a.Status == ApplicationStatus.AskDelay || a.Status == ApplicationStatus.Delay).Count()); break;
                    default:
                        doughnutCount.Add(applications.Where(a => !((userRole == Roles.CrmOrganization ? a.Status == ApplicationStatus.RejectOrg
                            : a.Status == ApplicationStatus.RejectMod) || a.Answer.Status == AnswerStatus.Confirm
                            || a.Status == ApplicationStatus.AskDelay || a.Status == ApplicationStatus.Delay)).Count()); break;
                }

            }

            ViewData["barAllData"] = JsonSerializer.Serialize(allCount);
            ViewData["barDoneData"] = JsonSerializer.Serialize(doneCount);
            ViewData["doughnutData"] = JsonSerializer.Serialize(doughnutCount);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("action")]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties() { RedirectUri = homeUrl }, "Cookies", "oidc");
        }

        [Authorize(Roles = "CrmOperator")]
        public IActionResult StatisticsOperator(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            var classifications = _context.Classifications.Include(c => c.Applications).Select(c => new Classification
            {
                Title = c.Title,
                Applications = c.Applications.Where(a => (fromDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)fromDate) >= 0)
                    && (toDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)toDate) <= 0)).ToList(),
            }).ToList();
            float countApps = _context.Applications.ToList().Count;
            ViewData["countApps"] = countApps;

            return View(classifications);
        }

        [Authorize(Roles = "CrmModerator")]
        public IActionResult StatisticsModerator(int userId, int? branchId, bool? byClassification, DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;

            if (byClassification ?? false)
            {
                User? user1 = _context.Users
               .Where(u => u.Id == userId)
               .Include(u => u.Direction)
                   .ThenInclude(c => c.Classifications)
               .FirstOrDefault();

                if (toDate != null)
                    toDate = toDate.Value.AddDays(1);

                var classifications = user1?.Direction?.Classifications.ToList();
                var countApps = _context.Applications.Include(a => a.Recipient)
                    .Where(a => (a.RecipientId == userId || a.Recipient.ModeratorId == userId)
                     && (fromDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)fromDate) >= 0)
                    && (toDate == null || DateTimeOffset.Compare((DateTimeOffset)a.CreatedDate, (DateTimeOffset)toDate) <= 0))
                    .ToList();
                ViewData["countApps"] = (float)countApps.Count;
                ViewData["byClassification"] = byClassification;

                return View("StatisticsOperator", classifications);
            }

            User? user = _context.Users
                .Include(u => u.Organizations)
                .Where(u => u.Id == userId).FirstOrDefault();

            ViewData["branches"] = user.Organizations.ToList();
            ViewData["BranchesList"] = new SelectList(user.Organizations, "Id", "Title", branchId);

            List<ModeratorStats>? moderatorStats = _applicationService.GetModeratorStats(userId, branchId, fromDate, toDate);

            return View(moderatorStats);
        }

        [Authorize(Roles = "CrmOrganization,CrmModerator")]
        public IActionResult StatisticsOrganization(int userId, DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            List<OrganizationStats>? organizationStats = _applicationService.GetOrganizationStats(userId, fromDate, toDate);

            return View(organizationStats);
        }

    }
}