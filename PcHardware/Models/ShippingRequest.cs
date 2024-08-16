using System.ComponentModel.DataAnnotations;

namespace PcHardware.Models
{
    public class ShippingRequest
    {
        public string? ServiceType { get; set; }
        public string? PackagingType { get; set; }
        public string? ShipperName { get; set; }
        public string? ShipperAddress { get; set; }
        public string? ShipperCity { get; set; }
        public string? ShipperState { get; set; }
        public string? ShipperPostalCode { get; set; }
        public string? ShipperCountryCode { get; set; }
        
        public string? RecipientName { get; set; }
        public string? RecipientAddress { get; set; }
        public string? RecipientCity { get; set; }
        public string? RecipientState { get; set; }
        public string? RecipientPostalCode { get; set; }
        public string? RecipientCountryCode { get; set; }
        public decimal PackageWeight { get; set; }
    }
}
