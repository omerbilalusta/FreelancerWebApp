using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreelancerWebApp.Data;
using FreelancerWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FreelancerWebApp.Controllers
{
    public class jobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public jobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: jobs
        public async Task<IActionResult> Index()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //Console.WriteLine(userName);
            return View(await _context.job.ToListAsync());
        }


        // GET: jobs/ShowSearchFrom
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        // Post: jobs/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.job.Where(j=>j.Job_Title.Contains(SearchPhrase)).ToListAsync()); //filtering 
        }

        // GET: jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.job == null)
            {
                return NotFound();
            }

            var job = await _context.job
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: jobs/Create
        [Authorize]
        public IActionResult Create()
        {
            //ViewData("UserId");
            return View();
        }

        // POST: jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Job_Title,Job_Category,Job_Description,Offered_Price,Day,Owner_ID,Freelancer_ID")] job job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }

        // GET: jobs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.job == null)
            {
                return NotFound();
            }

            var job = await _context.job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        // POST: jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Job_Title,Job_Category,Job_Description,Offered_Price,Day,Owner_ID,Freelancer_ID")] job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!jobExists(job.Id))
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
            return View(job);
        }

        // GET: jobs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.job == null)
            {
                return NotFound();
            }

            var job = await _context.job
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: jobs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.job == null)
            {
                return Problem("Entity set 'ApplicationDbContext.job'  is null.");
            }
            var job = await _context.job.FindAsync(id);
            if (job != null)
            {
                _context.job.Remove(job);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool jobExists(int id)
        {
          return _context.job.Any(e => e.Id == id);
        }
    }
}
