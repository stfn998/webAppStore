using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}