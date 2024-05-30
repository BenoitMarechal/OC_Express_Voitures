using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OC_Express_Voitures.Data;
using OC_Express_Voitures.Models;

namespace OC_Express_Voitures.Controllers
{

    public class RepairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repairs 
       
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Repair.Include(r => r.Vehicle);

            if (id == null)
            {
                return View(await applicationDbContext.ToListAsync());
            } 
            
            var vehicle = await _context.Vehicle
             .Include(v => v.Operation)
             .Include(v => v.Repairs)
             .Include(v => v.Photo)
             .FirstOrDefaultAsync(m => m.Id == id)
             ;
            ViewData["IsCurrentVehicleSold"] = true;

            if (vehicle.Operation.SaleDate == null)
            {
            ViewData["CurrentVehicleId"] = id;
            ViewData["CurrentVehicleVin"] = vehicle.Vin;
               ViewData["IsCurrentVehicleSold"] = false;
            }


            var result = await applicationDbContext.Where(r => r.VehicleId == id).ToListAsync();

            return View(result);
        }

     

        // GET: Repairs/Create
        [Authorize]
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

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Cost,Date,VehicleId")] Repair repair)
        {
            

            if (ModelState.IsValid)
            {
                var vehicle = await _context.Vehicle.FindAsync(repair.VehicleId);
                repair.Vehicle = vehicle;
                _context.Add(repair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", repair.VehicleId);
            return View(repair);
        }

        // GET: Repairs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repair.FindAsync(id);
            if (repair == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", repair.VehicleId);
            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Cost,Date,VehicleId")] Repair repair)
        {
            if (id != repair.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairExists(repair.Id))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", repair.VehicleId);
            return View(repair);
        }

        // GET: Repairs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repair
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repair.FindAsync(id);
            if (repair != null)
            {
                _context.Repair.Remove(repair);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepairExists(int id)
        {
            return _context.Repair.Any(e => e.Id == id);
        }
    }
}
