using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OC_Express_Voitures.Data;
using OC_Express_Voitures.Models;

namespace OC_Express_Voitures.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(PhotoImportViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ImageFile.CopyToAsync(memoryStream);







                    var photo = new Photo
                    {
                        VehicleId = model.VehicleId,
                        ImageFileName = model.ImageFile.FileName,
                        ImageData = memoryStream.ToArray()
                    };

                    //HERE
                    var vehicle = await _context.Vehicle.Include(v => v.Photo).FirstOrDefaultAsync(v => v.Id == photo.VehicleId);
                    if (vehicle == null)
                    {
                        // Handle the case where the vehicle is not found
                        return NotFound();
                    }
                    if (vehicle.Photo != null)
                    {
                        // Remove the existing photo
                        _context.Photo.Remove(vehicle.Photo);
                        await _context.SaveChangesAsync();
                    }

                    //



                    _context.Photo.Add(photo);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "Vehicles", new { id = model.VehicleId });
                }
            }

            return View(model);
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Photo.Include(p => p.Vehicle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .Include(p => p.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }
            return File(photo.ImageData, "image/jpeg");
            // return View(photo);
        }

        // GET: Photos/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                //Populate the select menu with available VehicleId's'
                ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Vin");
                return View();
            }
            // Disable the select menu and preselect the correct VehicleId
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound($"Vehicle with ID {id} not found.");
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Vin", id);
            ViewData["Disabled"] = "disabled";
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleId,ImageData,ImageFileName")] Photo photo)
        {
            if (ModelState.IsValid)
            {               
                var vehicle = await _context.Vehicle.Include(v => v.Photo).FirstOrDefaultAsync(v => v.Id == photo.VehicleId);
                if (vehicle == null)
                {
                    // Handle the case where the vehicle is not found
                    return NotFound();
                }
                if (vehicle.Photo != null)
                {
                    // Remove the existing photo
                    _context.Photo.Remove(vehicle.Photo);
                    await _context.SaveChangesAsync();
                }

                // Add the new photo
                vehicle.Photo = photo; // Associate the new photo with the vehicle
                _context.Photo.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", photo.VehicleId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,ImageData,ImageFileName")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", photo.VehicleId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .Include(p => p.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            string mimeType = "image/jpeg"; // default to jpeg
            if (photo.ImageFileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                mimeType = "image/png";
            }

            return File(photo.ImageData, mimeType);
        }




        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photo.FindAsync(id);
            if (photo != null)
            {
                _context.Photo.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.Id == id);
        }
    }
}
