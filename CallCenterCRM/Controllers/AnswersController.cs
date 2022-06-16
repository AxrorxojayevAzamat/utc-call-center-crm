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
        private readonly IApplicationService _applicationService;

        public AnswersController(CallcentercrmContext context, IAttachmentService attachmentService, IApplicationService applicationService)
        {
            _context = context;
            _attachmentService = attachmentService;
            _applicationService = applicationService;
        }

        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Answers
                .Include(a => a.Application)
                    .ThenInclude(a => a.Recipient)
                .Include(a => a.Attachment)
                .Include(a => a.Author);
            return View(callcentercrmContext.ToList().OrderByDescending(a => a.CreatedDate));
        }

        [Authorize(Roles = "CrmModerator, CrmOrganization")]
        public async Task<IActionResult> AnswersList(int? authorId)
        {
            var callcentercrmContext = _context.Answers.Include(a => a.Author)
                .Where(a => a.AuthorId == authorId || a.Author.ModeratorId == authorId)
                .Include(a => a.Attachment);

            return View("Index", callcentercrmContext.ToList().OrderByDescending(a => a.CreatedDate));
        }

        public async Task<IActionResult> Details(int? id, int? userId, string? actionName)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Application)
                    .ThenInclude(a => a.Applicant)
                .Include(a => a.Attachment)
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            User user = _context.Users.Where(a => a.Id == userId).FirstOrDefault();

            bool willBeSave = answer.Status == AnswerStatus.Send && answer.Author.ModeratorId == userId
                || (answer.AuthorId == userId || answer.Author.ModeratorId == userId);

            if (answer.Status == AnswerStatus.Send && answer.Author.ModeratorId == userId)
            {
                answer.Status = AnswerStatus.GotMod;
            }

            if (answer.AuthorId == userId || answer.Author.ModeratorId == userId)
            {
                answer.IsGot = _applicationService.IsGotAnswer(user.Role, answer.Status);
            }

            ViewData["actionName"] = actionName;

            if (willBeSave)
            {
                _context.Update(answer);
                _context.SaveChanges();
            }

            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        public IActionResult Create(int applicationId, int authorId)
        {
            var application = _context.Applications.Where(a => a.Id == applicationId)
                .Include(a => a.Classification).Include(a => a.Recipient).ThenInclude(a => a.Moderator).First();

            AnswerStatus answerStatus = application.Recipient.Moderator != null ? AnswerStatus.Send : AnswerStatus.Confirm;

            ViewData["AppId"] = application.Id;
            ViewData["AppType"] = application.Type.GetDisplayName();
            ViewData["AppMeaning"] = application.MeaningOfApplication;
            ViewData["AppClassification"] = application.Classification.Title;

            Answer answer = new Answer()
            {
                ResponsiblePerson = $"{application.Recipient.Surname} {application.Recipient.Firstname} {application.Recipient.Middlename}",
                ApplicationId = applicationId,
                AuthorId = authorId,
                Status = answerStatus,
            };
            return View(answer);
        }

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

                return RedirectToAction(nameof(AnswersList), new { authorId = answer.AuthorId });
            }

            return View(answer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = _context.Answers.Include(a => a.Application)
                .ThenInclude(a => a.Classification).Include(a => a.Attachment)
                .Where(a => a.Id == id).FirstOrDefault();

            if (answer == null)
            {
                return NotFound();
            }
            ViewData["AppType"] = answer.Application.Type.GetDisplayName();
            ViewData["AppMeaning"] = answer.Application.MeaningOfApplication;
            ViewData["AppClassification"] = answer.Application.Classification.Title;

            return View(answer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Answer answer, IFormFile file)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

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

                    answer.Status = AnswerStatus.Edit;
                    answer.IsGot = false;
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
                return RedirectToAction(nameof(AnswersList), new { authorId = answer.AuthorId });
            }

            return View(answer);
        }

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
            var answers = _context.Answers.Include(a => a.Author)
                .Where(a => a.AuthorId == authorId && a.Status == AnswerStatus.Reject).ToList().OrderByDescending(a => a.CreatedDate);

            return View("Index", answers);
        }

        [Authorize(Roles = "CrmModerator")]
        [HttpGet]
        public IActionResult Edited(int authorId)
        {
            var answers = _context.Answers.Include(a => a.Author)
                .Where(a => a.Author.ModeratorId == authorId && a.Status == AnswerStatus.Edit).ToList().OrderByDescending(a => a.CreatedDate);

            return View("Index", answers);
        }

        [HttpGet]
        public async Task<IActionResult> Reject(int? id)
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

        [Authorize(Roles = "CrmModerator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var answer = _context.Answers.Include(a => a.Author).FirstOrDefault(a => a.Id == id);
            answer.Status = AnswerStatus.Reject;
            answer.IsGot = false;
            _context.Update(answer);
            _context.SaveChanges();

            return RedirectToAction(nameof(AnswersList), new { authorId = answer.Author.ModeratorId });
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int? id)
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

        [Authorize(Roles = "CrmModerator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(int id)
        {
            var answer = _context.Answers.Include(a => a.Author).FirstOrDefault(a => a.Id == id);
            answer.Status = AnswerStatus.Confirm;
            answer.IsGot = false;
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
