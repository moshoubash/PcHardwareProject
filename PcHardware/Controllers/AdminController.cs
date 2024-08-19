using Microsoft.AspNetCore.Mvc;

namespace PcHardware.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
