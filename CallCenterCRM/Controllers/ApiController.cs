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
                .Where(c => c.Region == (Regions)Id)
                .Select(c => new
                {
                    Id = c.Id,
                    Title = c.Title,
                    RegionId = c.Region
                }).ToList();

            return Json(citiesOrDistricts);
        }

        public JsonResult Classification(int Id)
        {
            var classification = _context.Classifications.Find(Id);
            var direction = _context.Directions.Find(classification.DirectionId);
            var moderators = _context.Users.Where(u => u.DirectionId == classification.DirectionId).Select(c => new
            {
                Id = c.Id,
                Title = c.Title
            }).ToList();

            return Json(new
            {
                directionTitle = direction.Title,
                directionConsequence = direction.Consequence,
                moderators
            });
        }
    }
}
