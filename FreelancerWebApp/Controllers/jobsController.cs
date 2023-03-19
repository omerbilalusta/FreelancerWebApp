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
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using AutoMapper;
using FreelancerWebApp.ViewModels;
using Microsoft.Extensions.FileProviders;
using System.Web;


namespace FreelancerWebApp.Controllers
{
    public class jobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;
        public static string filepaths = "asd";
        private readonly IMapper _mapper;
        private readonly IFileProvider _fileProvider;

        public jobsController(ApplicationDbContext context, IHostingEnvironment env, IMapper mapper, IFileProvider fileProvider)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
            _fileProvider = fileProvider;
        }

        // GET: jobs
        public IActionResult Index()
        {
            var Job = _context.job.ToList();

            return View(_mapper.Map<List<JobViewModel>>(Job));
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
            var jobb = await _context.job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewBag.userid = User.Identity.Name;

            var JobMapped = _mapper.Map<JobViewModel>(jobb);


            return View(JobMapped);
        }

        // GET: jobs/Create
        [Authorize]
        public IActionResult Create()
        {
            //ViewData("UserId");
            ViewBag.userid = User.Identity.Name;
            ViewBag.datenow = DateTime.UtcNow;
            ViewBag.ColorSet = new SelectList(new List<categorySelectList>()
            {
                new(){Data="Graphic Design", Value="GraphicDesign"},
                new(){Data="Text", Value="Text"},
                new(){Data="Software", Value="Software"}
            }, "Value", "Data");
            return View();
        }

        // POST: jobs/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobAddViewModel newJob)
        {
           

            if (ModelState.IsValid)
            {
                var root = _fileProvider.GetDirectoryContents("wwwroot");
                var UploadedFile = root.First(x => x.Name == "JobPhotos");
                var randomFileName = Guid.NewGuid() + Path.GetExtension(newJob.JobImage.FileName);
                var path = Path.Combine(UploadedFile.PhysicalPath, randomFileName);
                using var stream = new FileStream(path, FileMode.Create);

                newJob.JobImage.CopyTo(stream);

                var Job = _mapper.Map<job>(newJob);

                Job.Job_Photo_Path = randomFileName;

                Job.Confirmed = false;
                _context.Add(Job);
                await _context.SaveChangesAsync();
                TempData["status"] = "Job published successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(newJob);
        }

        // GET: jobs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var existJob = _context.job.Find(id);

            ViewBag.ColorSet = new SelectList(new List<categorySelectList>()
            {
                new(){Data="Graphic Design", Value="GraphicDesign"},
                new(){Data="Text", Value="Text"},
                new(){Data="Software", Value="Software"}
            }, "Value", "Data",existJob.Job_Category);



            if (id == null || _context.job == null)
            {
                return NotFound();
            }

            var job = await _context.job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewBag.userid = User.Identity.Name;
            return View(job);
        }

        // POST: jobs/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Job_Title,Job_Category,Job_Description,Offered_Price,Day,Owner_ID,Freelancer_ID,Deliver_File_Path")] job job)
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
                    TempData["status"] = "Job updated successfully";
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

        // GET: jobs/Deliver/5
        [Authorize]
        public async Task<IActionResult> Deliver(int? id)
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

            return View(_mapper.Map<JobViewModel>(job));
        }


        //public IActionResult UploadDone(int id, IFormFile fileUploadinput, [Bind("Id,Job_Title,Job_Category,Job_Description,Offered_Price,Day,Owner_ID,Freelancer_ID,Deliver_File_Path")] job job)
        // POST: jobs/Deliver/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadDone(int id, JobViewModel job)
        {
            var root = _fileProvider.GetDirectoryContents("wwwroot");
            var UploadedFile = root.First(x => x.Name == "UploadedFiles");
            var randomFileName = Guid.NewGuid() + Path.GetExtension(job.File.FileName);
            var path = Path.Combine(UploadedFile.PhysicalPath, randomFileName);
            using var stream = new FileStream(path, FileMode.Create);

            job.File.CopyTo(stream);

            var JobDone = _mapper.Map<job>(job);
            JobDone.Deliver_File_Path = randomFileName;

            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(JobDone);
                    _context.SaveChanges();
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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }



        // GET: jobs/getJob/5
        [Authorize]
        public async Task<IActionResult> getJob(int? id)
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
            ViewBag.userid = User.Identity.Name;
            return View(job);
        }

        // POST: jobs/getJob/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> getJob(int id, [Bind("Id,Job_Title,Job_Category,Job_Description,Offered_Price,Day,Owner_ID,Freelancer_ID,Deliver_File_Path")] job job)
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
            ViewBag.userid = User.Identity.Name;
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

        [Authorize]
        public async Task<IActionResult> Delivery(int ?id)
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
            ViewBag.userid = User.Identity.Name;

            return View(job);
        }

        //[HttpPost]
        //public ActionResult DownloadFile(string filePath)
        //{
        //    // Dosya yolunu doğrulayın ve dosya mevcut olduğundan emin olun
        //    if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
        //    {
        //        return HttpNotFound();
        //    }

        //    // HttpResponse nesnesini oluşturun ve öznitelikleri ayarlayın
        //    var response = new HttpResponse();
        //    response.Clear();
        //    response.ContentType = "application/octet-stream";
        //    response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
        //    response.WriteFile(filePath);
        //    response.End();

        //    return new EmptyResult();
        //}



        private bool jobExists(int id)
        {
          return _context.job.Any(e => e.Id == id);
        }
    }
}
