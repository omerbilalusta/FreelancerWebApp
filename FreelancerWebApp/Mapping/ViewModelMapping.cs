using AutoMapper;
using FreelancerWebApp.Models;
using FreelancerWebApp.ViewModels;

namespace FreelancerWebApp.Mapping
{
    public class ViewModelMapping:Profile
    {
        public ViewModelMapping()
        {
            CreateMap<job, JobViewModel>().ReverseMap();
            CreateMap<job, JobAddViewModel>().ReverseMap();
            CreateMap<user, AddMoneyComponentViewModel>().ReverseMap();
        }

    }
}
