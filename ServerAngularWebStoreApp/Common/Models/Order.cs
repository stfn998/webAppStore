using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Common.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } // This will link the order to the customer (Person)
        public string DeliveryAddress { get; set; }
        public string Comment { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool CanCancel { get; set; }
        public float ShippingCost { get; set; }

        // Navigation properties
        public Person Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } // This is to establish the many-to-many relationship between Product and Order

        [NotMapped]
        public float TotalPrice => OrderDetails.Sum(od => od.Price);

        [NotMapped]
        public Dictionary<int, float> SellersShippingCosts
        {
            get
            {
                return OrderDetails
                    .GroupBy(od => od.Product.SellerId)
                    .ToDictionary(
                        group => group.Key,
                        group => group.First().Product.Seller.ShippingCost);
            }
        }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            CanCancel = true; // All orders can initially be cancelled
            DeliveryTime = OrderDate.AddHours(new Random().Next(2, 24)); // Random delivery time greater than 1 hour from the order date
        }
    }
}