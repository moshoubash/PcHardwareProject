using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Models;
using PcHardware.Repositories;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        
        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        // Manage products
        public ActionResult Manage()
        {
            return View(productRepository.GetProducts());
        }

        // Create products

        public ActionResult Create()
        {
            return View();
        }

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

        public ActionResult Edit()
        {
            return View();
        }

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
