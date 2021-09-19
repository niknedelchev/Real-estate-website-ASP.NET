using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Realtor.Data;
using Realtor.Models;

namespace Realtor.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _iwebhost;

        public ImagesController(ApplicationDbContext context, IWebHostEnvironment iwebhost)
        {
            _context = context;
            _iwebhost = iwebhost;
        }

    
        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Image
                .FirstOrDefaultAsync(m => m.id == id);
            if (image == null)
            {
                return NotFound();
            }


            int tempImgCount = (int)TempData["imgCount"];
            if (tempImgCount > 0)
            {
                TempData["imgCount"] = --tempImgCount;
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            var result = _context.Image.ToList();
            return View(result);
        }

        // POST: Images/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile ifile, Image img, string propID)
        {
            string imgext = Path.GetExtension(ifile.FileName);
            if (imgext == ".jpg" || imgext == ".png")
            {
                var uniqueName = string.Format(@"{0}"+imgext, DateTime.Now.Ticks);
                var saveimg = Path.Combine(_iwebhost.WebRootPath, "images", uniqueName);
                var stream = new FileStream(saveimg, FileMode.Create);
                await ifile.CopyToAsync(stream);
                stream.Close();

                img.name = uniqueName; 
                img.path = saveimg;
                img.propertyId =  int.Parse(propID) ;
                await _context.Image.AddAsync(img);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Saved.";
            }
            else
            {
                ViewData["Message"] = "Please upload only .jpg or .png";
            }
            return RedirectToAction("Create");
        }


        private bool DeleteFile(string imgName = "")
        {
            try
            {
                if (imgName != null && imgName.Length > 0)
                {
                    string fullPath = Path.Combine(_iwebhost.WebRootPath, "images", imgName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        return true;
                    }
                }
            }
            catch (Exception e)
            { }
            return false;
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Image
                .FirstOrDefaultAsync(m => m.id == id);
            if (image == null)
            {
                return NotFound();
            }

            int tempImgCount = (int)TempData["imgCount"];
            if (tempImgCount > 0)
            {
                TempData["imgCount"] = --tempImgCount;
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Image.FindAsync(id);
            _context.Image.Remove(image);
            await _context.SaveChangesAsync();
            DeleteFile(image.name);

            int tempImgCount=0;
            try
            {
                tempImgCount = (int)TempData["imgCount"];
                if (tempImgCount > 0)
                {
                    TempData["imgCount"] = --tempImgCount;
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
           

            return RedirectToAction(nameof(Create));
        }


        private bool ImageExists(int id)
        {
            return _context.Image.Any(e => e.id == id);
        }
    }
}
