using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    public class ReportController : Controller
    {
        private readonly MyDbContext dbContext;
        private readonly OrderReportService orderReportService;

        public ReportController(MyDbContext dbContext, OrderReportService orderReportService)
        {
            this.dbContext = dbContext;
            this.orderReportService = orderReportService;

        }
        [HttpGet]
        public IActionResult DownloadOrderReport(int id)
        {
            // Retrieve the order from your database using the provided id
            var order = dbContext.Orders.Include(o => o.OrderItems)
                                         .Include(o => o.Address)
                                         .Include(o => o.User)
                                         .Include(o => o.Discount)
                                         .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Generate the PDF report
            var pdfBytes = orderReportService.GenerateOrderReport(order);

            // Return the PDF as a downloadable file
            return File(pdfBytes, "application/pdf", $"Order_{order.Id}_Report.pdf");
        }
    }
}
