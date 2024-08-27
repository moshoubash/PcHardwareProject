using Microsoft.AspNetCore.Mvc;
using PcHardware.Repositories.Activity;

namespace PcHardware.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityRepository activityRepository;
        public ActivityController(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }
        public ActionResult Manage()
        {
            return View(activityRepository.GetActivities());
        }

        public ActionResult Delete(int Id)
        {
            activityRepository.DeleteActivity(Id);
            return RedirectToAction("Manage");
        }
    }
}
