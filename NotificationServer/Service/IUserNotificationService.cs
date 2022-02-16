using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationServer.Models;

namespace NotificationServer.Service
{
    public interface IUserNotificationService
    {
        public Task SendAsync(NotificationMessage messaage);
        public void Send(NotificationMessage message);
    }
}
