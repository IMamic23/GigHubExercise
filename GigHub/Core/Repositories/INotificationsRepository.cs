using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface INotificationsRepository
    {
        IEnumerable<Notification> GetNewNotifications(string userId);
    }
}