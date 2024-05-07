﻿using System;
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
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const double FixMargin = 500;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicle

        .Include(v => v.Operation)  
        .Include(v => v.Repairs)     
        .ToListAsync();
            var operations = await _context.Operation.Include(v => v.Vehicle).ToListAsync();
            var vehicleViewModels = new List<VehicleViewModel>();
            foreach(var vehicle in vehicles)
            {
                vehicleViewModels.Add(new VehicleViewModel()
                {
                    Id = vehicle.Id,
                    Brand = vehicle.Brand,
                    Finish = vehicle.Finish,
                    Model = vehicle.Model,
                    Operation = vehicle.Operation,
                    Vin = vehicle.Vin,
                    Year = vehicle.Year,
                    RetailPrice=CalulateRetailPrice(vehicle.Operation, vehicle.Repairs.ToList()),
                    IsAvailable=vehicle.Operation.SaleDate!=null,

                });                
            }
            return View(vehicleViewModels);
        }

        private double CalulateRetailPrice(Operation operation, List <Repair >repairs)
        {
            double price = 0;
            foreach(Repair repair in repairs)
            {
                price += repair.Cost;
            }
            price += operation.PurchasePrice;
            price += FixMargin;
            return price;        
        }

        private bool ToggleAccessibility(Operation operation)
        {
            if(operation.SaleDate != null) return false;
            return !operation.IsAvailable;

        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include (v => v.Operation)
                .Include(v => v.Repairs)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (vehicle == null)
            {
                return NotFound();
            }
            //var vehicleViewModel = new VehicleViewModel
            //{
            //    Id = vehicle.Id,
            //    Brand = vehicle.Brand,
            //    Finish = vehicle.Finish,
            //    Model = vehicle.Model,          
            //    Vin = vehicle.Vin,
            //    Year = vehicle.Year,               
            //    Operation= vehicle.Operation,
            //};

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OperationId,Vin,Brand,Model,Finish,Year, PurchasePrice, PurchaseDate")] VehicleOperationViewModel vehicleOperationViewModel)
        {
            if (ModelState.IsValid)
            {
                var operation= new Operation{
                    VehicleId=vehicleOperationViewModel.Id,
                    PurchaseDate=vehicleOperationViewModel.PurchaseDate,
                    PurchasePrice=vehicleOperationViewModel.PurchasePrice,
                };
                var vehicle = new Vehicle{
                Id = vehicleOperationViewModel.Id,
                Brand=vehicleOperationViewModel.Brand,
                Finish=vehicleOperationViewModel.Finish,
                Model = vehicleOperationViewModel.Model,
                Operation = operation,
                Vin=vehicleOperationViewModel.Vin,
                Year=vehicleOperationViewModel.Year,
                };                 
            _context.Add(vehicle);
            _context.Add(operation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }

            return View(vehicleOperationViewModel);
        }



        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           // var vehicle = await _context.Vehicle.FindAsync(id);


            var vehicle = await _context.Vehicle
             .Include(v => v.Operation)
             .Include(v => v.Repairs)
             .FirstOrDefaultAsync(m => m.Id == id)
             ;



            if (vehicle == null)
            {
                return NotFound();
            }

            //Create viewModel
            var vehicleOperationViewModel = new VehicleViewModel
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Finish = vehicle.Finish,
                Model = vehicle.Model,
                Vin = vehicle.Vin,
                Year = vehicle.Year,
                Operation=vehicle.Operation,                
            };



            return View(vehicleOperationViewModel);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vin,Brand,Model,Finish,Year, Operation")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var targetVehicle = await _context.Vehicle
            .Include(v => v.Operation)
            .Include(v => v.Repairs)
            .FirstOrDefaultAsync(m => m.Id == id);

                targetVehicle.Vin= vehicle.Vin;
                targetVehicle.Year= vehicle.Year;
                targetVehicle.Operation= vehicle.Operation;
                targetVehicle.Brand= vehicle.Brand;
                targetVehicle.Finish= vehicle.Finish;   
                targetVehicle.Model= vehicle.Model;

            


                try
                {
                    _context.Update(targetVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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


            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}
