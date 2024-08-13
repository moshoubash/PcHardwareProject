using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PcHardware.Models;
using PcHardware.Repositories.Address;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        private readonly MyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public AddressController(IAddressRepository _addressRepository, MyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this._addressRepository = _addressRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        
        [Authorize(Roles = "admin, seller")]
        public ActionResult Manage()
        {
            return View(_addressRepository.GetAddresses());
        }

        [Authorize(Roles = "client")]
        public async Task<ActionResult> AddressBook()
        {
            var user = await userManager.GetUserAsync(User);
            return View(_addressRepository.GetUserAddresses(user.Id));
        }

        [Authorize(Roles ="client")]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Countries = new SelectList(dbContext.Countries, "Id", "Name");
            return View();
        }

        [Authorize(Roles ="client")]
        [HttpPost]
        public async Task<ActionResult> Create(Address address)
        {
            var user = await userManager.GetUserAsync(User);
            address.UserId = user.Id;
            _addressRepository.AddAddress(address);
            return RedirectToAction("AddressBook");
        }

        [Authorize(Roles = "admin, seller")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.Countries = new SelectList(dbContext.Countries, "Id", "Name");
            return View(_addressRepository.GetAddressById(Id));
        }

        [Authorize(Roles = "admin, seller")]
        [HttpPost]
        public ActionResult Edit(Address address)
        {
            _addressRepository.EditAddress(address);
            return RedirectToAction("Manage");
        }

        [Authorize]
        public ActionResult Delete(int Id)
        {
            _addressRepository.RemoveAddress(Id);
            return Redirect("/Index");
        }
    }
}
