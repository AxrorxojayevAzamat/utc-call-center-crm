﻿#nullable disable
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

        // GET: Applicants
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Applicants
                .Include(a => a.CityDistrict)
                .Include(a => a.Organization);
            return View(await callcentercrmContext.ToListAsync());
        }

        // GET: Applicants/Details/5
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

        // GET: Applicants/Create
        public IActionResult Create()
        {
            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title");
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City");
            Applicant applicant = new Applicant();
            return View(applicant);
        }

        // POST: Applicants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ReferenceSource,Surname,Firstname,Middlename,Contact,ExtraContact,Region,CityDistrictId,Maxalla,Address,Gender,BirthDate,Type,Employment,NumberOfApplication,Confidentiality,MeaningOfApplication,AdditionalNote,OrganizationId")] Applicant applicant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicant);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title", applicant.CityDistrictId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", applicant.OrganizationId);
            return View(applicant);
        }

        // GET: Applicants/Edit/5
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

        // POST: Applicants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReferenceSource,Surname,Firstname,Middlename,Contact,ExtraContact,Region,CityDistrictId,Maxalla,Address,Gender,BirthDate,Type,Employment,NumberOfApplication,Confidentiality,MeaningOfApplication,AdditionalNote,OrganizationId,CreatedDate,UpdatedDate")] Applicant applicant)
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
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title", applicant.CityDistrictId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", applicant.OrganizationId);
            return View(applicant);
        }

        // GET: Applicants/Delete/5
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

        // POST: Applicants/Delete/5
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
    }
}
