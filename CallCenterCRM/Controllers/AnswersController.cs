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
using Microsoft.AspNetCore.Authorization;
using CallCenterCRM.Interfaces;

namespace CallCenterCRM.Controllers
{
    public class AnswersController : Controller
    {
        private readonly CallcentercrmContext _context;
        private readonly IAttachmentService _attachmentService;

        public AnswersController(CallcentercrmContext context, IAttachmentService attachmentService)
        {
            _context = context;
            _attachmentService = attachmentService;
        }

        // GET: Answers
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Answers
                .Include(a => a.Application)
                    .ThenInclude(a => a.Recipient)
                .Include(a => a.Attachment)
                .Include(a => a.Author);
            return View(await callcentercrmContext.ToListAsync());
        }

        [Authorize(Roles = "CrmModerator, CrmOrganization")]
        public async Task<IActionResult> AnswersList(int? authorId)
        {
            var callcentercrmContext = _context.Answers.Include(a => a.Author)
                .Where(a => a.AuthorId == authorId || a.Author.ModeratorId == authorId)
                .Include(a => a.Attachment);

            return View("Index", await callcentercrmContext.ToListAsync());
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id, int? userId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Application)
                .Include(a => a.Attachment)
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (answer.Status == AnswerStatus.Send && answer.Author.ModeratorId == userId)
            {
                answer.Status = AnswerStatus.GotMod;
                _context.Update(answer);
                _context.SaveChanges();
            }

            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create(int applicationId, int authorId)
        {
            var application = _context.Applications.Where(a => a.Id == applicationId).Include(a => a.Classification).First();

            ViewData["AppType"] = application.Type.GetDisplayName();
            ViewData["AppMeaning"] = application.MeaningOfApplication;
            ViewData["AppClassification"] = application.Classification.Title;

            Answer answer = new Answer() {
                ApplicationId = applicationId,
                AuthorId = authorId
            };
            return View(answer);
        }

        // POST: Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Answer answer, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int attachmentId = -1;

                    if (file != null)
                    {
                        attachmentId = _attachmentService.UploadFileToStorage(file);
                    }

                    if (attachmentId > -1)
                    {
                        answer.AttachmentId = attachmentId;
                    }

                    _context.Answers.Add(answer);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return RedirectToAction(nameof(Index));
            }

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
            //ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", answer.OrganizationId);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ResponsiblePerson,Executor,ResponseLetter,AttachmentId,RegisterNumber,Result,Conclusion,OrganizationId,ApplicationId")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    answer.Status = AnswerStatus.Edit;
                    _context.Update(answer);
                    _context.SaveChanges();
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
            //ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", answer.OrganizationId);
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
                .Include(a => a.Author)
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

        [Authorize(Roles = "CrmOrganization")]
        [HttpGet]
        public IActionResult Rejected(int authorId)
        {
            var answers = _context.Answers.Where(a => a.AuthorId == authorId && a.Status == AnswerStatus.Reject);

            return View("Index", answers);
        }

        [Authorize(Roles = "CrmModerator")]
        [HttpGet]
        public IActionResult Edited(int authorId)
        {
            var answers = _context.Answers.Include(a => a.Author)
                .Where(a => a.Author.ModeratorId == authorId && a.Status == AnswerStatus.Edit);

            return View("Index", answers);
        }

        [Authorize(Roles = "CrmModerator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int answerId)
        {
            var answer = _context.Answers.FirstOrDefault(a => a.Id == answerId);
            answer.Status = AnswerStatus.Reject;
            _context.Update(answer);
            _context.SaveChanges();

            return View(nameof(AnswersList), new { authorId = answer.Author.ModeratorId });
        }

        [Authorize(Roles = "CrmModerator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(int id)
        {
            var answer = _context.Answers.FirstOrDefault(a => a.Id == id);
            answer.Status = AnswerStatus.Confirm;
            _context.Update(answer);
            _context.SaveChanges();

            return RedirectToAction(nameof(AnswersList), new { authorId = answer.Author.ModeratorId });
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }

    }
}
