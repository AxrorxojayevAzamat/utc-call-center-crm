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
    public class ApplicationsController : Controller
    {
        private readonly CallcentercrmContext _context;

        public ApplicationsController(CallcentercrmContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Applications.Include(a => a.Applicant).Include(a => a.Attachment).Include(a => a.Classification).Include(a => a.Recipient);
            return View(await callcentercrmContext.ToListAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public IActionResult Create()
        {
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "Id", "AdditionalNote");
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension");
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Direction");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "City");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Application application)
        {
            if (ModelState.IsValid)
            {
                _context.Add(application);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "Id", "AdditionalNote", application.ApplicantId);
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension", application.AttachmentId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Direction", application.ClassificationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "City", application.RecipientId);
            return View(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "Id", "AdditionalNote", application.ApplicantId);
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension", application.AttachmentId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Direction", application.ClassificationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "City", application.RecipientId);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.Id))
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
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "Id", "AdditionalNote", application.ApplicantId);
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension", application.AttachmentId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Direction", application.ClassificationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "City", application.RecipientId);
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
