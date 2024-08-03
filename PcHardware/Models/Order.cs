using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PcHardware.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }

        public int AddressId { get; set; }
        public Address? Address { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
    }
}
