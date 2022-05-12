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
using CallCenterCRM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CallCenterCRM.Forms;

namespace CallCenterCRM
{
    public class ApplicationsController : Controller
    {
        private readonly CallcentercrmContext _context;
        private readonly IAttachmentService _attachmentService;
        private readonly IApplicationService _applicationService;
        public ApplicationsController(CallcentercrmContext context, IAttachmentService attachmentService, IApplicationService applicationService)
        {
            _context = context;
            _attachmentService = attachmentService;
            _applicationService = applicationService;
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

        public async Task<IActionResult> Details(int? id, int? userId, string? actionName)
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
            User user = _context.Users.Where(a => a.Id == userId).FirstOrDefault();
            if (application.Status == ApplicationStatus.SendMod && application.RecipientId == userId)
            {
                application.Status = ApplicationStatus.GotMod;
            }
            if ((application.RecipientId == userId || application.Recipient.ModeratorId == userId || application.Applicant.OrganizationId == userId)
                && userId != null && application.IsGot == false )
            {
                application.IsGot = _applicationService.IsGot(user.Role, application.Status);
            }
            _context.Update(application);
            _context.SaveChanges();
            ViewData["actionName"] = actionName;
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

        public IActionResult CreateApplicant()
        {
            DateTime date = DateTime.Now.AddDays(3);
            ApplicantAppInput applicantApp = new ApplicantAppInput()
            {
                ExpireTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, date.Kind),
                BirthDate = DateTime.Today.AddYears(-18),
            };

            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title");
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Title");
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Title");
            ViewData["RecipientId"] = new SelectList(_context.Users, "Id", "Title", applicantApp.RecipientId);
            return View("ApplicantCreate", applicantApp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApplicant(ApplicantAppInput applicantApp, IFormFile file)
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
                        applicantApp.AttachmentId = attachmentId;
                    }

                    Applicant applicant = new Applicant()
                    {
                        Address = applicantApp.Address,
                        BirthDate = applicantApp.BirthDate,
                        CityDistrictId = applicantApp.CityDistrictId,
                        Confidentiality = applicantApp.Confidentiality,
                        Contact = applicantApp.Contact,
                        Employment = applicantApp.Employment,
                        ExtraContact = applicantApp.ExtraContact,
                        Firstname = applicantApp.Firstname,
                        Gender = applicantApp.Gender,
                        Maxalla = applicantApp.Maxalla,
                        Middlename = applicantApp.Middlename,
                        OrganizationId = applicantApp.OrganizationId,
                        ReferenceSource = applicantApp.ReferenceSource,
                        Region = applicantApp.Region,
                        Surname = applicantApp.Surname,
                        Type = applicantApp.Type,
                    };

                    _context.Applicants.Add(applicant);
                    _context.SaveChanges();

                    Application application = new Application()
                    {
                        AdditionalNote = applicantApp.AdditionalNote,
                        ApplicantId = applicant.Id,
                        AttachmentId = applicantApp.AttachmentId,
                        ClassificationId = applicantApp.ClassificationId,
                        Comment = applicantApp.Comment,
                        ExpireTime = applicantApp.ExpireTime,
                        MeaningOfApplication = applicantApp.MeaningOfApplication,
                        Reason = applicantApp.Reason,
                        RecipientId = applicantApp.RecipientId,
                        Type = applicantApp.AppType
                    };
                    _context.Applications.Add(application);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(applicantApp);
        }

