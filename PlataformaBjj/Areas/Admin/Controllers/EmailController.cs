using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Data;
using PlataformaBjj.Models;
using PlataformaBjj.Models.ViewModels;
using PlataformaBjj.Services;
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
        private readonly ITemplateSender _templateSender;
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public EmailVM EmailVM{ get; set; }

        public EmailController(IEmailSender emailSender, ApplicationDbContext context, ITemplateSender templateSender)
        {
            _emailSender = emailSender;
            _templateSender = templateSender;
            _context = context;
            EmailVM = new EmailVM
            {
                Email=new Email()
            };

        }

        public async Task<IActionResult> Index()
        {
            var emails = await _context.Emails.ToListAsync();
            return View(emails);
        }

        //GET-CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Email mail)
        {
            if (!ModelState.IsValid)
                return View(mail);
            await _context.Emails.AddAsync(mail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var email = await _context.Emails.SingleOrDefaultAsync(e => e.Id == id);

            if (email == null)
                return NotFound();

            return View(email);
        }

        //GET-Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var email = await _context.Emails.SingleOrDefaultAsync(e => e.Id == id);

            if (email == null)
                return NotFound();

            return View(email);
        }

        //POST-Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
                return NotFound();
            var emailInDb = await _context.Emails.SingleOrDefaultAsync(m => m.Id == id);
            if (emailInDb == null)
                return NotFound();
            _context.Emails.Remove(emailInDb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        //GET-Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var mail = await _context.Emails.SingleOrDefaultAsync(m => m.Id == id);
            if (mail == null)
                return NotFound();
            return View(mail);
        }

        //POST-Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, Email email)
        {
            if (id == null)
                return NotFound();
            var mailInDb = await _context.Emails.SingleOrDefaultAsync(m => m.Id == id);
            mailInDb.TemplateKey = email.TemplateKey;
            mailInDb.EmailSubject = email.EmailSubject;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }



        //GET-SendEmail
        public async Task<IActionResult> SendEmail(int? id)
        {
            if (id == null)
                return View();

            var email = await _context.Emails.SingleOrDefaultAsync(e => e.Id == id);

            if (email == null)
                return NotFound();
            EmailVM.Email = email;

            return View(EmailVM);
            
        }

        //POST-SendEmail
        //GET-SendEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail()
        {
            var allUsers = await _context.ApplicationUsers.ToListAsync();

            var email = EmailVM.Email;
            if (EmailVM.AllUsers)
            {
                foreach(var user in allUsers)
                {
                    await _emailSender.SendEmailAsync(user.Email, EmailVM.Email.EmailSubject ,
                        EmailVM.Email.EmailMessage);
                }
            }
            if (EmailVM.Students)
            {
                var students = await _context.ApplicationUsers.Where(u => u.UserTypeId == 1).ToListAsync();
                foreach (var user in students)
                {
                    await _emailSender.SendEmailAsync(user.Email, EmailVM.Email.EmailSubject,
                        EmailVM.Email.EmailMessage);
                }
            }
            if (EmailVM.Profesors)
            {
                var profesors = await _context.ApplicationUsers.Where(u => u.UserTypeId == 2).ToListAsync();
                foreach (var user in profesors)
                {
                    await _emailSender.SendEmailAsync(user.Email, EmailVM.Email.EmailSubject,
                        EmailVM.Email.EmailMessage);
                }
            }
            else
            {
                await _emailSender.SendEmailAsync(EmailVM.Email.EmailAddress, EmailVM.Email.EmailSubject,
                       EmailVM.Email.EmailMessage);

            }
            return RedirectToAction(nameof(Index));
        }
        //GET-SendTemplate
        public async Task<IActionResult> SendTemplate(int id)
        
        {
            

            var email = await _context.Emails.SingleOrDefaultAsync(e => e.Id == id);

            if (email == null)
                return NotFound();
            EmailVM.Email = email;

            return View(EmailVM);

        

        }

        //POST-SendTemplate
        [HttpPost, ActionName("SendTemplate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendTemplatePost()
        {
            var allUsers = await _context.ApplicationUsers.ToListAsync();

            var email = EmailVM.Email;
            if (EmailVM.AllUsers)
            {
                foreach (var user in allUsers)
                {
                    await _templateSender.SendTemplateAsync(user.Email,EmailVM.Email.TemplateKey);
                }
            }
            if (EmailVM.Students)
            {
                var students = await _context.ApplicationUsers.Where(u => u.UserTypeId == 1).ToListAsync();
                foreach (var user in students)
                {
                    await _templateSender.SendTemplateAsync(user.Email, EmailVM.Email.TemplateKey);
                }
            }
            if (EmailVM.Profesors)
            {
                var profesors = await _context.ApplicationUsers.Where(u => u.UserTypeId == 2).ToListAsync();
                foreach (var user in profesors)
                {
                    await _templateSender.SendTemplateAsync(user.Email, EmailVM.Email.TemplateKey);
                }
            }
            else
            {
                await _templateSender.SendTemplateAsync(EmailVM.Email.EmailAddress, EmailVM.Email.TemplateKey);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
