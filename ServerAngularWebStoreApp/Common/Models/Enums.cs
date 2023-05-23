using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Enums
    {
        public enum OrderStatus
        {
            Accept,
            OnHold,
            Done
        }

        public enum PersonType
        {
            Admin,
            Seller,
            Customer,
            None,
        }

        public enum VerificationStatus
        {
            Active,
            Denied,
            OnHold,
            None
        }
    }
}