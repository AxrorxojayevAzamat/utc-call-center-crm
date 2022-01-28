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
    public class AnswersController : Controller
    {
        private readonly CallcentercrmContext _context;

        public AnswersController(CallcentercrmContext context)
        {
            _context = context;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Answers.Include(a => a.Application).Include(a => a.Attachment).Include(a => a.Organization);
            return View(await callcentercrmContext.ToListAsync());
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Application)
                .Include(a => a.Attachment)
                .Include(a => a.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create()
        {
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Comment");
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension");
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City");
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResponsiblePerson,Executor,RerponseLetter,AttachmentId,RegisterNumber,Author,Result,Conclusion,OrganizationId,ApplicationId,CreatedDate,UpdatedDate")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Comment", answer.ApplicationId);
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension", answer.AttachmentId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", answer.OrganizationId);
            return View(answer);
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Comment", answer.ApplicationId);
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension", answer.AttachmentId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", answer.OrganizationId);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResponsiblePerson,Executor,RerponseLetter,AttachmentId,RegisterNumber,Author,Result,Conclusion,OrganizationId,ApplicationId,CreatedDate,UpdatedDate")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
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
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Comment", answer.ApplicationId);
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension", answer.AttachmentId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", answer.OrganizationId);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Application)
                .Include(a => a.Attachment)
                .Include(a => a.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
    }
}