        public IActionResult EditApplicantCreateApp(int applicantId)
        {
            if (applicantId == null)
            {
                return NotFound();
            }
            var applicant = _context.Applicants.Where(a => a.Id == applicantId).FirstOrDefault();
            
            DateTime date = DateTime.Now.AddDays(3);
            ApplicantAppInput applicantApp = new ApplicantAppInput()
            {
                ApplicantId = applicant.Id,
                Address = applicant.Address,
                ExpireTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, date.Kind),
                BirthDate = applicant.BirthDate,
                CityDistrictId = applicant.CityDistrictId,
                Confidentiality = applicant.Confidentiality,
                Contact = applicant.Contact,
                CreatedDate = applicant.CreatedDate,
                Employment = applicant.Employment,
                ExtraContact = applicant.ExtraContact,
                Firstname = applicant.Firstname,
                Gender = applicant.Gender,
                Maxalla = applicant.Maxalla,
                Middlename = applicant.Middlename,
                OrganizationId = applicant.OrganizationId,
                ReferenceSource = applicant.ReferenceSource,
                Region = applicant.Region,
                Surname = applicant.Surname,
                Type = applicant.Type,
            };

            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title", applicantApp.CityDistrictId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Title", applicantApp.OrganizationId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Title", applicantApp.ClassificationId);
            ViewData["RecipientId"] = new SelectList(_context.Users, "Id", "Title", applicantApp.RecipientId);
            return View("ApplicantEditAppCreate", applicantApp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApplicantCreateApp(ApplicantAppInput applicantApp, IFormFile file)
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
                        applicantApp.AttachmentId = attachmentId;
                    }

                    Applicant applicant = new Applicant()
                    {
                        Id = applicantApp.ApplicantId,
                        Address = applicantApp.Address,
                        BirthDate = applicantApp.BirthDate,
                        CityDistrictId = applicantApp.CityDistrictId,
                        Confidentiality = applicantApp.Confidentiality,
                        Contact = applicantApp.Contact,
                        Employment = applicantApp.Employment,
                        ExtraContact = applicantApp.ExtraContact,
                        Firstname = applicantApp.Firstname,
                        Gender = applicantApp.Gender,
                        Maxalla = applicantApp.Maxalla,
                        Middlename = applicantApp.Middlename,
                        OrganizationId = applicantApp.OrganizationId,
                        ReferenceSource = applicantApp.ReferenceSource,
                        Region = applicantApp.Region,
                        Surname = applicantApp.Surname,
                        Type = applicantApp.Type,
                        CreatedDate = applicantApp.CreatedDate,
                    };

                    _context.Applicants.Update(applicant);
                    _context.SaveChanges();

                    Application application = new Application()
                    {
                        AdditionalNote = applicantApp.AdditionalNote,
                        ApplicantId = applicant.Id,
                        AttachmentId = applicantApp.AttachmentId,
                        ClassificationId = applicantApp.ClassificationId,
                        Comment = applicantApp.Comment,
                        ExpireTime = applicantApp.ExpireTime,
                        MeaningOfApplication = applicantApp.MeaningOfApplication,
                        Reason = applicantApp.Reason,
                        RecipientId = applicantApp.RecipientId,
                        Type = applicantApp.AppType
                    };
                    _context.Applications.Add(application);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(applicantApp);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var application = _context.Applications.Where(a => a.Id == id).Include(a => a.Applicant).FirstOrDefault();

            DateTime date = DateTime.Now.AddDays(3);
            ApplicantAppInput applicantApp = new ApplicantAppInput()
            {
                ApplicantId = application.Applicant.Id,
                Address = application.Applicant.Address,
                BirthDate = application.Applicant.BirthDate,
                CityDistrictId = application.Applicant.CityDistrictId,
                Confidentiality = application.Applicant.Confidentiality,
                Contact = application.Applicant.Contact,
                CreatedDate = application.Applicant.CreatedDate,
                Employment = application.Applicant.Employment,
                ExtraContact = application.Applicant.ExtraContact,
                Firstname = application.Applicant.Firstname,
                Gender = application.Applicant.Gender,
                Maxalla = application.Applicant.Maxalla,
                Middlename = application.Applicant.Middlename,
                OrganizationId = application.Applicant.OrganizationId,
                ReferenceSource = application.Applicant.ReferenceSource,
                Region = application.Applicant.Region,
                Surname = application.Applicant.Surname,
                Type = application.Applicant.Type,
                AdditionalNote = application.AdditionalNote,
                AttachmentId = application.AttachmentId,
                ClassificationId = application.ClassificationId,
                ExpireTime = application.ExpireTime,
                Comment = application.Comment,
                MeaningOfApplication = application.MeaningOfApplication,
                Reason = application.Reason,
                RecipientId = application.RecipientId,
                AppType = application.Type,
                AppCreatedDate = application.CreatedDate,
                AppId = (int)id,
            };

