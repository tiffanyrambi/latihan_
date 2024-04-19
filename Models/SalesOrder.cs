using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace latihan2.Models
{
    public class SalesOrder
    {
        public Guid SalesOrderId { get; set; }

        public string NomorSalesOrder { get; set; }

        public DateTime TanggalSalesOrder { get; set; }

        public List<SalesOrderItem> SalesOrderItems { get; set; }

        //public bool IsCompleted {get;set;}
    }
}