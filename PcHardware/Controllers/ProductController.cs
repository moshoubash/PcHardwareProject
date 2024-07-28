using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PcHardware.Models;
using PcHardware.Repositories;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller")]
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
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(dbContext.Categories, "Id", "Name");
            ViewBag.Manufactureres = new SelectList(dbContext.Manufactureres, "Id", "Name");
            return View();
        }

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
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ViewBag.Categories = new SelectList(dbContext.Categories, "Id", "Name");
            ViewBag.Manufactureres = new SelectList(dbContext.Manufactureres, "Id", "Name");
            return View(productRepository.GetProductById(Id));
        }
        
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
        public ActionResult Delete(int Id)
        {
            productRepository.DeleteProduct(Id);
            return RedirectToAction("Manage");
        }

    }
}
