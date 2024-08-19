using PcHardware.Models;
namespace PcHardware.ViewModels
{
    public class ManufacturerViewModel
    {
        public int ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }

        public List<Product>? Products { get; set; }
    }
}
