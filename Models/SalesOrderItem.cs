using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace latihan2.Models
{
    public class SalesOrderItem
    {

        [Key]
        public Guid SalesOrderItemId { get; set; }

        public Guid SalesOrderId { get; set; }


        public int NomorBaris { get; set; }




        public string KodeBarang { get; set; }
        public string NamaBarang { get; set; }
        public int ItemQty { get; set; }
        public int ItemQtyDelivered { get; set; }
        public int HargaJual { get; set; }

        //public bool IsCompleted {get;set;}

        public SalesOrder SalesOrder { get; set; }
    }
}