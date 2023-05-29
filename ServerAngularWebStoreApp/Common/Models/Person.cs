using System;
using System.Collections.Generic;
using static Common.Models.Enums;

namespace Common.Models
{
    public class Person
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime Birth { get; set; }
        public PersonType PersonType { get; set; }
        public bool DecisionMade { get; set; }
        public VerificationStatus Verification { get; set; }
        public string ImageUrl { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; } // A seller can have many products
        public ICollection<Order> Orders { get; set; } // A customer can have many orders
        public float ShippingCost { get; set; } // New property
    }
}