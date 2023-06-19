using AutoMapper;
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
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IHostingEnvironment env, ApplicationDbContext contextjob, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _env = env;
            _contextjob = contextjob;
            _mapper = mapper;
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
        public async Task<IActionResult> Reviews()
        {
            var list = _context.user.Select(x => new UserListReviewViewModel()
            {
                Id = x.Id,
                user_email = x.user_email,
                user_name = x.user_name
            }).ToList();
            return View(list);
        }
        public async Task<IActionResult> ReviewsOf(int? id)
        {
            var list = _context.Comment.Where(s => s.freelancerUserID == id).Select(x => new CommentListComponentViewModel()
            {
                Comment = x.text,
                userName = x.ownerUser.user_email,
                freelancerName = x.job.Freelancer_ID,
                rate = x.rate,
                jobName = x.job.Job_Title
            }).ToList();
            var freelancerUser = _context.user.Find(id);


            if (freelancerUser == null)
            {
                return Json("There is no user with this name");
            }
            ViewBag.userName = freelancerUser.user_name;
            ViewBag.userEmail = freelancerUser.user_email;
            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userEmail = User.Identity.Name;
            var user = _context.user.FirstOrDefault(x => x.user_email == userEmail);
            ViewBag.userName = user.user_name;
            ViewBag.UserEmail = user.user_email;
            ViewBag.userid = User.Identity.Name;
            ViewBag.Money = user.Money +"$";
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
            var Owner_user = _context.user.FirstOrDefault(x => x.user_email == job.Owner_ID);
            var Freelancer_user = _context.user.FirstOrDefault(x => x.user_email == job.Freelancer_ID);
            Comment.ownerUserId = Owner_user.Id;

            Comment.job = job;
            Comment.ownerUser = Owner_user;
            Comment.freelancerUserID = Freelancer_user.Id;
            job.Confirmed = true;

            var OwnerMoneyFinalCount = Owner_user.Money - job.Offered_Price;
            Owner_user.Money = OwnerMoneyFinalCount;

            var FreelancerMoneyFinalCount = Freelancer_user.Money + job.Offered_Price;
            Freelancer_user.Money = FreelancerMoneyFinalCount;

            _context.user.Update(Freelancer_user);
            _context.SaveChanges();

            _context.user.Update(Owner_user);
            _context.SaveChanges();

            _context.job.Update(job);
            _context.SaveChanges();

            _context.Comment.Add(Comment);
            _context.SaveChanges();

            return RedirectToAction("Profile");
            
        }

        [HttpGet]
        public async Task<IActionResult> DeliveryPost(int? id)
        {
            var job = _context.job.Find(id);
            job.Confirmed = true;

            var Owner_user = _context.user.FirstOrDefault(x=> x.user_email == job.Owner_ID);
            var Freelancer_user = _context.user.FirstOrDefault(x=> x.user_email == job.Freelancer_ID);

            var OwnerUserFinalMoneyCount = Owner_user.Money - job.Offered_Price;
            var FreelancerUserFinalMoneyCount = Freelancer_user.Money + job.Offered_Price;

            Owner_user.Money = OwnerUserFinalMoneyCount;
            Freelancer_user.Money = FreelancerUserFinalMoneyCount;

            _context.user.Update(Owner_user);
            _context.user.Update(Freelancer_user);
            _context.job.Update(job);

            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", "Home");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Revision (int? id)
        {
            var job = _context.job.Find(id);
            job.Deliver_File_Path = "1";
            job.Confirmed = false;
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

        

        private bool jobExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> AddMoney(AddMoneyComponentViewModel addMoneyComponentViewModel)
        {
            var UserEmail=User.Identity.Name;
            try
            {
                var users = _mapper.Map<user>(addMoneyComponentViewModel);
                var user = _context.user.FirstOrDefault(x => x.user_email == UserEmail);
                var FinalMoneyCount = user.Money + addMoneyComponentViewModel.Money;
                user.Money = FinalMoneyCount;
                _context.user.Update(user);
                _context.SaveChanges();
                TempData["result"] = "true";
                return RedirectToAction(nameof(HomeController.Profile));
            }
            catch (Exception)
            {
                TempData["result"] = "true";
                return View(nameof(HomeController.Profile));
            }
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}