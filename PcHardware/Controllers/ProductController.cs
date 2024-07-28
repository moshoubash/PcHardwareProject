using Microsoft.AspNetCore.Authorization;
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
        
        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment, MyDbContext dbContext)
        {
            this.productRepository = productRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.dbContext = dbContext;
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
        public ActionResult Create(Product product, IFormFile ImageUrl)
        {
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
        public ActionResult Edit(Product product, IFormFile ImageUrl)
        {
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
            return RedirectToAction("Manage");
        }
        
        // Delete products
        [Authorize(Roles = "seller")]
        public ActionResult Delete(int Id)
        {
            productRepository.DeleteProduct(Id);
            return RedirectToAction("Manage");
        }

    }
}
