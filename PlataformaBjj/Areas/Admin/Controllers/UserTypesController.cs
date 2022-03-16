using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Data;
using PlataformaBjj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Areas.Admin.Controllers
{
    [Authorize(Roles = "Manager, SUser")]
    [Area("Admin")]
    public class UserTypesController : Controller
    {
       
     
            private readonly ApplicationDbContext _context;

            public UserTypesController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: Admin/Categories
            public async Task<IActionResult> Index()
            {
                return View(await _context.UserTypes.ToListAsync());
            }

            // GET: Admin/Categories/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var userType = await _context.UserTypes
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (userType == null)
                {
                    return NotFound();
                }

                return View(userType);
            }

            // GET: Admin/Categories/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Admin/Categories/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create( UserType userType)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(userType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(userType);
            }

            // GET: Admin/Categories/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var userType = await _context.UserTypes.FindAsync(id);
                if (userType == null)
                {
                    return NotFound();
                }
                return View(userType);
            }

            // POST: Admin/Categories/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, UserType userType)
            {
                if (id != userType.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(userType);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserTypeExist(userType.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(userType);
            }

            // GET: Admin/Categories/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var userType = await _context.UserTypes
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (userType == null)
                {
                    return NotFound();
                }

                return View(userType);
            }

            // POST: Admin/Categories/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var userType = await _context.UserTypes.FindAsync(id);
                _context.UserTypes.Remove(userType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool UserTypeExist(int id)
            {
                return _context.UserTypes.Any(e => e.Id == id);
            }
        }
}
