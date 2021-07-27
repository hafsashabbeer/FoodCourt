using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodCourt.Models
{
    public class BillValues
    {
        public DateTime BDate { get; set; }
        public int ItemNo { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
    
    }
}