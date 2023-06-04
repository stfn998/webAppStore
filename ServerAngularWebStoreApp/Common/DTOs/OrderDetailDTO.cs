﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class OrderDetailDTO
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
    }
}