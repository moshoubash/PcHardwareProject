using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using PcHardware.Models;
using PcHardware.Services;
using PcHardware.Repositories.Category;
using PcHardware.ViewModels;

namespace PcHardware.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;

        public CategoryController(ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager, MyDbContext dbContext)
        {
            this.categoryRepository = categoryRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;
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

        [Authorize(Roles = "admin, seller")]
        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            var user = await userManager.GetUserAsync(User);
            categoryRepository.CreateCategory(category);

            var activity = new Activity { 
                Type = $"New Category {category.Name}",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();
            return RedirectToAction("Manage");

        }

        [Authorize(Roles = "admin, seller")]
        public async Task<ActionResult> Delete(int Id)
        {
            var user = await userManager.GetUserAsync(User);
            categoryRepository.DeleteCategory(Id);

            var activity = new Activity
            {
                Type = $"Delete Category {Id}",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "admin, seller")]
        [HttpGet]
        public ActionResult Manage()
        {
            return View(categoryRepository.GetCategories());
        }
    }
}
