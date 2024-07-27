using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PcHardware.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [MinLength(1), MaxLength(5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
