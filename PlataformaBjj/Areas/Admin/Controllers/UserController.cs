using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Data;
using PlataformaBjj.Models;
using PlataformaBjj.Models.ViewModels;
using PlataformaBjj.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlataformaBjj.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Manager, SUser")]
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var users = await _context.ApplicationUsers.Where(u => u.Id != claim.Value).Include(u=>u.UserType).ToListAsync();
            return View(users);
        }
        [Authorize]
        public async Task<IActionResult> Details()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Id == claim.Value);
            return View(user);

        }
        [Authorize(Roles ="Manager, SUser")]
        public async Task<IActionResult> EditOtherUser(string email)
        {
            
            var user = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Email == email);
            var model = new UserVM
            {
                ApplicationUser = user,
                UserTypesList = await _context.UserTypes.ToListAsync()
            };
            return View("Edit",model);

        }
        //GET-EDIT
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Id == claim.Value);
            return View(user);

        }

        //POST-EDIT
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserVM user)
        {
            if (user == null)
                return NotFound();
            var userInDb = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Id == user.ApplicationUser.Id);
            if (userInDb == null)
                return NotFound();
            userInDb.Name = user.ApplicationUser.Name;
            userInDb.LastName = user.ApplicationUser.LastName;
            userInDb.PhoneNumber = user.ApplicationUser.PhoneNumber;
            userInDb.UserTypeId = user.ApplicationUser.UserTypeId;
            await _context.SaveChangesAsync();
            if (User.IsInRole(SD.CustomerUser))
                return RedirectToAction(nameof(Details));
           
            return RedirectToAction(nameof(Index));
        }


        //GET-DELETE
        [Authorize(Roles = "Manager, SUser")]
        public async Task<IActionResult> Delete(string email)
        {
            if (email == null)
                return NotFound();
            var user = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound();
            return View(user);
        }

        [Authorize(Roles = "Manager, SUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string email)
        {
            var user = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Email == email);
            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "Manager, SUser")]
        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now.AddYears(1000);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Manager, SUser")]
        public async Task<IActionResult> UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
