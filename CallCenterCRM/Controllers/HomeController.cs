using CallCenterCRM.Data;
using CallCenterCRM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CallCenterCRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CallcentercrmContext _context;

        public HomeController(ILogger<HomeController> logger, CallcentercrmContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            Guid valueIdentityId = Guid.Empty;
            string nameIdentityId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            
            var userIdentity = User.Identities.First().Claims.First(c => c.Type == nameIdentityId).Value;
            valueIdentityId = new Guid(userIdentity);
            int userId = _context.Users.FirstOrDefault(d => d.IdentityId == valueIdentityId).Id;
            ViewData["userId"] = userId;

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
            return SignOut("Cookies","oidc");
        }
    }
}