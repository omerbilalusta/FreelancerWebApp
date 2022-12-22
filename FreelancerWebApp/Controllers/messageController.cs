using FreelancerWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreelancerWebApp.Data;
using FreelancerWebApp.Models;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;
using NuGet.Versioning;
using Microsoft.AspNetCore.Mvc.Rendering;
using FreelancerWebApp.ViewModels;
using System;

namespace FreelancerWebApp.Controllers
{
    public class messageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public messageController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var inbox = await _context.inbox.ToListAsync();
            var asd = _context.inbox.Count();
            ViewBag.userCount = asd;
            ViewBag.user = new SelectList(inbox, "Id", "last_message");
            ViewBag.userr = new SelectList(inbox, "Id", "last_sent_user_id");
            ViewBag.userrr = new SelectList(inbox, "Id","Id");

            return View(await _context.inbox.ToListAsync());
        }

        public async Task<IActionResult> SendMessage()
        {
            //return Json("ok");

            //var message = await _context.message.Join();

            return View(await _context.message.ToListAsync());




            //if (id == null || _context.message == null)
            //{
            //    return NotFound();
            //}

            //var message = await _context.message.FindAsync(id);
            //if (message == null)
            //{
            //    return NotFound();
            //}

            //return View(message);

            //ViewBag.messages = new List<string>() { _context.message.Find(id) };

            //Dictionary<string, DateTime, int, int> messageList = new Dictionary<string, DateTime, int, int>();

            //List<object> messageId = new List<object>();
            //List<object> messagesText = new List<object>();
            //List<object> dateCreated = new List<object>();
            //List<object> inboxId = new List<object>();
            //List<object> userId = new List<object>();
            //var counter = 0;


            //foreach (var item in _context.message.Where(c => c.inbox.Id == id))
            //{
            //    if(item.message_text == null)
            //    {
            //        messagesText.Add("0");
            //    }
            //    else
            //    {
            //        messagesText.Add(item.message_text);
            //    }

            //    messageId.Add(item.Id);
            //    dateCreated.Add(item.date_created);
            //    inboxId.Add(item.user.Id);
            //    userId.Add(item.inbox.Id);

            //    counter = counter + 1;
            //}

            //ViewBag.messageIds = messageId;
            //ViewBag.messagesTexts = messagesText;
            //ViewBag.dateCreateds = dateCreated;
            //ViewBag.inboxIds = inboxId;
            //ViewBag.userIds = userId;
            //ViewBag.counter = counter;

            //return View();
            //return View(await _context.job.ToListAsync());



        }

        public async Task<IActionResult> asd(int? id)
        {
            
            return View(await _context.message.Where(c => c.inboxId == id).ToListAsync());
        }

        public async Task<IActionResult> _messagePartial(int? id)
        {
            ViewBag.user = User.Identity.Name;
            ViewBag.Date = DateTime.Now;
            ViewBag.inboxId = id;


            var dbEntry = _context.inbox.FirstOrDefault(acc => acc.Id == id);
            ViewBag.userId = dbEntry.last_user_sent_id;

            var user = _context.user.FirstOrDefault(acc => acc.Id == dbEntry.last_user_sent_id);
            ViewBag.username = user.user_email;

            var messages = _context.message.OrderBy(x => x.Id).Where(c=> c.inboxId == id).Select(x => new MessagePartialViewModel()
            {
                Id = x.Id,
                message_text = x.message_text,
                date_created = x.date_created,
                userId = x.userId

            }).ToList();

            ViewBag.messageListPartialViewModell = new MessageListPartialViewModel()
            {
                Messages = messages
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _messagePartial(message newMessage)
        {

            var inbox = _context.inbox.FirstOrDefault(x => x.Id == newMessage.inboxId);
            inbox.last_message = newMessage.message_text;

            var user = _context.user.FirstOrDefault(x => x.Id == newMessage.userId);
            //inbox.last_sent_user = user.user_email;
            //inbox.last_user_sent_id = ;
            var test = _context.Users.Where(z=>z.UserName == User.Identity.Name).FirstOrDefault();
            //var Userr = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            //inbox.last_user_sent_id = Convert.ToInt64(Userr.Id);
            user.user_email = test.UserName;
            //inbox.last_user_sent_id = Convert.ToInt32(test.Id);

            try
            {
                _context.Update(inbox);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!messageExists(inbox.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(newMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(_messagePartial));
            }
            return View(newMessage);
        }




        //[Bind("Id,Job_Title,Job_Category,Job_Description,Offered_Price,Day,Owner_ID,Freelancer_ID,Deliver_File_Path")] job job
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> asd(int id, message newMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(asd));
            }
            return View(newMessage);
        }

        public IActionResult messages()
        {

            var messages = _context.message.OrderBy(x=>x.Id).Select(x => new MessagePartialViewModel()
            {
                Id = x.Id,
                message_text = x.message_text,
                date_created = x.date_created,
                userId = x.userId

            }).ToList();

            ViewBag.messageListPartialViewModel = new MessageListPartialViewModel()
            {
                Messages = messages
            };

            //var users = _context.Users.OrderBy(x => x.Id).Select(x => new UserPartialViewModel()
            //{

            //});
            //ViewBag.userEmail = _context.Users.ToList();

            return View();

        }

        private bool messageExists(int id)
        {
            return _context.job.Any(e => e.Id == id);
        }
    }

}
