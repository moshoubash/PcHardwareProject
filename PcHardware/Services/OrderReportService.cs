using PcHardware.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PcHardware.Services
{
    public class OrderReportService
    {
        private readonly MyDbContext dbContext;
        public OrderReportService(MyDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public byte[] GenerateOrderReport(Order order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Header
                    page.Header()
                        .Text($"Order #{order.Id}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                    // Content
                    page.Content().Element(container =>
                    {
                        container.Column(column =>
                        {
                            // Order Information
                            column.Item().PaddingBottom(10).Text($"Order ID: {order.Id}");
                            column.Item().PaddingBottom(10).Text($"Order Date: {order.OrderDate.ToString("f")}");
                            column.Item().PaddingBottom(10).Text($"Total Amount: {order.TotalAmount:C}");
                            column.Item().PaddingBottom(10).Text($"Status: {order.Status}");
                            column.Item().PaddingBottom(10).Text($"User Email: {order.User?.Email}");
                            column.Item().PaddingBottom(10).Text($"Phone Number: {order.User?.PhoneNumber}");
                            column.Item().PaddingBottom(10).Text($"Discount: {order.Discount?.Name} ({order.Discount?.DiscountPercentage}%)");

                            // Shipping Address
                            column.Item().PaddingTop(15).PaddingBottom(5)
                                .Text("Shipping Address:")
                                .SemiBold().FontSize(14).FontColor(Colors.Grey.Darken2);

                            column.Item().PaddingBottom(10).Text($"{order.Address?.Street}, {order.Address?.City}");
                            column.Item().PaddingBottom(10).Text($"{order.Address?.State}, {order.Address?.PostalCode}");

                            // Order Items Table
                            column.Item().PaddingTop(15).PaddingBottom(5)
                                .Text("Order Items:")
                                .SemiBold().FontSize(14).FontColor(Colors.Grey.Darken2);

                            column.Item().Table(table =>
                            {
                                // Define table columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                // Define table header with border
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Product Name").SemiBold();
                                    header.Cell().Element(CellStyle).Text("Quantity").SemiBold();
                                    header.Cell().Element(CellStyle).Text("Unit Price").SemiBold();
                                    header.Cell().Element(CellStyle).Text("Total Price").SemiBold();

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold())
                                                        .Padding(5)
                                                        .BorderBottom(1)
                                                        .BorderColor(Colors.Black);
                                    }
                                });

                                // Define table content with borders
                                foreach (var item in order.OrderItems)
                                {
                                    var productName = dbContext.Products.FirstOrDefault(p => p.Id == item.ProductId).Name;
                                    table.Cell().Element(CellStyle).Text(productName);
                                    table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
                                    table.Cell().Element(CellStyle).Text($"{item.UnitPrice:C}");
                                    table.Cell().Element(CellStyle).Text($"{item.Quantity * item.UnitPrice:C}");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.Padding(5)
                                                        .BorderBottom(1)
                                                        .BorderColor(Colors.Grey.Lighten2);
                                    }
                                }
                            });
                        });
                    });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            });

            // Generate and return the PDF document
            return document.GeneratePdf();
        }
    }
}
