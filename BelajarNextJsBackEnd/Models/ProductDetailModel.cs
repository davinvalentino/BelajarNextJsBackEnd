﻿namespace BelajarNextJsBackEnd.Models
{
    public class ProductDetailModel
    {
        public string Id { set; get; } = "";

        public string Name { set; get; } = "";

        public string Description { get; set; } = "";

        public decimal Price { get; set; }

        public int Quantity { set; get; }

        public string BrandId { set; get; } = "";

        public string BrandName { set; get; } = "";
    }
}
