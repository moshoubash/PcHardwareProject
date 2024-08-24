using System.ComponentModel.DataAnnotations.Schema;

namespace PcHardware.Models
{
    public class Site
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Owner { get; set; }
        public string? LogoUrl { get; set; }
        public string? DefaultCurrency { get; set;}
    }
}
