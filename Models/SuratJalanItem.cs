using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace latihan2.Models
{
    public class SuratJalanItem
    {
        [Key]
        public Guid SuratJalanItemId { get; set; }

        public Guid SuratJalanId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid SalesOrderItemId { get; set; }



        public int NomorBaris { get; set; }



        public string KodeBarang { get; set; }
        public string NamaBarang { get; set; }
        public int ItemQtyOrder { get; set; }
        public int ItemQtyDelivered { get; set; }
        [NotMapped]
        public int ItemQtyBatas { get; set; }
        public int HargaJual { get; set; }

        public SuratJalan SuratJalan { get; set; }
    }
}