using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latihan2.Models;

namespace latihan2.Controllers
{
    public class SalesController : ApiController
    {
        private Context db;

        public SalesController()
        {
            db = new Context();
        }

        [HttpGet] // all
        [Route("api/SalesOrder")]
        public IHttpActionResult GetSales()
        {
            var sales = db.SalesOrders.OrderBy(x => x.NomorSalesOrder).ToList();
            return Ok(sales);
        }

        [HttpGet] // filtered by IsCompleted, used for Modal in SuratJalan
        [Route("api/SalesOrder/ForModal")]
        public IHttpActionResult GetSalesFiltered()
        {
            List<SalesOrder> salesOrders = db.SalesOrders
                .Include(x => x.SalesOrderItems)
                .OrderBy(x => x.NomorSalesOrder)
                .ToList();

            salesOrders = salesOrders.Where(x => x.SalesOrderItems.Any(y => y.ItemQtyDelivered < y.ItemQty)).ToList();

            //List<SalesOrder> filteredSales = new List<SalesOrder>(); //empty

            //for (var i = 0; i < allSales.Count; i++)
            //{
            //    for (var j = 0; j < allSales[i].SalesOrderItems.Count; j++)
            //    {
            //        //kalau ada salah satu item yang "belum terpenuhi", berarti masih bisa diklik, dan masih mau dimunculkan di Modal!
            //        if (allSales[i].SalesOrderItems[j].ItemQtyDelivered < allSales[i].SalesOrderItems[j].ItemQty)
            //        {

            //            //tambahkan SO ke filteredSales,...
            //            filteredSales.Add(allSales[i]);
            //            //...lalu skip ke SO berikutnya.
            //            break;
            //        }
            //    }
            //}

            return Ok(salesOrders);
        }

        [HttpGet]  //one specific id
        [Route("api/SalesOrder")]
        public IHttpActionResult Get(Guid id)
        {
            //include untuk mendapatkan salesorderitem bersamaan dengan query
            //tanpa include = salesorderitems jadi null
            var dataSale = db.SalesOrders.Include(x => x.SalesOrderItems).SingleOrDefault(x => x.SalesOrderId == id);
            //mengurutkan salesorderitems (bisa diurutkan dengan query karena bentuknya list...?)
            dataSale.SalesOrderItems = dataSale.SalesOrderItems.OrderBy(x => x.NomorBaris).ToList();

            return Ok(dataSale);
        }

        [HttpPost] //save
        [Route("api/SalesOrder")]
        public IHttpActionResult PostSale([FromBody] SalesOrder salesOrder)
        {

            var message = verifyTransaction(salesOrder);
            if (!string.IsNullOrWhiteSpace(message)) return BadRequest(message);

            SaveTransaction(salesOrder);
            db.SaveChanges();
            return Ok();
        }

        [HttpPut] //edit
        [Route("api/SalesOrder")]
        public IHttpActionResult Put([FromBody] SalesOrder salesOrder)
        {
            if (!string.IsNullOrWhiteSpace(verifyTransaction(salesOrder)))
            {
                return BadRequest(verifyTransaction(salesOrder));
            }

            UndoTransaction(salesOrder.SalesOrderId);
            SaveTransaction(salesOrder);
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("api/SalesOrder")]
        public IHttpActionResult Delete(Guid id)
        {
            UndoTransaction(id);
            db.SaveChanges();
            return Ok();
        }

        public string verifyTransaction(SalesOrder salesOrder)
        {
            string message = string.Empty;
            bool itemQtyIsIlegalFlag = false;
            bool itemDoesNotExistFlag = false;
            if (db.SalesOrders.Any(x => x.NomorSalesOrder == salesOrder.NomorSalesOrder && x.SalesOrderId != salesOrder.SalesOrderId))
            {
                message = "Sorry, kode data ini sudah adaaa";
            }

            foreach (var salesOrderItem in salesOrder.SalesOrderItems)
            {
                var dataInventory = db.Inventories.SingleOrDefault(x => x.InventoryCode == salesOrderItem.KodeBarang);
                if (dataInventory == null)
                {
                    itemDoesNotExistFlag = true;
                }
                if (salesOrderItem.ItemQty > dataInventory.InventoryQty || salesOrderItem.ItemQty <= 0)
                {
                    itemQtyIsIlegalFlag = true;
                }
            }

            if (db.SalesOrders.Any(x => x.NomorSalesOrder == salesOrder.NomorSalesOrder))
            {
                message += "Sorry, kode data ini sudah adaaa \n";
            }
            else if (itemQtyIsIlegalFlag || itemDoesNotExistFlag)
            {
                message += "Sorry, barang yang dibeli kuantitasnya salah atau tidak ada. \n";
            }

            return message;
        }

        public void SaveTransaction(SalesOrder salesOrder)
        {

            salesOrder.SalesOrderId = Guid.NewGuid();
            foreach (var salesOrderItem in salesOrder.SalesOrderItems)
            {
                salesOrderItem.SalesOrderItemId = Guid.NewGuid();
                salesOrderItem.SalesOrderId = salesOrder.SalesOrderId;
                salesOrderItem.ItemQtyDelivered = 0;

                var dataInventory = db.Inventories.Single(x => x.InventoryCode == salesOrderItem.KodeBarang);
                //dataInventory.InventoryQty = dataInventory.InventoryQty - salesOrderItem.ItemQty;
                dataInventory.BackOrderSalesOrder += (double)salesOrderItem.ItemQty;

                //db.Inventories.Attach(dataInventory);
                db.Entry(dataInventory).State = System.Data.Entity.EntityState.Modified;

            }
            db.SalesOrders.Add(salesOrder);
        }

        //public void IsCompletedCheck(SalesOrder salesOrder)
        //{
        //    bool[] temporaryArray = new bool[salesOrder.SalesOrderItems.Count];
        //    for (var i = 0; i < salesOrder.SalesOrderItems.Count; i++)
        //    {

        //        if (salesOrder.SalesOrderItems[i].ItemQtyDelivered >= salesOrder.SalesOrderItems[i].ItemQty)
        //        {
        //            salesOrder.SalesOrderItems[i].IsCompleted = true;
        //            temporaryArray[i] = true;
        //        }
        //        else
        //        {
        //            temporaryArray[i] = false;
        //        }
        //    }
        //    bool temporaryChecker = true;
        //    foreach (bool x in temporaryArray)
        //    {
        //        temporaryChecker = temporaryChecker && x;
        //    }
        //    salesOrder.IsCompleted = temporaryChecker;
        //}






        public void UndoTransaction(Guid id)
        {
            var salesOrder = db.SalesOrders.Include(x => x.SalesOrderItems).SingleOrDefault(x => x.SalesOrderId == id);
            foreach (var oldSalesOrderItem in salesOrder.SalesOrderItems)
            {
                var dataInventory = db.Inventories.Single(x => x.InventoryCode == oldSalesOrderItem.KodeBarang);
                //dataInventory.InventoryQty = dataInventory.InventoryQty + oldSalesorderItem.ItemQty;
                dataInventory.BackOrderSalesOrder -= (double)oldSalesOrderItem.ItemQty;
                db.Entry(dataInventory).State = System.Data.Entity.EntityState.Modified;

            }

            db.SalesOrders.Remove(salesOrder);
        }
    }

}