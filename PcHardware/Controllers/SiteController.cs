using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Services;
using PcHardware.Models;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "admin")]
    public class SiteController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly MyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public SiteController(IWebHostEnvironment webHostEnvironment, MyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public ActionResult Settings()
        {
            return View(dbContext.Site.FirstOrDefault(s => s.Id == 2));
        }

        [HttpPost]
        public async Task<ActionResult> Settings(Models.Site siteSettings, IFormFile LogoUrl)
        {
            var user = await userManager.GetUserAsync(User);
            if (LogoUrl != null) {
                var wwroot = webHostEnvironment.WebRootPath + "/Site";
                var guid = Guid.NewGuid();
                var fullpath = System.IO.Path.Combine(wwroot, guid + LogoUrl.FileName);

                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    LogoUrl.CopyTo(stream);
                }

                siteSettings.LogoUrl = guid + LogoUrl.FileName;
            }

            dbContext.Site.Update(siteSettings);
            dbContext.SaveChanges();

            var activity = new Activity { 
                Type = "Site settings has been updated",
                Time = DateTime.Now,
                UserId = user.Id
            };
            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();
            return RedirectToAction("Settings");
        }
    }
}
