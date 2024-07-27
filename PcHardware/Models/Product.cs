using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHardware.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }

        public List<CartItem>? CartItems { get; set; }
        public List<Inventory>? Inventories { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<Wishlist>? Wishlists { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public List<ProductSpecification>? ProductSpecifications { get; set; }
    }
}
