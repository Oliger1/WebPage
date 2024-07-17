using Microsoft.AspNetCore.Mvc;
using WebSiteDocuments.Service;

namespace WebSiteDocuments.Controllers
{
    public class NotificationController : Controller

    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
