using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PcHardware.Models;
using PcHardware.Repositories;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyDbContext dbContext;
        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment, MyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.productRepository = productRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        
        // Manage products
        [Authorize(Roles = "seller")]
        public ActionResult Manage()
        {
            return View(productRepository.GetProducts());
        }

        // Details of product
        public ActionResult Details(int Id)
        {
            return View(productRepository.GetProductById(Id));
        }
        
        // Create products
        [Authorize(Roles = "seller")]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(dbContext.Categories, "Id", "Name");
            ViewBag.Manufactureres = new SelectList(dbContext.Manufactureres, "Id", "Name");
            return View();
        }
        
        [Authorize(Roles = "seller")]
        [HttpPost]
        public async Task<ActionResult> Create(Product product, IFormFile ImageUrl)
        {
            var user = await userManager.GetUserAsync(User);

            if (ImageUrl != null) {
                var wwwroot = webHostEnvironment.WebRootPath + "/ProductsImages";
                var guid = Guid.NewGuid();
                var fullPath = System.IO.Path.Combine(wwwroot, guid + ImageUrl.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }

                product.ImageUrl = guid + ImageUrl.FileName;
            }

            productRepository.CreateProduct(product);

            var activity = new Activity {
                Type = $"New Product {product.Id}",
                Time = DateTime.Now,
                UserId = user.Id
            };
            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }
        
        // Edit products
        [Authorize(Roles = "seller")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.Categories = new SelectList(dbContext.Categories, "Id", "Name");
            ViewBag.Manufactureres = new SelectList(dbContext.Manufactureres, "Id", "Name");
            return View(productRepository.GetProductById(Id));
        }
        
        [Authorize(Roles = "seller")]
        [HttpPost]
        public async Task<ActionResult> Edit(Product product, IFormFile ImageUrl)
        {
            var user = await userManager.GetUserAsync(User);

            if (ImageUrl != null)
            {
                var wwwroot = webHostEnvironment.WebRootPath + "/ProductsImages";
                var guid = Guid.NewGuid();
                var fullPath = System.IO.Path.Combine(wwwroot, guid + ImageUrl.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }

                product.ImageUrl = guid + ImageUrl.FileName;
            }

            productRepository.EditProduct(product);

            var activity = new Activity
            {
                Type = $"Product {product.Id} edited",
                Time = DateTime.Now,
                UserId = user.Id
            };
            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }
        
        // Delete products
        [Authorize(Roles = "seller")]
        public async Task<ActionResult> Delete(int Id)
        {
            var user = await userManager.GetUserAsync(User);
            productRepository.DeleteProduct(Id);

            var activity = new Activity
            {
                Type = $"Product {Id} deleted",
                Time = DateTime.Now,
                UserId = user.Id
            };
            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

    }
}
