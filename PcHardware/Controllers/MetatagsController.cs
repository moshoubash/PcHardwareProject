using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PcHardware.Models;
using PcHardware.Repositories.Metatag;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    public class MetatagsController : Controller
    {
        private readonly IMetatagRepository metatagRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;

        public MetatagsController(IMetatagRepository metatagRepository, UserManager<ApplicationUser> userManager, MyDbContext dbContext)
        {
            this.metatagRepository = metatagRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;

        }
        [HttpGet]
        public ActionResult Manage()
        {
            return View(metatagRepository.GetMetaTags());
        }

        public async Task<ActionResult> Create(MetaTag metaTag)
        {
            var user = await userManager.GetUserAsync(User);

            metatagRepository.CreateMeta(metaTag);
            
            var activity = new Activity
            {
                Type = $"New Metatag added {metaTag.Name}",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            return View(metatagRepository.GetMetaTagById(Id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MetaTag metaTag)
        {
            var user = await userManager.GetUserAsync(User);

            metatagRepository.EditMeta(metaTag);

            var activity = new Activity
            {
                Type = $"Metatag {metaTag.Id} edited",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

        public async Task<ActionResult> Delete(int Id)
        {
            var user = await userManager.GetUserAsync(User);
            
            metatagRepository.DeleteMeta(Id);

            var activity = new Activity
            {
                Type = $"Metatag {Id} deleted",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }
    }
}
