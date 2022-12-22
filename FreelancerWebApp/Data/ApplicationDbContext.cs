using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FreelancerWebApp.Models;

namespace FreelancerWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FreelancerWebApp.Models.job> job { get; set; }
        public DbSet<FreelancerWebApp.Models.profile> profile { get; set; }
        public DbSet<FreelancerWebApp.Models.message> message { get; set; }
        public DbSet<FreelancerWebApp.Models.user> user { get; set; }
        public DbSet<FreelancerWebApp.Models.inbox> inbox { get; set; }
        public DbSet<FreelancerWebApp.Models.inbox_participants> inbox_participants { get; set; }
    }
}