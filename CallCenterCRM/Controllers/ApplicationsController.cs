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
using CallCenterCRM.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CallCenterCRM
{
    public class ApplicationsController : Controller
    {
        private readonly CallcentercrmContext _context;
        private readonly IAttachmentService _attachmentService;
        public ApplicationsController(CallcentercrmContext context, IAttachmentService attachmentService)
        {
            _context = context;
            _attachmentService = attachmentService;
        }

        [Authorize(Roles = "CrmOperator")]
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Applications
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Answer)
                .Include(a => a.Classification)
                .Include(a => a.Recipient).OrderByDescending(a => a.Id);

            return View(await callcentercrmContext.ToListAsync());
        }

        [Authorize(Roles = "CrmModerator, CrmOrganization")]
        public async Task<IActionResult> AppsList(int? recipientId)
        {
            var callcentercrmContext = _context.Applications.Include(a => a.Recipient)
                .Where(a => a.RecipientId == recipientId || a.Recipient.ModeratorId == recipientId)
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Answer)
                .Include(a => a.Classification)
                .Include(a => a.Recipient).OrderByDescending(a => a.Id);

            return View("Index", await callcentercrmContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id, int? userId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient)
                .Include(a => a.Answer)
                    .ThenInclude(a => a.Attachment)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (application.Status == ApplicationStatus.SendMod && application.RecipientId == userId)
            {
                application.Status = ApplicationStatus.GotMod;
                _context.Update(application);
                _context.SaveChanges();
            }

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        public IActionResult Create(int applicantId)
        {
            DateTime date = DateTime.Now.AddDays(3);
            Application application = new Application()
            {
                ExpireTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, date.Kind),
            };
            application.ApplicantId = applicantId;

            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Title");
            ViewData["RecipientId"] = new SelectList(_context.Users, "Id", "Title", application.RecipientId);
            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Application application, IFormFile file)
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
                        application.AttachmentId = attachmentId;
                    }
                    _context.Applications.Add(application);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(application);
        }

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
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Title", application.ClassificationId);
            ViewData["RecipientId"] = new SelectList(_context.Users, "Id", "Username", application.RecipientId);

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Application application, IFormFile file)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    application.Status = application.Status == ApplicationStatus.RejectMod ?
                        ApplicationStatus.Edit : ApplicationStatus.SendMod;

                    application.IsChanged = true;

                    int attachmentId = -1;

                    if (file != null)
                    {
                        attachmentId = _attachmentService.UploadFileToStorage(file);
                    }

                    if (attachmentId > -1)
                    {
                        application.AttachmentId = attachmentId;
                    }

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

            return View(application);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
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

        public IActionResult Save(int applicantId, [Bind] Application application)
        {
            application.ApplicantId = applicantId;

            _context.Applications.Add(application);
            _context.SaveChanges();
            return View(nameof(Index));
        }

        public IActionResult SetStatus(int id, ApplicationStatus status)
        {
            var application = _context.Applications.Find(id);
            application.Status = status;

            _context.Update(application);
            _context.SaveChanges();

            return View("Details", id);
        }

        [Authorize(Roles = "CrmOperator")]
        public IActionResult ToggleSelected(int Id)
        {
            var application = _context.Applications.Find(Id);
            application.IsSelected = !application.IsSelected;

            _context.Update(application);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { Id });
        }

        public IActionResult Selected()
        {
            var applications = _context.Applications.Where(a => a.IsSelected == true)
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient).OrderByDescending(a => a.Id).ToList();

            return View("Index", applications);
        }

        public IActionResult RejectedMod()
        {
            var applications = _context.Applications.Where(a => a.Status == ApplicationStatus.RejectMod)
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient).OrderByDescending(a => a.Id).ToList();

            return View("Index", applications);
        }
        public IActionResult RejectedOrg(int recipientId)
        {
            var applications = _context.Applications.Include(a => a.Recipient)
                .Where(a => (a.Status == ApplicationStatus.RejectOrg || a.Status == ApplicationStatus.RejectMod)
                && (a.Recipient.ModeratorId == recipientId || a.Recipient.Id == recipientId))
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification).OrderByDescending(a => a.Id)
                .ToList();

            return View("Index", applications);
        }

        public IActionResult Delayed(int recipientId)
        {
            var applications = _context.Applications.Include(a => a.Recipient)
                .Where(a => a.Status == ApplicationStatus.Delay && (a.Recipient.ModeratorId == recipientId || a.Recipient.Id == recipientId))
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient).OrderByDescending(a => a.Id).ToList();

            return View("Index", applications);
        }

        [HttpGet]
        public async Task<IActionResult> RejectMod(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
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

        [Authorize(Roles = "CrmModerator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectMod(int id)
        {
            var application = _context.Applications.FirstOrDefault(a => a.Id == id);
            application.Status = ApplicationStatus.RejectMod;
            _context.Update(application);
            _context.SaveChanges();

            return RedirectToAction(nameof(AppsList), new { recipientId = application.RecipientId });
        }

        [HttpGet]
        public IActionResult SendOrg(int id, int moderatorId)
        {
            var branches = _context.Users.Where(u => u.ModeratorId == moderatorId).ToList();
            var application = _context.Applications.FirstOrDefault(a => a.Id == id);
            ViewData["RecipientId"] = new SelectList(branches, "Id", "Title", 0);

            return View(application);
        }


        [Authorize(Roles = "CrmModerator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendOrg(int id, Application app)
        {
            var application = _context.Applications.FirstOrDefault(a => a.Id == id);
            int recipientId = application.RecipientId;

            if (id != app.Id)
            {
                return NotFound();
            }
            try
            {
                application.Status = ApplicationStatus.SendOrg;
                application.RecipientId = app.RecipientId;
                _context.Update(application);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(app.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(AppsList), new { recipientId });
        }

        [HttpGet]
        public IActionResult RejectOrg(int id)
        {
            var application = _context.Applications.FirstOrDefault(a => a.Id == id);

            return View(application);
        }

        [Authorize(Roles = "CrmModerator,CrmOrganization")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectOrg(int id, Application app)
        {
            var application = _context.Applications.Include(a => a.Recipient).FirstOrDefault(a => a.Id == id);
            int recipientId = application.RecipientId;

            if (id != app.Id)
            {
                return NotFound();
            }
            try
            {
                application.Status = ApplicationStatus.RejectOrg;
                application.Reason = app.Reason;
                if (application.Recipient.ModeratorId == null)
                {
                    application.Status = ApplicationStatus.RejectMod;
                }
                _context.Update(application);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(app.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(AppsList), new { recipientId });
        }

        [HttpGet]
        public IActionResult Delay(int id)
        {
            var application = _context.Applications.FirstOrDefault(a => a.Id == id);

            return View(application);
        }

        [Authorize(Roles = "CrmModerator,CrmOrganization")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delay(int id, Application app)
        {
            var application = _context.Applications.Include(a => a.Recipient).FirstOrDefault(a => a.Id == id);
            int recipientId = application.RecipientId;

            if (id != app.Id)
            {
                return NotFound();
            }
            try
            {
                if (app.ExpireTime > application.ExpireTime)
                {
                    application.Status = ApplicationStatus.Delay;
                    application.IsDelayed = true;
                    _context.Update(application);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(app.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(AppsList), new { recipientId });
        }
    }
}
