using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PcHardware.Models;
using PcHardware.Repositories.Warehouse;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller, admin")]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository warehouseRepository;
        private readonly MyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public WarehouseController(IWarehouseRepository warehouseRepository, UserManager<ApplicationUser> userManager, MyDbContext dbContext)
        {
            this.warehouseRepository = warehouseRepository;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult Manage()
        {
            return View(warehouseRepository.GetWarehouses());
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            return View(warehouseRepository.GetWarehouse(Id));
        }

        public ActionResult Create()
        {
            ViewBag.Countries = new SelectList(dbContext.Countries, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Warehouse warehouse, string Country, int PostalCode, string State, string Street, string City)
        {
            var user = await userManager.GetUserAsync(User);

            var add = new Address
            {
                UserId = user.Id,
                Country = Country,
                PostalCode = PostalCode,
                State = State,
                Street = Street,
                City = City
            };

            dbContext.Addresses.Add(add);
            dbContext.SaveChanges();

            warehouse.AddressId = add.Id;

            warehouseRepository.CreateWarehouse(warehouse);
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(int Id)
        {
            warehouseRepository.DeleteWarehouse(Id);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.Countries = new SelectList(dbContext.Countries, "Id", "Name");
            return View(warehouseRepository.GetWarehouse(Id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Warehouse warehouse, string Country, int PostalCode, string State, string Street, string City)
        {
            var user = await userManager.GetUserAsync(User);

            var add = new Address
            {
                UserId = user.Id,
                Country = Country,
                PostalCode = PostalCode,
                State = State,
                Street = Street,
                City = City
            };

            dbContext.Addresses.Add(add);
            dbContext.SaveChanges();

            warehouse.AddressId = add.Id;

            warehouseRepository.EditWarehouse(warehouse);
            return RedirectToAction("Manage");
        }
    }
}
