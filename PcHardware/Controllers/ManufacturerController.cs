using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Models;
using PcHardware.Repositories.Category;
using PcHardware.Repositories.Manufacturer;
using PcHardware.Services;
using PcHardware.ViewModels;
using System.Data.Common;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "admin, seller")]
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;
        public ManufacturerController(MyDbContext dbContext, IManufacturerRepository manufacturerRepository, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.manufacturerRepository = manufacturerRepository;
            this.dbContext = dbContext;
        }
        public ActionResult Manage()
        {
            return View(manufacturerRepository.GetManufacturers());
        }

        [HttpPost]
        public async Task<ActionResult> Create(Manufacturer manufacturer)
        {
            manufacturerRepository.CreateManufacturer(manufacturer);

            var user = await userManager.GetUserAsync(User);
            var activity = new Activity { 
                Type = $"New Manufacturer has been added {manufacturer.Id}",
                Time = DateTime.Now,
                UserId = user.Id
            };
            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

        public async Task<ActionResult> Delete(int Id)
        {
            manufacturerRepository.DeleteManufacturer(Id);

            var user = await userManager.GetUserAsync(User);
            var activity = new Activity
            {
                Type = $"Manufacturer {Id} has been deleted",
                Time = DateTime.Now,
                UserId = user.Id
            };
            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

        [Route("manufacturer/products/{Id}")]
        public async Task<ActionResult> Products(int Id)
        {
            var manufacturer = await manufacturerRepository.GetManufacturerProductsAsync(Id);

            if (manufacturer == null)
            {
                return NotFound();
            }

            var model = new ManufacturerViewModel
            {
                ManufacturerId = manufacturer.Id,
                ManufacturerName = manufacturer.Name,
                Products = manufacturer.Products,
            };

            return View(model);
        }
    }
}
