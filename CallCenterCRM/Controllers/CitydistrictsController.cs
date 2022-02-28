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

        // GET: Citydistricts
        public async Task<IActionResult> Index()
        {
            var citydistricts = await _context.Citydistricts.ToListAsync();

            return View(citydistricts);
        }

        // GET: Citydistricts/Details/5

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

        // GET: Citydistricts/Create
        public IActionResult Create()
        {
            Citydistrict citydistrict = new Citydistrict();
            return View(citydistrict);
        }

        // POST: Citydistricts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Citydistricts/Edit/5
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

        // POST: Citydistricts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Citydistricts/Delete/5
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

        // POST: Citydistricts/Delete/5
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
