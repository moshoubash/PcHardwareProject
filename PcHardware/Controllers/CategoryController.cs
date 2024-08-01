using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Models;
using PcHardware.Repositories.Category;
using PcHardware.ViewModels;

namespace PcHardware.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [Route("category/products/{Id}")]
        public async Task<ActionResult> Products(int Id)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(Id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryViewModel
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                Products =category.Products,
            };

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Create() {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(Category category)
        {
            categoryRepository.CreateCategory(category);
            return RedirectToAction("Manage");

        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int Id)
        {
            categoryRepository.DeleteCategory(Id);
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Manage()
        {
            return View(categoryRepository.GetCategories());
        }
    }
}
