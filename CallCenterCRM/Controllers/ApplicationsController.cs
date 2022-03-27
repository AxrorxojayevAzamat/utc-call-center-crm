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

        // GET: Applications
        [Authorize(Roles="CrmOperator")]
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Applications
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient);

            return View(await callcentercrmContext.ToListAsync());
        }

        [Authorize(Roles = "CrmModerator, CrmOrganization")]
        public async Task<IActionResult> AppsList(int? recipientId)
        {
            var callcentercrmContext = _context.Applications.Where(a => a.RecipientId == recipientId)
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient);

            return View("Index", await callcentercrmContext.ToListAsync());
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
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (application.Status == ApplicationStatus.SendMod)
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

        // GET: Applications/Create
        public IActionResult Create(int id)
        {
            Application application = new Application()
            {
                ApplicantId = id
            };

            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "OriginName");
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Title");
            ViewData["RecipientId"] = new SelectList(_context.Users, "Id", "Username", application.RecipientId);
            return View(application);
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                        attachmentId = await _attachmentService.UploadFileToStorage(file);
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
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "Id", "AdditionalNote", application.ApplicantId);
            ViewData["AttachmentId"] = new SelectList(_context.Attachments, "Id", "Extension", application.AttachmentId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Direction", application.ClassificationId);
            ViewData["RecipientId"] = new SelectList(_context.Users, "Id", "Username", application.RecipientId);
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
                    application.Status = ApplicationStatus.Edit;
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
                .Include(a => a.Recipient).ToList();

            return View("Index", applications);
        }

        public IActionResult RejectedMod()
        {
            var applications = _context.Applications.Where(a => a.Status == ApplicationStatus.RejectMod)
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient).ToList();

            return View("Index", applications);
        }
        public IActionResult RejectedOrg()
        {
            var applications = _context.Applications
                .Where(a => (a.Status == ApplicationStatus.RejectOrg))
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient).ToList();

            return View("Index", applications);
        }

        public IActionResult Delayed()
        {
            var applications = _context.Applications.Where(a => a.Status == ApplicationStatus.Delay)
                .Include(a => a.Applicant)
                    .ThenInclude(a => a.CityDistrict)
                .Include(a => a.Attachment)
                .Include(a => a.Classification)
                .Include(a => a.Recipient).ToList();

            return View("Index", applications);
        }

       

        // GET: Applications/Delete/5
        //public async Task<IActionResult> SendOrg(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var application = await _context.Applications
        //        .Include(a => a.Applicant)
        //            .ThenInclude(a => a.CityDistrict)
        //        .Include(a => a.Attachment)
        //        .Include(a => a.Classification)
        //        .Include(a => a.Recipient)
        //        .FirstOrDefaultAsync(m => m.Id == id);


        //    if (application == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(application);
        //}

        //// POST: Applications/Delete/5
        //[HttpPost, ActionName("SendOrg")]
        //[ValidateAntiForgeryToken]
        //public IActionResult SendOrg(int id)
        //{
        //    var application = _context.Applications.Find(id);
        //    _context.Applications.Remove(application);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
