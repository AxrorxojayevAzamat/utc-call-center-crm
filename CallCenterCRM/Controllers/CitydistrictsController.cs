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
            return View(await _context.Citydistricts.ToListAsync());
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
            return View();
        }

        // POST: Citydistricts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,RegionId,CreatedDate,UpdatedDate")] Citydistrict citydistrict)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citydistrict);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,RegionId,CreatedDate,UpdatedDate")] Citydistrict citydistrict)
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
                    await _context.SaveChangesAsync();
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
