using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Data;
using PlataformaBjj.Models;

namespace PlataformaBjj.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PhrasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhrasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer/Phrases
        public async Task<IActionResult> Index()
        {
            return View(await _context.Phrases.ToListAsync());
        }

        // GET: Customer/Phrases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phrases = await _context.Phrases
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phrases == null)
            {
                return NotFound();
            }

            return View(phrases);
        }

        // GET: Customer/Phrases/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Phrases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phrases phrases)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phrases);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phrases);
        }

        // GET: Customer/Phrases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phrases = await _context.Phrases.FindAsync(id);
            if (phrases == null)
            {
                return NotFound();
            }
            return View(phrases);
        }

        // POST: Customer/Phrases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phrases phrases)
        {
            if (id != phrases.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phrases);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhrasesExists(phrases.Id))
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
            return View(phrases);
        }

        // GET: Customer/Phrases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phrases = await _context.Phrases
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phrases == null)
            {
                return NotFound();
            }

            return View(phrases);
        }

        // POST: Customer/Phrases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phrases = await _context.Phrases.FindAsync(id);
            _context.Phrases.Remove(phrases);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhrasesExists(int id)
        {
            return _context.Phrases.Any(e => e.Id == id);
        }
    }
}
