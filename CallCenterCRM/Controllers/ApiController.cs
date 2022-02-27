using CallCenterCRM.Data;
using CallCenterCRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace CallCenterCRM.Controllers
{
    public class ApiController : Controller
    {
        private readonly CallcentercrmContext _context;
        public ApiController(CallcentercrmContext context)
        {
            _context = context;
        }

        public JsonResult Cities(int Id)
        {
            var citiesOrDistricts = _context.Citydistricts
                .Where(c => c.Region == (Regions) Id)
                .Select(c => new
                {
                    Id = c.Id,
                    Title = c.Title,
                    RegionId = c.Region
                }).ToList();

            return Json(citiesOrDistricts);
        }

        public JsonResult Classifications(int Id)
        {
            var classifications = _context.Classifications.Find(Id);

            return Json(classifications);
        }
    }
}
