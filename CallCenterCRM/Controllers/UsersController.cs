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

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var callcentercrmContext = _context.Users.Include(u => u.Moderator);
            return View(await callcentercrmContext.ToListAsync());
        }

        // GET: Users/Details/5
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

        // GET: Users/Create
        public IActionResult Create()
        {
            RegisterUserInput user = new RegisterUserInput();
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City");
            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    throw new Exception("Something wrong with server!!!");
                }

                return RedirectToAction(nameof(Index));
            }
            //ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "City", user.ModeratorId);
            return View(user);
        }

        // GET: Users/Edit/5
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Users/Delete/5
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

        // POST: Users/Delete/5
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

            return View("Index", branches);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
