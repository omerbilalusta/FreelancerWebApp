using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreelancerWebApp.Data;
using FreelancerWebApp.Models;

namespace FreelancerWebApp.Controllers
{
    public class profilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public profilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: profiles
        public async Task<IActionResult> Index()
        {
              return View(await _context.profile.ToListAsync());
        }

        // GET: profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.profile == null)
            {
                return NotFound();
            }

            var profile = await _context.profile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // GET: profiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email")] profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }

        // GET: profiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.profile == null)
            {
                return NotFound();
            }

            var profile = await _context.profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // POST: profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email")] profile profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!profileExists(profile.Id))
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
            return View(profile);
        }

        // GET: profiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.profile == null)
            {
                return NotFound();
            }

            var profile = await _context.profile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.profile == null)
            {
                return Problem("Entity set 'ApplicationDbContext.profile'  is null.");
            }
            var profile = await _context.profile.FindAsync(id);
            if (profile != null)
            {
                _context.profile.Remove(profile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool profileExists(int id)
        {
          return _context.profile.Any(e => e.Id == id);
        }
    }
}
