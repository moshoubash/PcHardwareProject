﻿using PcHardware.Models;
namespace PcHardware.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public List<Product>? Products { get; set; }
    }
}
