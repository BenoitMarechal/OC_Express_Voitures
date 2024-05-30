using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OC_Express_Voitures.Data;
using OC_Express_Voitures.Migrations;
using OC_Express_Voitures.Models;
using OC_Express_Voitures.Utils;

namespace OC_Express_Voitures.Controllers
{

        [Authorize]
    public class OperationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OperationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Operations
        
        public async Task<IActionResult> Index()
        {
            // var applicationDbContext = _context.Operation.Include(o => o.Vehicle);

            var operations = await _context.Operation.Include(o => o.Vehicle).Include(o => o.Vehicle.Repairs).ToListAsync();

            foreach (var operation in operations)
            {
                var repairs = operation.Vehicle.Repairs.ToList();
                operation.SellingPrice = RetailPriceCalculator.CalculateRetailPrice(operation, repairs);
            }




            // return View(await applicationDbContext.ToListAsync());
            return View(operations);
        }

        // GET: Operations/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operation
                .Include(o => o.Vehicle)
                .Include(o => o.Vehicle.Repairs)
                .FirstOrDefaultAsync(m => m.Id == id);

            var repairs = operation.Vehicle.Repairs.ToList();
            if (operation == null)
            {
                return NotFound();
            }
            operation.SellingPrice=RetailPriceCalculator.CalculateRetailPrice(operation, repairs);


            return View(operation);
        }

      

        // GET: Operations/Edit/5
        
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operation
                .Include(o => o.Vehicle)
                .Include(o => o.Vehicle.Repairs)
                .FirstOrDefaultAsync(m => m.Id == id);


            // var vehicle = operation.Vehicle;
            var repairs = operation.Vehicle.Repairs.ToList();


            if (operation == null)
            {
                return NotFound();
            }

            var operationEditViewModel = new OperationEditViewModel
            {
                SellingPrice = RetailPriceCalculator.CalculateRetailPrice(operation, repairs),
                Id = operation.Id,
                IsAvailable = operation.IsAvailable,
                PurchaseDate = operation.PurchaseDate,
                PurchasePrice = operation.PurchasePrice,
                SaleDate = operation.SaleDate,
                VehicleId = operation.VehicleId,

            };

            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", operationEditViewModel.VehicleId);


            return View(operationEditViewModel);
        }

        // POST: Operations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,PurchasePrice,IsAvailable,SellingPrice,PurchaseDate,SaleDate")] OperationEditViewModel operationEditViewModel)
        {
            if (id != operationEditViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (id == null)
                    {
                        return NotFound();
                    }

                    var operation = await _context.Operation
                        .Include(o => o.Vehicle)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (operation == null)
                    {
                        return NotFound();
                    }
                    var vehicle = operation.Vehicle;
                    operation.PurchasePrice = operationEditViewModel.PurchasePrice;
                    operation.SaleDate = operationEditViewModel.SaleDate;
                    // operation.SellingPrice = operationEditViewModel.SellingPrice;
                    operation.PurchaseDate = operationEditViewModel.PurchaseDate;
                    operation.PurchasePrice = operationEditViewModel.PurchasePrice;
                    operation.VehicleId = vehicle.Id;
                    operation.Vehicle = vehicle;
                    operation.Id = id;
                    operation.VehicleId = vehicle.Id;
                    operation.IsAvailable = operationEditViewModel.SaleDate != null ? false : operationEditViewModel.IsAvailable;

                    _context.Update(operation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationExists(operationEditViewModel.Id))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", operationEditViewModel.VehicleId);
            return View(operationEditViewModel);
        }

        // GET: Operations/Delete/5
  
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operation
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // POST: Operations/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //  var operation = await _context.Operation.FindAsync(id);
            var operation = await _context.Operation
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (operation != null)
            {
                var vehicle = operation.Vehicle;
                _context.Operation.Remove(operation);
                _context.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationExists(int id)
        {
            return _context.Operation.Any(e => e.Id == id);
        }
    }
}
