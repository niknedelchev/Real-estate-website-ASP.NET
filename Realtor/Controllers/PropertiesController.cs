using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Realtor.Data;
using Realtor.Models;

namespace Realtor.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;
   
        public PropertiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Properties1
        public async Task<IActionResult> Index(string cityID, string propType, string transctType)
        {

            ViewData["CitiesList"] = _context.City.ToList();
            if (!String.IsNullOrEmpty(cityID) && !String.IsNullOrEmpty(propType) && !String.IsNullOrEmpty(transctType))
            {
                PropertyType propTypeChoice = (PropertyType)Enum.Parse(typeof(PropertyType), propType);
                Transaction transctTypeChoice = (Transaction)Enum.Parse(typeof(Transaction), transctType);

                var applicationDbContext = _context.Property.Include(p => p.city).Include(p => p.image)
                .Where(p => p.city.id == int.Parse(cityID) && p.propertyType == propTypeChoice && p.transaction == transctTypeChoice); 
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                return View();
            }
            
        }

        // GET: Properties1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Property
                .Include(p => p.city)
                .Include(p=>p.image)
                .FirstOrDefaultAsync(m => m.propertyId == id);
            if (@property == null)
            {
                return NotFound();
            }
            
            return View(@property);
        }
        
      
        // GET: Properties1/Create
        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            ViewData["cityID"] = new SelectList(_context.City, "id", "id");
            ViewData["User"] = User.Identity.Name;
            ViewData["CitiesList"] = _context.City.ToList();
          
            return View();
        }

        // POST: Properties1/Create
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("propertyId,cityID,propertyType,transaction,header,description,phoneNumber,userID,price")] Property @property)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@property);
                await _context.SaveChangesAsync();
            
                TempData["PropertyID"] = property.propertyId;
                TempData["imgCount"] = 0;

                return RedirectToAction("Create", "Images");
            }
            ViewData["cityID"] = new SelectList(_context.City, "id", "id", @property.cityID);
            return View(@property);
        }

        // GET: Properties1/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["User"] = User.Identity.Name;
            
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Property
                .Include(p=>p.city)  
                .Include(p => p.image)
                .FirstOrDefaultAsync(m => m.propertyId == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties1/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @property = await _context.Property
                  .Include(p => p.city) 
                  .Include(p => p.image)
                  .FirstOrDefaultAsync(m => m.propertyId == id);


            List<Image> tempImages = property.image.ToList();

            _context.Property.Remove(@property);
            await _context.SaveChangesAsync();

            DeleteFile(tempImages);

            return RedirectToAction(nameof(Index));
        }

        private bool DeleteFile(List<Image> images)
        {
             
            try
            {
                if (images.Any())
                {
                    foreach(var img in images)
                    {
                        if (System.IO.File.Exists(img.path))
                        {
                            System.IO.File.Delete(img.path);
                           
                        }
                    }
                    return true;
                }
            }
            catch (Exception e)
            { }
            return false;
        }


        private bool PropertyExists(int id)
        {
            return _context.Property.Any(e => e.propertyId == id);
        }
    }
}
