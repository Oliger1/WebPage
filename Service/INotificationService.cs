using System.Collections.Generic;
using WebSiteDocuments.Data;
using WebSiteDocuments.Models;

namespace WebSiteDocuments.Service
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetNotifications();
        void AddNotification(Notification notification);
        void MarkAsRead(int notificationId);
    }
}
