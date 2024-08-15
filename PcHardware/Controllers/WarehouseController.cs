using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PcHardware.Models;
using PcHardware.Repositories.Warehouse;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller, admin")]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository warehouseRepository;
        public WarehouseController(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;
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
            return View();
        }

        [HttpPost]
        public ActionResult Create(Warehouse warehouse)
        {
            warehouseRepository.CreateWarehouse(warehouse);
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(int Id)
        {
            warehouseRepository.DeleteWarehouse(Id);
            return RedirectToAction("Manage");
        }

        public ActionResult Edit(int Id)
        {
            return View(warehouseRepository.GetWarehouse(Id));
        }

        public ActionResult Edit(Warehouse warehouse)
        {
            warehouseRepository.EditWarehouse(warehouse);
            return RedirectToAction("Manage");
        }
    }
}
