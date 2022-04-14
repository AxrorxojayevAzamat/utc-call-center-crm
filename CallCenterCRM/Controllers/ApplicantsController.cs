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

namespace CallCenterCRM
{
    public class ApplicantsController : Controller
    {
        private readonly CallcentercrmContext _context;

        public ApplicantsController(CallcentercrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Applicants
                .Include(a => a.CityDistrict)
                .Include(a => a.Organization)
                .Include(a => a.Applications);
            return View(await callcentercrmContext.ToListAsync());
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
