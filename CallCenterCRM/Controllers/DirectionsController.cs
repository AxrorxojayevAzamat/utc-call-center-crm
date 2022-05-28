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

namespace CallCenterCRM.Controllers
{
    public class DirectionsController : Controller
    {
        private readonly CallcentercrmContext _context;

        public DirectionsController(CallcentercrmContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "CrmAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Directions.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Directions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (direction == null)
            {
                return NotFound();
            }

            return View(direction);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Direction direction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(direction);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(direction);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Directions.FindAsync(id);
            if (direction == null)
            {
                return NotFound();
            }
            return View(direction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Direction direction)
        {
            if (id != direction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direction);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectionExists(direction.Id))
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
            return View(direction);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direction = await _context.Directions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (direction == null)
            {
                return NotFound();
            }

            return View(direction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direction = await _context.Directions.FindAsync(id);
            _context.Directions.Remove(direction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DirectionExists(int id)
        {
            return _context.Directions.Any(e => e.Id == id);
        }
    }
}
