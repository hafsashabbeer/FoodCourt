using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodCourt.Models
{
    public class FinalBill
    {
        public int Number { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
    }
}