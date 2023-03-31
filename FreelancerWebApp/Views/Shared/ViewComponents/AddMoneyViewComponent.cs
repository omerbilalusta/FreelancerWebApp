using AutoMapper;
using FreelancerWebApp.Data;
using FreelancerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerWebApp.Views.Shared.ViewComponents
{
    public class AddMoneyViewComponent :ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddMoneyViewComponent(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
