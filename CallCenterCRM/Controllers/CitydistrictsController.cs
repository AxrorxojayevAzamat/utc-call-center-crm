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
using static CallCenterCRM.Models.Citydistrict;

namespace CallCenterCRM.Controllers
{
    public class CitydistrictsController : Controller
    {
        private readonly CallcentercrmContext _context;

        public CitydistrictsController(CallcentercrmContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var citydistricts = await _context.Citydistricts.ToListAsync();

            return View(citydistricts);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citydistrict = await _context.Citydistricts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (citydistrict == null)
            {
                return NotFound();
            }

            return View(citydistrict);
        }

        public IActionResult Create()
        {
            Citydistrict citydistrict = new Citydistrict();
            return View(citydistrict);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Citydistrict citydistrict)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citydistrict);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(citydistrict);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citydistrict = await _context.Citydistricts.FindAsync(id);
            
            if (citydistrict == null)
            {
                return NotFound();
            }
            return View(citydistrict);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Citydistrict citydistrict)
        {
            if (id != citydistrict.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citydistrict);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitydistrictExists(citydistrict.Id))
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
            return View(citydistrict);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citydistrict = await _context.Citydistricts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (citydistrict == null)
            {
                return NotFound();
            }

            return View(citydistrict);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var citydistrict = await _context.Citydistricts.FindAsync(id);
            _context.Citydistricts.Remove(citydistrict);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitydistrictExists(int id)
        {
            return _context.Citydistricts.Any(e => e.Id == id);
        }
    }
}
