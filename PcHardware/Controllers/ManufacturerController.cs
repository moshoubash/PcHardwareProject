using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Models;
using PcHardware.Repositories.Manufacturer;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "admin, seller")]
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerRepository manufacturerRepository;
        public ManufacturerController(IManufacturerRepository manufacturerRepository)
        {
            this.manufacturerRepository = manufacturerRepository;
        }
        public ActionResult Manage()
        {
            return View(manufacturerRepository.GetManufacturers());
        }

        [HttpPost]
        public ActionResult Create(Manufacturer manufacturer)
        {
            manufacturerRepository.CreateManufacturer(manufacturer);
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(int Id)
        {
            manufacturerRepository.DeleteManufacturer(Id);
            return RedirectToAction("Manage");
        }
    }
}
