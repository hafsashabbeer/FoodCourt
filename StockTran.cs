//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoodCourt
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class StockTran
    {
        public int TransId { get; set; }
        public int ItemNo { get; set; }
        public int StockQuantity { get; set; }
        public int StockAmount { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Item Item { get; set; }
    }
}
