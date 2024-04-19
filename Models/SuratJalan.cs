using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace latihan2.Models
{
    public class SuratJalan
    {
        [Key]
        public Guid SuratJalanId { get; set; }

        public Guid SalesOrderId { get; set; }
        public string NomorSalesOrder { get; set; }

        public DateTime TanggalSuratJalan { get; set; }

        public string NamaSuratJalan { get; set; }

        public string NamaCustomer { get; set; }
        public string AlamatCustomer { get; set; }

        //public SalesOrder SalesOrder { get; set; }


        public List<SuratJalanItem> SuratJalanItems { get; set; }
    }
}