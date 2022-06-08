#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CallCenterCRM.Data;
using CallCenterCRM.Models;
using PagedList;

namespace CallCenterCRM
{
    public class ApplicantsController : Controller
    {
        private readonly CallcentercrmContext _context;

        public ApplicantsController(CallcentercrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? surname, string? firstname, string? middlename, int? appcount,
            int? region, int? citydistrictid, string? contact, string? extracontact, DateTime? birthdate, int? gender, string? address,
            int? page, int? pageSize)
        {

            // searching from DB
            var applicants = _context.Applicants
                .Include(a => a.Applications)
                .Include(a => a.CityDistrict)
                .Include(a => a.Organization)
                .Where(a => (!String.IsNullOrEmpty(address) ? a.Address.ToLower().Contains(address.ToLower()) : true)
                && ((appcount != null && appcount != 0) ? a.Applications.Count == appcount : true)
                && (!String.IsNullOrEmpty(extracontact) ? a.ExtraContact == extracontact : true)
                && (birthdate != null ? a.BirthDate.Equals(birthdate) : true)
                && ((citydistrictid != null && citydistrictid != 0) ? a.CityDistrictId == citydistrictid : true)
                && ((region != null && region != 0) ? ((int)a.Region) == region : true)
                && (!String.IsNullOrEmpty(contact) ? a.Contact == contact : true)
                && ((gender != null) ? ((int)a.Gender) == gender : true)
                && (!String.IsNullOrEmpty(middlename) ? a.Middlename.ToLower().Contains(middlename.ToLower()) : true)
                && (!String.IsNullOrEmpty(firstname) ? a.Firstname.ToLower().Contains(firstname.ToLower()) : true)
                && (!String.IsNullOrEmpty(surname) ? a.Surname.ToLower().Contains(surname.ToLower()) : true));
            // \ searching from DB


            // select-option values
            ViewData["RegionsList"] = new SelectList(new Applicant().RegionsList, "Value", "Text", region);
            ViewData["CityDistrictList"] = new SelectList(_context.Citydistricts, "Id", "Title", citydistrictid);
            ViewData["GendersList"] = new SelectList(new Applicant().GendersList, "Value", "Text", gender);
            // \ select-option values 


            // pagination
            ViewData["Surname"] = surname ?? string.Empty;
            ViewData["Firstname"] = firstname ?? string.Empty;
            ViewData["Middlename"] = middlename ?? string.Empty;
            ViewData["Appcount"] = appcount ?? null;
            ViewData["Birthdate"] = birthdate ?? null;
            ViewData["Contact"] = contact ?? string.Empty;
            ViewData["ExtraContact"] = extracontact ?? string.Empty;
            ViewData["Region"] = region ?? null;
            ViewData["City"] = citydistrictid ?? null;
            ViewData["Gender"] = gender ?? null;
            ViewData["Address"] = address ?? string.Empty;

            int allCount = _context.Applicants.ToList().Count;
            int searchedCount = applicants.ToList().Count;
            int size = pageSize ?? 20;
            int pageNumber = page ?? 1;

            ViewData["allCount"] = allCount;
            ViewData["searchedCount"] = searchedCount;
            ViewData["pageSize"] = size;
            ViewData["pageNumber"] = pageNumber;
            // \ pagination


            return View(applicants.ToPagedList(pageNumber, size));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants
                .Include(a => a.CityDistrict)
                .Include(a => a.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title");
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Title");
            Applicant applicant = new Applicant()
            {
                BirthDate = DateTime.Today.AddYears(-18),
            };
            return View(applicant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Applicant applicant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicant);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id = applicant.Id });
            }
            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title", applicant.CityDistrictId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Title", applicant.OrganizationId);
            return View(applicant);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }
            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title", applicant.CityDistrictId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", applicant.OrganizationId);
            return View(applicant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Applicant applicant)
        {
            if (id != applicant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicant);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantExists(applicant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = applicant.Id });
            }
            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title", applicant.CityDistrictId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Title", applicant.OrganizationId);
            return View(applicant);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants
                .Include(a => a.CityDistrict)
                .Include(a => a.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            _context.Applicants.Remove(applicant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantExists(int id)
        {
            return _context.Applicants.Any(e => e.Id == id);
        }

        public IActionResult SetStatus(ApplicationStatus status)
        {
            return View("Index");
        }
    }
}
