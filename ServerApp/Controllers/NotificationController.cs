﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Models;

namespace ServerApp.Controllers {

    [Produces("application/json")]
    [Route("api/Notifications")]
    [Authorize]
    public class NotificationsController : Controller {
        private readonly WishContext _context;
        private readonly UserManager<User> _userManager;

        public NotificationsController(WishContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            return _context.Notification.Where(n => n.UserId == user.Id);
        }

        // PUT: api/Notifications
        [HttpPut]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<Notification> notifications = _context.Notification.Where(n => n.UserId == user.Id && n.IsUnread);
            notifications.ToList().ForEach(n => n.MarkAsRead());

            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/Notifications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> MarkNotificationAsRead([FromRoute] int id) {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Notification notification = _context.Notification.SingleOrDefault(n => n.UserId == user.Id && n.NotificationId == id);

            if (notification == null)
                return NotFound();

            notification.MarkAsRead();

            await _context.SaveChangesAsync();
            return Ok(notification);
        }

        // GET: api/Notifications/Deadlines
        [HttpGet("Deadlines")]
        public async Task<IEnumerable<Notification>> CheckForDeadlines() {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<Notification> notifications = _context.Notification.Where(n => n.UserId == user.Id && n.Type == NotificationType.DeadlineReminder);

            // TODO: Improve notification constructor
            user.SubscribedLists.ToList().ForEach(l => {
                if (l.List.IsSoon() && !notifications.Any(n => n.ListId == l.ListId))
                    user.Notifications.Add(new Notification() { Type = NotificationType.DeadlineReminder, ListId = l.ListId });
            });

            await _context.SaveChangesAsync();
            return await GetNotifications();
        }

    }
}