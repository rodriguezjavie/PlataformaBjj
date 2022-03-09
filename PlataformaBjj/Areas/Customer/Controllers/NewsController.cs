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
            var news = await _context.NewsItems.ToListAsync(); 
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
    }
}
