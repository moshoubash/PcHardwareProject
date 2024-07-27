using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller")]
    public class SellerController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
