using PcHardware.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHardware.Models
{
    public class Activity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Type { get; set; }
        public DateTime Time { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
