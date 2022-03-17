using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager, SUser")]
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public EmailController(IEmailSender emailSender, ApplicationDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var emails = _context.Emails.ToListAsync();
            return View(emails);
        }

        //GET-SendEmailToAll
        public IActionResult SendEmailToAll()
        {
            return View();
        }
    }
}
