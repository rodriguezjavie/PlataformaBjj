using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Data;
using PlataformaBjj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        [BindProperty]
        public NewsItem NewsItem { get; set; }


        public NewsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            NewsItem = new NewsItem();
        }

        public async Task<IActionResult> Index()
        {
            var news = await _context.NewsItems.OrderByDescending(n=>n.UploadDate).ToListAsync(); 
            return View(news);
        }

        //POST-Create
        [Authorize(Roles = "Manager, SUser")]
        public IActionResult Create()
        {
            return View(NewsItem);
        }
        [Authorize(Roles = "Manager, SUser")]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {

            if (!ModelState.IsValid)
            {
                return View(NewsItem);
            }
            NewsItem.UploadDate = DateTime.Now;
            _context.NewsItems.Add(NewsItem);
            await _context.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var newsItemFromDb = await _context.NewsItems.FindAsync(NewsItem.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, NewsItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                newsItemFromDb.Image = @"\images\" + NewsItem.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + "default.jpg");
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + NewsItem.Id + ".png");
                newsItemFromDb.Image = @"\images\" + NewsItem.Id + ".png";
            }

            await _context.SaveChangesAsync();
           
            return RedirectToAction(nameof(Index));
        }

        //GET-DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItem = await _context.NewsItems.SingleOrDefaultAsync(m => m.Id == id);

            if (NewsItem == null)
            {
                return NotFound();
            }

            return View(NewsItem);
        }
        //GET - EDIT
        [Authorize(Roles = "Manager, SUser")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItem = await _context.NewsItems.SingleOrDefaultAsync(m => m.Id == id);

            if (NewsItem == null)
            {
                return NotFound();
            }
            return View(NewsItem);
        }


        [Authorize(Roles = "Manager, SUser")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(NewsItem);
            }

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var newsItemFromDb = await _context.NewsItems.FindAsync(NewsItem.Id);

            if (files.Count > 0)
            {
                //New Image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagePath = Path.Combine(webRootPath, NewsItem.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //we will upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, NewsItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                newsItemFromDb.Image = @"\images\" + NewsItem.Id + extension_new;
            }

            newsItemFromDb.Title = NewsItem.Title;
            newsItemFromDb.Description = NewsItem.Description;
            //newsItemFromDb.IsActive = NewsItem.IsActive;
            

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET : Delete 
        [Authorize(Roles = "Manager, SUser")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItem = await _context.NewsItems.SingleOrDefaultAsync(m => m.Id == id);

            if (NewsItem == null)
            {
                return NotFound();
            }

            return View(NewsItem);
        }

        //POST Delete 
        [Authorize(Roles = "Manager, SUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            NewsItem newsItem = await _context.NewsItems.FindAsync(id);

            if (newsItem != null)
            {
                var imagePath = Path.Combine(webRootPath, newsItem.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _context.NewsItems.Remove(newsItem);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

    }
}
