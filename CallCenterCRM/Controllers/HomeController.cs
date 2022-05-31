using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenterCRM.Data;
using CallCenterCRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using CallCenterCRM.Interfaces;

namespace CallCenterCRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CallcentercrmContext _context;
        private readonly IApplicationService _applicationService;
        private const string homeUrl = "/";

        public HomeController(ILogger<HomeController> logger, CallcentercrmContext context, IApplicationService applicationService)
        {
            _logger = logger;
            _context = context;
            _applicationService = applicationService;
        }

        public IActionResult Index()
        {
            //Guid valueIdentityId = Guid.Empty;
            //string nameIdentityId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            //var userIdentity = User.Identities.First().Claims.First(c => c.Type == nameIdentityId).Value;
            //valueIdentityId = new Guid(userIdentity);
            //int userId = _context.Users.FirstOrDefault(d => d.IdentityId == valueIdentityId).Id;

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
        public IActionResult StatisticsOperator()
        {
            var classification = _context.Classifications.Include(c => c.Applications).ToList();
            float countApps = _context.Applications.ToList().Count;
            ViewData["countApps"] = countApps;

            return View(classification);
        }

        [Authorize(Roles = "CrmModerator")]
        public IActionResult StatisticsModerator(int userId)
        {
            User? user = _context.Users
                .Include(u => u.Organizations)
                .Where(u => u.Id == userId).FirstOrDefault();

            ViewData["branches"] = user.Organizations.ToList();

            List<ModeratorStats>? moderatorStats = _applicationService.GetModeratorStats(userId);

            return View(moderatorStats);
        }

        [Authorize(Roles = "CrmOrganization,CrmModerator")]
        public IActionResult StatisticsOrganization(int userId)
        {
            List<OrganizationStats>? organizationStats = _applicationService.GetOrganizationStats(userId);

            return View(organizationStats);
        }

        [Authorize(Roles = "CrmModerator")]
        public IActionResult StatisticsByClassification(int userId)
        {
            User? user = _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Direction)
                    .ThenInclude(c => c.Classifications)
                .FirstOrDefault();

            var classifications = user?.Direction?.Classifications.ToList();
            var countApps = _context.Applications.Include(a => a.Recipient)
                .Where(a => a.RecipientId == userId || a.Recipient.ModeratorId == userId)
                .ToList();
            ViewData["countApps"] = (float)countApps.Count;

            return View("StatisticsOperator", classifications);
        }
    }
}