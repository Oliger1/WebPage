using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebSiteDocuments.Data;
using WebSiteDocuments.Models;


namespace WebSiteDocuments.Controllers
{
    public class CommunityController : Controller
    {
        private readonly WebDocumentDb _context;
        private readonly  IHubContext<CommunityHub> _hubContext;
      

        public CommunityController(WebDocumentDb context, IHubContext<CommunityHub> hubContext )
        {
            _context = context;
            _hubContext = hubContext;
         
        }
        public IActionResult CreateUser(string message)
        {
            var lastUser = _context.Community.OrderByDescending(c => c.Id).FirstOrDefault();

            var newUserName = "User" + 1;

            var newUser = new Community
            {
                UserName = newUserName,
                Message = message,
                TimeStamp = DateTime.Now,
            };

            _context.Community.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
           var userName = HttpContext.User.Identity.Name;

            var newMessage = new Community
            {
                UserName = userName,
                Message = message,
                TimeStamp = DateTime.Now
            };

            _context.Community.Add(newMessage);
            _context.SaveChanges();

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", userName, message);

            return Json(newMessage);
        }

    }
}


