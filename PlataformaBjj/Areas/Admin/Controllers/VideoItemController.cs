using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Data;
using PlataformaBjj.Models;
using PlataformaBjj.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PlataformaBjj.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class VideoItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public VideoItemVM VideoItemVM { get; set; }

        public VideoItemController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, IEmailSender emailSender)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _emailSender = emailSender;
            VideoItemVM = new VideoItemVM
            {
                Category = _context.Categories,
                VideoItem = new VideoItem() { UploadDate=DateTime.Now}
            };
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 8;
            var videos = _context.VideoItems.Include(v => v.Category).Include(v => v.SubCategory).OrderByDescending(v => v.UploadDate).ToPagedList(pageNumber, pageSize);
            return View(videos);
            //var videoItems = await _context.VideoItems.Include(v => v.Category).Include(v => v.SubCategory).ToListAsync();
            //return View(videoItems);
        }


        [Authorize(Roles = "Manager, SUser")]
        public IActionResult Create()
        {
            return View(VideoItemVM);
        }

        [Authorize(Roles = "Manager, SUser")]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            VideoItemVM.VideoItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(VideoItemVM);
            }
            VideoItemVM.VideoItem.UploadDate = DateTime.Now;
            _context.VideoItems.Add(VideoItemVM.VideoItem);
            await _context.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var videoItemFromDb = await _context.VideoItems.FindAsync(VideoItemVM.VideoItem.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, VideoItemVM.VideoItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                videoItemFromDb.Image = @"\images\" + VideoItemVM.VideoItem.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + "default.jpg");
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + VideoItemVM.VideoItem.Id + ".png");
                videoItemFromDb.Image = @"\images\" + VideoItemVM.VideoItem.Id + ".png";
            }

            await _context.SaveChangesAsync();
            var usersEmails = await _context.ApplicationUsers.Select(u => u.Email).ToListAsync();
            if (VideoItemVM.EmailNotification)
            {
                foreach (var email in usersEmails)
                {
                    var user = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Email == email);
                    await _emailSender.SendEmailAsync(email, $"Nuevo Video publicado{VideoItemVM.VideoItem.UploadDate}",
                        $"Hola{user.Name}, se ha publicado un nuevo video en el portal de Legion BJJ, esperemos te guste");
                }
            }
           

            return RedirectToAction(nameof(Index));
        }

        //GET - EDIT
        [Authorize(Roles = "Manager, SUser")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VideoItemVM.VideoItem = await _context.VideoItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            VideoItemVM.SubCategory = await _context.SubCategories.Where(s => s.CategoryId == VideoItemVM.VideoItem.CategoryId).ToListAsync();

            if (VideoItemVM.VideoItem == null)
            {
                return NotFound();
            }
            return View(VideoItemVM);
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
            VideoItemVM.VideoItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                VideoItemVM.SubCategory = await _context.SubCategories.Where(s => s.CategoryId == VideoItemVM.VideoItem.CategoryId).ToListAsync();
                return View(VideoItemVM);
            }

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var videoItemFromDb = await _context.VideoItems.FindAsync(VideoItemVM.VideoItem.Id);

            if (files.Count > 0)
            {
                //New Image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagePath = Path.Combine(webRootPath, videoItemFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //we will upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, VideoItemVM.VideoItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                videoItemFromDb.Image = @"\images\" + VideoItemVM.VideoItem.Id + extension_new;
            }

            videoItemFromDb.Title = VideoItemVM.VideoItem.Title;
            videoItemFromDb.Description = VideoItemVM.VideoItem.Description;
            videoItemFromDb.URL = VideoItemVM.VideoItem.URL;
            videoItemFromDb.CategoryId = VideoItemVM.VideoItem.CategoryId;
            videoItemFromDb.SubCategoryId = VideoItemVM.VideoItem.SubCategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //GET : Details MenuItem
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VideoItemVM.VideoItem = await _context.VideoItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (VideoItemVM.VideoItem == null)
            {
                return NotFound();
            }

            return View(VideoItemVM);
        }

        //GET : Delete MenuItem
        [Authorize(Roles = "Manager, SUser")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VideoItemVM.VideoItem = await _context.VideoItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (VideoItemVM.VideoItem == null)
            {
                return NotFound();
            }

            return View(VideoItemVM);
        }

        //POST Delete MenuItem
        [Authorize(Roles = "Manager, SUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            VideoItem menuItem = await _context.VideoItems.FindAsync(id);

            if (menuItem != null)
            {
                var imagePath = Path.Combine(webRootPath, menuItem.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _context.VideoItems.Remove(menuItem);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
