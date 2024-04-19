using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace latihan2.Models
{
    public class Context : DbContext
    {
        public Context() : base("ConnectionDB")
        {

        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<SuratJalan> SuratJalans { get; set; }

        public DbSet<SuratJalanItem> SuratJalanItems { get; set; }

    }
}