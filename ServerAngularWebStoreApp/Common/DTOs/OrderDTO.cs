using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [StringLength(500, ErrorMessage = "Address length can't be more than 500.")]
        public string DeliveryAddress { get; set; }

        [StringLength(500, ErrorMessage = "Comment length can't be more than 500.")]
        public string Comment { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool CanCancel { get; set; }

        [Range(0.0, float.MaxValue, ErrorMessage = "Price should be in range of 0.0 to max value")]
        public float ShippingCost { get; set; }

        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}