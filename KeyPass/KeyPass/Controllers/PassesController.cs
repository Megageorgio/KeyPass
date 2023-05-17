using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeyPass.Data;
using KeyPass.Models;

namespace KeyPass.Controllers
{
    public class PassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Passes
        public async Task<IActionResult> Index()
        {
            var passes = _context.Passes?.Include(x => x.User).Include(x => x.PassType);
              return passes != null ? 
                          View(await passes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Passes'  is null.");
        }

        // GET: Passes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["PassTypeId"] = new SelectList(_context.PassTypes, "Id", "Name");
            return View();
        }

        // POST: Passes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,PassTypeId")] Pass pass)
        {
            pass.IssueDate = DateTime.Now.Date;
            if (ModelState.IsValid)
            {
                var passType = _context.PassTypes.Find(pass.PassTypeId);
                if (passType.Duration != 0)
                {
                    pass.ExpirationDate = pass.IssueDate.AddDays(passType.Duration);
                }
                _context.Add(pass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", pass.UserId);
            ViewData["PassTypeId"] = new SelectList(_context.PassTypes, "Id", "Name", pass.PassTypeId);
            return View(pass);
        }

        // GET: Passes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Passes == null)
            {
                return NotFound();
            }

            var pass = await _context.Passes.FindAsync(id);
            if (pass == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", pass.UserId);
            ViewData["PassTypeId"] = new SelectList(_context.PassTypes, "Id", "Name", pass.PassTypeId);
            return View(pass);
        }

        // POST: Passes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IssueDate,ExpirationDate,UserId,PassTypeId")] Pass pass)
        {
            if (id != pass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassExists(pass.Id))
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

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", pass.UserId);
            ViewData["PassTypeId"] = new SelectList(_context.PassTypes, "Id", "Name", pass.PassTypeId);
            return View(pass);
        }

        // GET: Passes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Passes == null)
            {
                return NotFound();
            }

            var pass = await _context.Passes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pass == null)
            {
                return NotFound();
            }

            return View(pass);
        }

        // POST: Passes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Passes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Passes'  is null.");
            }
            var pass = await _context.Passes.FindAsync(id);
            if (pass != null)
            {
                _context.Passes.Remove(pass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassExists(int id)
        {
          return (_context.Passes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
