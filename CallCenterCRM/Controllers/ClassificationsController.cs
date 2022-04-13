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

namespace CallCenterCRM.Controllers
{
    public class ClassificationsController : Controller
    {
        private readonly CallcentercrmContext _context;

        public ClassificationsController(CallcentercrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Classifications.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classification = await _context.Classifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classification == null)
            {
                return NotFound();
            }

            return View(classification);
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Classification classification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classification);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(classification);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classification = await _context.Classifications.FindAsync(id);
            if (classification == null)
            {
                return NotFound();
            }
            return View(classification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Classification classification)
        {
            if (id != classification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classification);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassificationExists(classification.Id))
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
            return View(classification);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classification = await _context.Classifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classification == null)
            {
                return NotFound();
            }

            return View(classification);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classification = await _context.Classifications.FindAsync(id);
            _context.Classifications.Remove(classification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassificationExists(int id)
        {
            return _context.Classifications.Any(e => e.Id == id);
        }
    }
}
