using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace latihan2.Models
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public double InventoryQty { get; set; } //InventoryQtyInit

        public double BackOrderSalesOrder { get; set; }
        public double InventoryQtyOutgoing { get; set; } //outgoing = keluar
        [NotMapped]
        public double InventoryQtyFinal
        {
            get { return InventoryQty - InventoryQtyOutgoing; }
        }//no setter here

        public double InventoryPrice { get; set; }
        //belum migration
    }
}