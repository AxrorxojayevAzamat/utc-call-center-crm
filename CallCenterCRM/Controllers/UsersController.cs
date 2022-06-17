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
using CallCenterCRM.Interfaces;

namespace CallCenterCRM.Controllers
{

    public class UsersController : Controller
    {
        private readonly CallcentercrmContext _context;
        private readonly IdentityService identityService;
        private readonly IUserService _userService;
        private const string nameIdentityId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public UsersController(CallcentercrmContext context, IdentityService identityService, IUserService userService)
        {
            _context = context;
            this.identityService = identityService;
            _userService = userService;
        }

        [Authorize(Roles = "CrmAdmin")]
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Users.Include(u => u.Moderator).OrderByDescending(u => u.CreatedDate);
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
                .Include(u => u.Direction)
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
            ViewData["ModeratorId"] = new SelectList(_context.Users.Where(u => u.Role == Roles.CrmModerator), "Id", "Username");
            ViewData["DirectionId"] = new SelectList(_context.Directions, "Id", "Title");
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
                ModeratorId = userInput.Role == Roles.CrmModerator || userInput.Role == Roles.CrmOperator ? null : userInput.ModeratorId,
                DirectionId = userInput.Role == Roles.CrmOrganization || userInput.Role == Roles.CrmOperator ? null : userInput.DirectionId,
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

        [Authorize(Roles = "CrmAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.Include(u => u.Direction).Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            ViewData["ModeratorId"] = new SelectList(_context.Users.Where(u => u.Role == Roles.CrmModerator), "Id", "Username", user.ModeratorId);
            ViewData["DirectionId"] = new SelectList(_context.Directions, "Id", "Title", user.DirectionId);
            UpdateUserInput userModel = new UpdateUserInput()
            {
                Id = user.Id,
                Title = user.Title,
                Username = user.Username,
                Email = user.Email,
                Contact = user.Contact,
                Role = user.Role,
                City = user.City,
                ModeratorId = user.ModeratorId,
                DirectionId = user.DirectionId,
            };

            return View(userModel);
        }

        [Authorize(Roles = "CrmAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind] UpdateUserInput userInput)
        {
            var user = _context.Users.Where(u => u.Id == userInput.Id).FirstOrDefault();
            string oldRoleName = Enum.GetName(typeof(Roles), user.Role);
            user.Title = userInput.Title;
            user.Username = userInput.Username;
            user.Email = userInput.Email;
            user.Contact = userInput.Contact;
            user.Role = userInput.Role;
            user.City = userInput.City;
            user.ModeratorId = userInput.Role == Roles.CrmModerator || userInput.Role == Roles.CrmOperator ? null : userInput.ModeratorId;
            user.DirectionId = userInput.Role == Roles.CrmOrganization || userInput.Role == Roles.CrmOperator ? null : userInput.DirectionId;

            if (ModelState.IsValid)
            {
                string roleName = Enum.GetName(typeof(Roles), userInput.Role);

                try
                {
                    User userResponse = await identityService.UpdateUser(user, roleName, oldRoleName);
                    user.Role = userResponse.Role;

                    _context.Update(user);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public async Task<IActionResult> Profile(int? id)
        {
            var userIdentity = User.Identities.First().Claims.First(c => c.Type == nameIdentityId).Value;

            if (id != _userService.GetUserId(userIdentity))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

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
                ViewData["DirectionId"] = new SelectList(_context.Directions, "Id", "Title", user.DirectionId);
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
                DirectionId = user.DirectionId,
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
                    user.DirectionId = profile.DirectionId;
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

        [HttpGet]
        public IActionResult PasswordChange(int id, bool byAdmin = false)
        {
            User user = _context.Users.Find(id);
            PasswordChangeInput passwordChange = new PasswordChangeInput()
            {
                UserId = user.Id,
                CreatedDate = user.CreatedDate,
                OldPassword = user.Password,
                Password = byAdmin ? user.Password : null,
            };
            ViewData["byAdmin"] = byAdmin;

            return View(passwordChange);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordChange(int id, PasswordChangeInput passwordChange)
        {
            var userIdentity = User.Identities.First().Claims.First(c => c.Type == nameIdentityId).Value;
            Roles userRole = _userService.GetRole(userIdentity);

            if (id != passwordChange.UserId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    User user = _context.Users.Find(id);
                    try
                    {
                        int statusCode = (await identityService.ChangePassword(passwordChange, user.IdentityId)).StatusCode;
                        if (statusCode == 200)
                        {
                            user.Password = passwordChange.NewPassword;
                            _context.Update(user);
                            _context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(passwordChange.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", userRole != Roles.CrmAdmin ? "Home" : "Users");
            }
            return View(passwordChange);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
