using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PcHardware.Models;
using PcHardware.Repositories.Metatag;

namespace PcHardware.Controllers
{
    public class MetatagsController : Controller
    {
        private readonly IMetatagRepository metatagRepository;
        public MetatagsController(IMetatagRepository metatagRepository)
        {
            this.metatagRepository = metatagRepository;
        }
        [HttpGet]
        public ActionResult Manage()
        {
            return View(metatagRepository.GetMetaTags());
        }

        public ActionResult Create(MetaTag metaTag)
        {
            metatagRepository.CreateMeta(metaTag);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            return View(metatagRepository.GetMetaTagById(Id));
        }

        [HttpPost]
        public ActionResult Edit(MetaTag metaTag)
        {
            metatagRepository.EditMeta(metaTag);
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(int Id)
        {
            metatagRepository.DeleteMeta(Id);
            return RedirectToAction("Manage");
        }
    }
}
