﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHardware.Models
{
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }

        public int AddressId { get; set; }
        public Address? Address { get; set; }

        public string? WarehouseManagerName { get; set; }

        public List<Inventory>? Inventories { get; set; }
    }
}
