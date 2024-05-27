using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OC_Express_Voitures.Data;
using OC_Express_Voitures.Models;
using OC_Express_Voitures.Utils;

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
            var vehicleIndexViewModels = new List<VehicleIndexViewModel>();
            foreach(var vehicle in vehicles)
            {
                vehicleIndexViewModels.Add(new VehicleIndexViewModel()
                {
                    Id = vehicle.Id,
                    Vin = vehicle.Vin,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    Finish = vehicle.Finish,
                    Year = vehicle.Year,
                    RetailPrice = CalulateRetailPrice(vehicle.Operation, vehicle.Repairs.ToList()),
                    IsAvailable = vehicle.Operation.IsAvailable,
                    Status = StatusHelper.ReturnStatus(vehicle.Operation),
                    Photo=vehicle.Photo,
                   

                });                
            }
            return View(vehicleIndexViewModels);
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

       // public Photo AddPhoto(string FilePath)


  

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
                .Include(v => v.Photo)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (vehicle == null)
            {
                return NotFound();
            }
            var vehicleDetailsViewModel = new VehicleDetailsViewModel
            {
                Id = vehicle.Id,
                Vin = vehicle.Vin,
                Brand = vehicle.Brand,
                Model = vehicle.Model,          
                Finish = vehicle.Finish,
                Year = vehicle.Year,               
                RepairsCount=vehicle.Repairs.Count(),
                RetailPrice = CalulateRetailPrice(vehicle.Operation, vehicle.Repairs.ToList()),
                IsAvailable = vehicle.Operation.IsAvailable,
                Description= vehicle.Description,
                Photo= vehicle.Photo,
               
            };

            return View(vehicleDetailsViewModel);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            var currentYear = DateTime.Now.Year;
            var minYear = 1990;
            var allowedYears = new List<int?>();
            allowedYears.Add(null);
            for (int year = currentYear; year >= minYear; year--)
            {
                allowedYears.Add(year);
            }
            ViewData["Years"] = new SelectList(allowedYears);
            return View();
        }
        
        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OperationId,Vin,Brand,Model,Finish,Year, PurchasePrice, PurchaseDate, Description")] VehicleCreateViewModel vehicleCreateViewModel)
        {
            if(vehicleCreateViewModel.Year  < 1990)
            {
                ModelState.AddModelError(nameof(vehicleCreateViewModel.Year), "Cars before 1990 will not be accepted");
            }
            if (ModelState.IsValid)
            {
                var operation= new Operation{
                    VehicleId=vehicleCreateViewModel.Id,
                    PurchaseDate=vehicleCreateViewModel.PurchaseDate,
                    PurchasePrice=vehicleCreateViewModel.PurchasePrice,
                };
                var vehicle = new Vehicle{
                Id = vehicleCreateViewModel.Id,
                Brand=vehicleCreateViewModel.Brand,
                Finish=vehicleCreateViewModel.Finish,
                Model = vehicleCreateViewModel.Model,
                Operation = operation,
                Vin=vehicleCreateViewModel.Vin,
                Year=vehicleCreateViewModel.Year,
                Description=vehicleCreateViewModel.Description,
                };                 
            _context.Add(vehicle);
            _context.Add(operation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }

            return View(vehicleCreateViewModel);
        }



        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }        


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
            var vehicleEditViewModel = new VehicleEditViewModel
            {
                Id = vehicle.Id,
                Vin = vehicle.Vin,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Finish = vehicle.Finish,
                Year = vehicle.Year,
                PurchaseDate=vehicle.Operation.PurchaseDate,
                PurchasePrice=vehicle.Operation.PurchasePrice,
                SaleDate= vehicle.Operation.SaleDate,
                IsAvailable = vehicle.Operation.SaleDate != null ? false : vehicle.Operation.IsAvailable,
                Description = vehicle.Description,
                Photo=vehicle.Photo,

               
            //IsAvailable = vehicle.Operation.SaleDate == null,
            };



            return View(vehicleEditViewModel);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vin,Brand,Model,Finish,Year,PurchaseDate,PurchasePrice, SaleDate, IsAvailable, Description")] VehicleEditViewModel vehicleEditViewModel)
        {
            if (id != vehicleEditViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

               var targetVehicle = await _context.Vehicle
            .Include(v => v.Operation)
            .Include(v => v.Repairs)
            .FirstOrDefaultAsync(m => m.Id == id);
                targetVehicle.Vin= vehicleEditViewModel.Vin;
                targetVehicle.Brand= vehicleEditViewModel.Brand;                
                targetVehicle.Model= vehicleEditViewModel.Model;           
                targetVehicle.Finish= vehicleEditViewModel.Finish;   
                targetVehicle.Year= vehicleEditViewModel.Year;
                targetVehicle.Description= vehicleEditViewModel.Description;
                targetVehicle.Operation.SaleDate=vehicleEditViewModel.SaleDate;
                targetVehicle.Operation.IsAvailable= vehicleEditViewModel.SaleDate != null?false:vehicleEditViewModel.IsAvailable;
                targetVehicle.Operation.PurchaseDate=vehicleEditViewModel.PurchaseDate;
                targetVehicle.Operation.PurchasePrice=vehicleEditViewModel.PurchasePrice;
      
                try
                {
                    _context.Update(targetVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicleEditViewModel.Id))
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


            return View(vehicleEditViewModel);
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
