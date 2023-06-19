using AutoMapper;
using FreelancerWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using FreelancerWebApp.ViewModels;
using System;
using System.Linq;

namespace FreelancerWebApp.Views.Shared.ViewComponents
{
    public class CommentViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentViewComponent(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = _context.Comment.Select(x => new CommentListComponentViewModel()
            {
                Comment = x.text,
                userName = x.ownerUser.user_email,
                freelancerName = x.job.Freelancer_ID
            }).ToList();
            //ViewBag.jobId = jobId;
            //ViewBag.Date=DateTime.Now;
            return View(list);
        }
    }
}
