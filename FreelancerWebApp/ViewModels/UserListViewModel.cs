namespace FreelancerWebApp.ViewModels
{
    public class UserListViewModel
    {
        public List<UserListViewModel> Users { get; set; }
    }
    public class UserViewModel
    {
        public int Id { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }

    }
}