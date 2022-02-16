using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationServer.Models;
using NotificationServer.Service;

namespace NotificationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        IUserNotificationService _notificationService;
        public NotificationsController(IUserNotificationService notificationService)
            => _notificationService = notificationService;
        [HttpPost]
        public IActionResult NewNotification([FromBody] NotificationMessage message)
        {
            _notificationService.Send(message);
            return Ok();
        }
    }
}