            ViewData["CityDistrictId"] = new SelectList(_context.Citydistricts, "Id", "Title", applicantApp.CityDistrictId);
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Title", applicantApp.OrganizationId);
            ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Title", applicantApp.ClassificationId);
            ViewData["RecipientId"] = new SelectList(_context.Users, "Id", "Title", applicantApp.RecipientId);
            return View("ApplicantEditAppEdit", applicantApp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicantAppInput applicantApp, IFormFile file)
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
                        applicantApp.AttachmentId = attachmentId;
                    }

                    Applicant applicant = new Applicant()
                    {
                        Id = applicantApp.ApplicantId,
                        Address = applicantApp.Address,
                        BirthDate = applicantApp.BirthDate,
                        CityDistrictId = applicantApp.CityDistrictId,
                        Confidentiality = applicantApp.Confidentiality,
                        Contact = applicantApp.Contact,
                        Employment = applicantApp.Employment,
                        ExtraContact = applicantApp.ExtraContact,
                        Firstname = applicantApp.Firstname,
                        Gender = applicantApp.Gender,
                        Maxalla = applicantApp.Maxalla,
                        Middlename = applicantApp.Middlename,
                        OrganizationId = applicantApp.OrganizationId,
                        ReferenceSource = applicantApp.ReferenceSource,
                        Region = applicantApp.Region,
                        Surname = applicantApp.Surname,
                        Type = applicantApp.Type,
                        CreatedDate = applicantApp.CreatedDate,
                    };

                    _context.Applicants.Update(applicant);
                    _context.SaveChanges();

                    Application application = new Application()
                    {
                        Id = applicantApp.AppId,
                        AdditionalNote = applicantApp.AdditionalNote,
                        ApplicantId = applicant.Id,
                        AttachmentId = applicantApp.AttachmentId,
                        ClassificationId = applicantApp.ClassificationId,
                        Comment = applicantApp.Comment,
                        ExpireTime = applicantApp.ExpireTime,
                        MeaningOfApplication = applicantApp.MeaningOfApplication,
                        Reason = applicantApp.Reason,
                        RecipientId = applicantApp.RecipientId,
                        Type = applicantApp.AppType,
                        CreatedDate = applicantApp.AppCreatedDate,
                        Status = applicantApp.AppStatus
                    };
                    application.Status = application.Status == ApplicationStatus.RejectMod ?
                    ApplicationStatus.Edit : ApplicationStatus.SendMod;

                    application.IsChanged = true;
                    application.IsGot = false;
                    _context.Applications.Update(application);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(applicantApp);
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
                .Where(a => (a.Status == ApplicationStatus.Delay || a.Status == ApplicationStatus.AskDelay || a.Status == ApplicationStatus.RejectDelay)
                && (a.Recipient.ModeratorId == recipientId || a.Recipient.Id == recipientId))
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
                application.IsGot = false;
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
                application.IsGot = false;
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
                    application.IsGot = false;
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


        [HttpGet]
        public async Task<IActionResult> AskDelay(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CrmModerator,CrmOrganization")]
        public IActionResult AskDelay(int id)
        {
            var application = _context.Applications.FirstOrDefault(a => a.Id == id);
            application.Status = ApplicationStatus.AskDelay;
            application.IsGot = false;
            _context.Update(application);
            _context.SaveChanges();

            return RedirectToAction(nameof(AppsList), new { recipientId = application.RecipientId });
        }

        [HttpGet]
        public async Task<IActionResult> RejectDelay(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CrmModerator,CrmOrganization")]
        public IActionResult RejectDelay(int id)
        {
            var application = _context.Applications.FirstOrDefault(a => a.Id == id);
            application.Status = ApplicationStatus.RejectDelay;
            application.IsGot = false;
            _context.Update(application);
            _context.SaveChanges();

            return RedirectToAction(nameof(AppsList), new { recipientId = application.RecipientId });
        }
    }
}