using System;
using System.Collections.Generic;
using System.Linq;
using WebSiteDocuments.Data;
using WebSiteDocuments.Models;

namespace WebSiteDocuments.Service
{
    public class NotificationService : INotificationService
    {
        private readonly WebDocumentDb _context;

        public NotificationService(WebDocumentDb context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetNotifications()
        {
            return _context.Notification.ToList();
        }

        public void AddNotification(Notification notification)
        {
            _context.Notification.Add(notification);
            _context.SaveChanges();
        }

        public void MarkAsRead(int notificationId)
        {
            var notification = _context.Notification.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = false;
                _context.SaveChanges();
            }
        }
    }
}
