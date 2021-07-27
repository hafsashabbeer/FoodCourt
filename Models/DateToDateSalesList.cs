using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodCourt.Models
{
    public class DateToDateSalesList
    {
        public int BillId { get; set; }
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public int Amount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }
        public DateTime Date { get; set; }
        public string EmpName { get; set; }

    }
}