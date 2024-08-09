using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    public class SearchController : Controller
    {
        private readonly MyDbContext _context;
        public SearchController(MyDbContext _context)
        {
            this._context = _context;
        }
        // GET: /Search/Results
        public IActionResult Results(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(Enumerable.Empty<Product>()); // Replace with your actual model
            }

            var results = _context.Products
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                .ToList();

            return View(results);
        }
    }
}
