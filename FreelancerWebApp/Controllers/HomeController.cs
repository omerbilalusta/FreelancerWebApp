//using FreelancerWebApp.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;

//namespace FreelancerWebApp.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {

//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}



using FreelancerWebApp.Data;
using FreelancerWebApp.Models;
using FreelancerWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FreelancerWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;
        private readonly ApplicationDbContext _contextjob;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IHostingEnvironment env, ApplicationDbContext contextjob)
        {
            _logger = logger;
            _context = context;
            _env = env;
            _contextjob = contextjob;
        }

        public async Task<IActionResult> Index(user? user)
        {
            var asd = User.Identity.Name;
            if(User.Identity.IsAuthenticated == true)
            {
                if(_context.user.FirstOrDefault(x=> x.user_email == asd) == null)
                {
                    user.Id = 0;
                    user.user_email = User.Identity.Name;
                    user.user_name = "asd";
                    _context.user.Add(user);
                    await _context.SaveChangesAsync();
                }
            }
            return View(await _context.job.ToListAsync());
        }

        public async Task<IActionResult> Privacy()
        {

            return View(await _context.job.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userEmail = User.Identity.Name;
            var user = _context.user.FirstOrDefault(x => x.user_email == userEmail);
            ViewBag.userName = user.user_name;
            ViewBag.UserEmail = user.user_email;
            ViewBag.userid = User.Identity.Name;
            return View(await _context.job.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Comment(int? id)
        {
            ViewBag.Date = DateTime.Now;
            ViewBag.jobId = id;
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Comment(Comment Comment)
        {
            var job = _context.job.Find(Comment.JobId);
            var user = _context.user.FirstOrDefault(x => x.user_email == job.Owner_ID);
            Comment.UserId =user.Id;

            Comment.job = job;
            Comment.user = user;
            job.Confirmed = true;

            _context.job.Update(job);
            await _context.SaveChangesAsync();

            _context.Comment.Add(Comment);
            await _context.SaveChangesAsync();

            //if (ModelState.IsValid)
            //{
            //    _context.Comment.Add(Comment);
            //    await _context.SaveChangesAsync();
            //    return View();
            //}
            return RedirectToAction("Profile");
            
        }

        [HttpGet]
        public async Task<IActionResult> DeliveryPost(int? id)
        {
            var job = _context.job.Find(id);
            job.Confirmed = true;
            _context.job.Update(job);
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", "Home");
        }

        //Get
        [Authorize]
        public async Task<IActionResult> Deliver()
        {
            return View(await _context.job.ToListAsync());
        }

        //Post // Kullanılmıyor !!!
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadDone(IFormFile fileUploadinput/*, IFormFile jobNo*/, int id, [Bind("Id,Job_Title,Job_Category,Job_Description,Offered_Price,Day,Owner_ID,Freelancer_ID,Deliver_File_Path")] job job)
        {
            string uniqueFileName = null;
            var dir = _env.ContentRootPath + "\\wwwroot\\UploadedFiles";
            uniqueFileName = Guid.NewGuid().ToString()+ "_" + fileUploadinput.FileName;
            using(var fileStream = new FileStream(Path.Combine(dir,uniqueFileName), FileMode.Create, FileAccess.Write))
            {
                fileUploadinput.CopyTo(fileStream);
            }
            
            //if (jobNo == job.Id)
            //{
                job.Deliver_File_Path = Path.Combine(dir, uniqueFileName);
            //}
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
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
                return RedirectToAction("Profile");
            }

            //_context.Update(job);
            //_dbContext.SaveChanges();
            return RedirectToAction("Profile");
            //return View(job);
        }

        private bool jobExists(int id)
        {
            throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}