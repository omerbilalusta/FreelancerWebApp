namespace FreelancerWebApp.ViewModels
{
    public class MessageListPartialViewModel
    {
        public List<MessagePartialViewModel> Messages { get; set; }
    }
    public class MessagePartialViewModel
    {
        public int Id { get; set; }
        public string? message_text { get; set; }
        public DateTime date_created { get; set; }
        public int? userId { get; set; }
        public string userEmail { get; set; }
    }
}
