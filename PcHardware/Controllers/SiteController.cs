using Microsoft.AspNetCore.Mvc;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    public class SiteController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly MyDbContext dbContext;
        public SiteController(IWebHostEnvironment webHostEnvironment, MyDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.dbContext = dbContext;
        }

        public ActionResult Settings()
        {
            return View(dbContext.Site.FirstOrDefault(s => s.Id == 2));
        }

        [HttpPost]
        public ActionResult Settings(Models.Site siteSettings, IFormFile LogoUrl)
        {
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

            return RedirectToAction("Settings");
        }
    }
}
