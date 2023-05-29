using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public int SellerId { get; set; }

        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string Name { get; set; }

        [Range(0.0, float.MaxValue, ErrorMessage = "Price should be in range of 0.0 to max value")]
        public float Price { get; set; }

        [StringLength(2000, ErrorMessage = "Description length can't be more than 2000.")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Range(0, 20000, ErrorMessage = "Quantity should be in range of 0 to max 20000")]
        public int Quantity { get; set; }

        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}