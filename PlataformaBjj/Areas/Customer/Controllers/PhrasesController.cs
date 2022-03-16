using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _hostingEnvironment;


        public PhrasesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostEnvironment;
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
            if (!ModelState.IsValid)
            {
                return View(phrases);
            }
            _context.Add(phrases);
            await _context.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var phraseItemFromDb = await _context.Phrases.FindAsync(phrases.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "imgPhrases");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, phrases.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                phraseItemFromDb.Image = @"\imgPhrases\" + phrases.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + "default.jpg");
                System.IO.File.Copy(uploads, webRootPath + @"\imgPhrases\" + phrases.Id + ".png");
                phraseItemFromDb.Image = @"\imgPhrases\" + phrases.Id + ".png";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int? id, Phrases phrases)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(phrases);
            }

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var phraseItemFromDb = await _context.Phrases.FindAsync(phrases.Id);

            if (files.Count > 0)
            {
                //New Image has been uploaded
                var uploads = Path.Combine(webRootPath, "imgPhrases");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagePath = Path.Combine(webRootPath, phraseItemFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //we will upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, phrases.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                phraseItemFromDb.Image = @"\imgPhrases\" + phrases.Id + extension_new;
            }

            phraseItemFromDb.Author = phrases.Author;
            phraseItemFromDb.Phrase = phrases.Phrase;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
