﻿namespace FreelancerWebApp.ViewModels
{
    public class InboxListViewModel
    {
        public List<InboxViewModel> Inboxes { get; set; }
    }
    public class InboxViewModel
    {
        public int Id { get; set; }
        public string? last_message { get; set; }
        public string last_message_sender_name { get; set; }
        public string last_message_receiver_name { get; set; }
        public string user2 { get; set; }
        public string user1 { get; set; }
    }
}
