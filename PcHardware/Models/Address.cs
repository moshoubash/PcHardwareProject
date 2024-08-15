using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PcHardware.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int PostalCode { get; set; }
        public string? State { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public Order? Order { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}
