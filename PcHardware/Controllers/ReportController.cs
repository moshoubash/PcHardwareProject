using Microsoft.AspNetCore.Mvc;

namespace PcHardware.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult GeneratePdf()
        {
            return View();
        }
    }
}
