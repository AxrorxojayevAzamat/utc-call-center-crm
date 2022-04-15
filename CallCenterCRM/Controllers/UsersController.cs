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
using CallCenterCRM.Features.Identity;
using System.ComponentModel.DataAnnotations;
using CallCenterCRM.Forms;
using Microsoft.AspNetCore.Authorization;

namespace CallCenterCRM.Controllers
{

    public class UsersController : Controller
    {
        private readonly CallcentercrmContext _context;
        private readonly IdentityService identityService;

        public UsersController(CallcentercrmContext context, IdentityService identityService)
        {
            _context = context;
            this.identityService = identityService;
        }

        [Authorize(Roles = "CrmAdmin")]
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Users.Include(u => u.Moderator);
            return View(await callcentercrmContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Moderator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            RegisterUserInput user = new RegisterUserInput();
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Username");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] RegisterUserInput userInput)
        {
            User user = new User()
            {
                Title = userInput.Title,
                Username = userInput.Username,
                Email = userInput.Email,
                Contact = userInput.Contact,
                Password = userInput.Password,
                Role = userInput.Role,
                City = userInput.City,
            };

            if (ModelState.IsValid)
            {
                string roleName = Enum.GetName(typeof(Roles), userInput.Role);
                try
                {
                    User userResponse = await identityService.Register(userInput, roleName);
                    user.IdentityId = userResponse.IdentityId;
                    user.Role = userResponse.Role;

                    _context.Add(user);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }
            //ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", user.ModeratorId);
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Username", user.ModeratorId);
            ViewBag.Role = user.Role;
            if (user.Role == Roles.CrmModerator)
            {
                ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Title", user.ClassificationId);
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", user.ModeratorId);
            return View(user);
        }

        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var moderators = _context.Users.Where(a => a.Role == Roles.CrmModerator).ToList();
            ViewData["ModeratorId"] = new SelectList(moderators, "Id", "Username", user.ModeratorId);
            ViewBag.Role = user.Role;
            if (user.Role == Roles.CrmModerator)
            {
                ViewData["ClassificationId"] = new SelectList(_context.Classifications, "Id", "Title", user.ClassificationId);
            }
            ProfileInput profile = new ProfileInput()
            {
                Id = user.Id,
                CreatedDate = user.CreatedDate,
                Surname = user.Surname,
                Firstname = user.Firstname,
                Middlename = user.Middlename,
                PassportData = user.PassportData,
                Address = user.Address,
                ClassificationId = user.ClassificationId,
            };
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(int id, ProfileInput profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    User user = _context.Users.Find(id);
                    user.Surname = profile.Surname;
                    user.Firstname = profile.Firstname;
                    user.Middlename = profile.Middlename;
                    user.PassportData = profile.PassportData;
                    user.Address = profile.Address;
                    user.ModeratorId = profile.ModeratorId;
                    user.ClassificationId = profile.ClassificationId;
                    _context.Update(user);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(profile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", profile.ModeratorId);
            return View(profile);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Moderator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Branches(int? moderatorId)
        {
            var branches = _context.Users.Where(u => u.ModeratorId == moderatorId).ToList();

            return View("Branches", branches);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
