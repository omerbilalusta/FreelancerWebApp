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
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var inbox = await _context.inbox.ToListAsync();
            //var asd = _context.inbox.Count();
            //ViewBag.userCount = asd;
            //ViewBag.user = new SelectList(inbox, "Id", "last_message");
            //ViewBag.userr = new SelectList(inbox, "Id", "last_sent_user_id");
            //ViewBag.userrr = new SelectList(inbox, "Id","Id");
            var userEmail = User.Identity.Name;
            var user = _context.user.FirstOrDefault(x => x.user_email == userEmail);
            var inboxes = _context.inbox.OrderBy(x => x.Id).Where(x=>x.last_user_sent_id == user.Id || x.last_receiver_user_id == user.Id).Select(x => new InboxViewModel()
            {
                
                Id = x.Id,
                last_message = x.last_message,
                last_message_sender_name = x.user.user_email

            }).ToList();

            ViewBag.inboxesListViewModel = new InboxListViewModel()
            {
                Inboxes = inboxes
            };

            return View(await _context.inbox.ToListAsync());
        }
        
        [Authorize]
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
        
        [Authorize]
        public async Task<IActionResult> asd(int? id)
        {
            
            return View(await _context.message.Where(c => c.inboxId == id).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> _messagePartial(int? id)
        {
            ViewBag.user = User.Identity.Name;
            ViewBag.Date = DateTime.Now;
            ViewBag.inboxId = id;

            var dbEntry = _context.inbox.FirstOrDefault(acc => acc.Id == id);
            ViewBag.userId = dbEntry.last_user_sent_id;

            var user = _context.user.FirstOrDefault(acc => acc.user_email == User.Identity.Name);
            //ViewBag.username = user.user_email;

            var messages = _context.message.OrderBy(x => x.Id).Where(c => c.inboxId == id).Select(x => new MessagePartialViewModel()
            {
                Id = x.Id,
                message_text = x.message_text,
                date_created = x.date_created,
                userId = x.userId,
                userEmail = x.user.user_email
                
            }).ToList();

            ViewBag.messageListPartialViewModell = new MessageListPartialViewModel()
            {
                Messages = messages
            };

            //var FindUser = _context.user.Where(x => x.Id == dbEntry.);

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> _messagePartial(message newMessage)
        {
            newMessage.userEmail = User.Identity.Name;
            var inbox = _context.inbox.FirstOrDefault(x => x.Id == newMessage.inboxId);
            inbox.last_message = newMessage.message_text;

            var user = _context.user.FirstOrDefault(x => x.user_email == User.Identity.Name);

            var findUser = _context.user.FirstOrDefault(x => x.user_email == User.Identity.Name);
            newMessage.userId = user.Id;
            newMessage.userEmail = user.user_email;
            if (inbox.last_user_sent_id != user.Id)
            {
                var last_user_sent = inbox.last_user_sent_id;
                inbox.last_user_sent_id = user.Id;
                inbox.last_receiver_user_id = last_user_sent;
            }

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

        public IActionResult createChat()
        {

            //ViewBag.userList = new SelectList(_context.user.OrderBy(x => x.Id).Select(x => new List<UserViewModel>()
            //{
            //    new(){Id=x.Id, user_email=x.user_email }
            //}), "Id", "user_email");

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> createChat([Bind("last_message")] inbox inbox, message? newMessage)
        {
           

            var name = HttpContext.Request.Form["name"];
            var message = HttpContext.Request.Form["message"];
            inbox.last_message = message;
            var user = _context.user.FirstOrDefault(x => x.user_email == User.Identity.Name);
            inbox.last_user_sent_id = user.Id;

            newMessage.message_text = message;
            newMessage.date_created = DateTime.Now;
            newMessage.userEmail = User.Identity.Name;
            newMessage.inboxId = inbox.Id;
            newMessage.userId = user.Id;
            var receiverUserEmail = HttpContext.Request.Form["name"].ToString();
            var receiverUser = _context.user.FirstOrDefault(x => x.user_email == receiverUserEmail);
            if (receiverUser == null)
            {
                TempData["status"] = "User could not found, Please Try Again";
                return RedirectToAction(nameof(createChat));
            }
            inbox.last_receiver_user_id= receiverUser.Id;

            var inboxes = _context.inbox.FirstOrDefault(x => x.last_user_sent_id == inbox.last_user_sent_id && x.last_receiver_user_id == inbox.last_receiver_user_id);
            if (inboxes != null)
            {
                TempData["alert"]="This chat already exist";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Add(inbox);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!inboxExists(inbox.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            newMessage.inboxId = inbox.Id;
            try
            {
                _context.Add(newMessage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!messageExists(newMessage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool messageExists(int id)
        {
            return _context.message.Any(e => e.Id == id);
        }
        private bool inboxExists(int id)
        {
            return _context.inbox.Any(e => e.Id == id);
        }
    }

}
