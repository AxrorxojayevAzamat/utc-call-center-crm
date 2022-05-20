using Microsoft.AspNetCore.Mvc;

namespace CallCenterCRM.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult AccessDenied()
        {
            return View("Denied");
        }
    }
}
