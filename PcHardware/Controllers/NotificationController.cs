using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "admin, seller")]
    public class NotificationController : Controller
    {
        private readonly MyDbContext dbContext;
        public NotificationController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Manage()
        {
            return View(dbContext.Notifications.OrderByDescending(n => n.Time).ToList());
        }

        public ActionResult Delete(int Id)
        {
            var targetNotify = dbContext.Notifications.FirstOrDefault(n => n.Id == Id);
            dbContext.Notifications.Remove(targetNotify);
            return RedirectToAction("Manage");
        }

        public ActionResult ToggleIsRead(int Id) {
            var notify = dbContext.Notifications.FirstOrDefault(n => n.Id == Id);
            if (notify.IsRead == false) { 
                notify.IsRead = true;
                dbContext.SaveChanges();
                return RedirectToAction("Manage");
            }
            else { 
                return RedirectToAction("Manage");
            }
        }
    }
}
