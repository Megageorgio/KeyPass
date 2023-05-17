using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeyPass.Data;
using KeyPass.Models;
using System.Diagnostics.Eventing.Reader;

namespace KeyPass.Controllers
{
    public class PassTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PassTypes
        public async Task<IActionResult> Index()
        {
            var passTypes = _context.PassTypes?.Include(x => x.Department);
              return passTypes != null ? 
                          View(await passTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.PassTypes'  is null.");
        }

        // GET: PassTypes/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: PassTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Duration,Description,DepartmentId")] PassType passType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(passType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", passType.DepartmentId);
            return View(passType);
        }

        // GET: PassTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PassTypes == null)
            {
                return NotFound();
            }

            var passType = await _context.PassTypes.FindAsync(id);
            if (passType == null)
            {
                return NotFound();
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", passType.DepartmentId);
            return View(passType);
        }

        // POST: PassTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Duration,Description,DepartmentId")] PassType passType)
        {
            if (id != passType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassTypeExists(passType.Id))
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

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", passType.DepartmentId);
            return View(passType);
        }

        // GET: PassTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PassTypes == null)
            {
                return NotFound();
            }

            var passType = await _context.PassTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passType == null)
            {
                return NotFound();
            }

            return View(passType);
        }

        // POST: PassTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PassTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PassTypes'  is null.");
            }
            var passType = await _context.PassTypes.FindAsync(id);
            if (passType != null)
            {
                _context.PassTypes.Remove(passType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassTypeExists(int id)
        {
          return (_context.PassTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
