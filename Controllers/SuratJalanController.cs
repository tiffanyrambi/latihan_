using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latihan2.Models;
using System.Collections;

namespace latihan2.Controllers
{
    public class SuratJalan2
    {
        public string NamaSuratJalan { get; set; }
        public string NomorSalesOrder { get; set; }
    }

    public class SuratJalanController : ApiController
    {
        private Context db;

        public SuratJalanController()
        {
            db = new Context();
        }

        [HttpGet]
        [Route("api/SuratJalan")]
        public IHttpActionResult GetSuratJalans()
        {
            List<SuratJalan> suratJalans = db.SuratJalans.OrderBy(x => x.NamaSuratJalan).ToList();

            //var querySuratJalans = (from suratJalan in db.SuratJalans join suratJalanItem in db.SuratJalanItems 
            //                        on suratJalan.SuratJalanId equals suratJalanItem.SuratJalanId
            //                        where suratJalan.NamaSuratJalan == "1SJ4_1"
            //                        select new
            //                        {
            //                            suratJalan.NamaSuratJalan,
            //                            suratJalanItem.KodeBarang
            //                        }).ToList();

            //var querySuratJalan2s = (from suratJalan in db.SuratJalans
            //                         join suratJalanItem in db.SuratJalanItems
            //                         on suratJalan.SuratJalanId equals suratJalanItem.SuratJalanId into sJItems
            //                         where suratJalan.NamaSuratJalan == "1SJ4_1"
            //                         select new
            //                         {
            //                             suratJalan.NamaSuratJalan,
            //                             Items = sJItems.ToList(), 
            //                             TotalAllQty = sJItems.AsEnumerable().Sum(x => x.ItemQtyDelivered)
            //                         }).ToList();

            return Ok(suratJalans);
        }

        [HttpGet]
        [Route("api/SuratJalan")]
        public IHttpActionResult Get(Guid id)
        {
            var suratJalan = db.SuratJalans.Include(x => x.SuratJalanItems).SingleOrDefault(x => x.SuratJalanId == id);
            suratJalan.SuratJalanItems = suratJalan.SuratJalanItems.OrderBy(x => x.NomorBaris).ToList();
            var relatedSalesOrderItems = db.SalesOrderItems.Where(x => x.SalesOrderId == suratJalan.SalesOrderId).ToList();

            foreach (var suratJalanItem in suratJalan.SuratJalanItems)
            {
                var salesOrderItem = relatedSalesOrderItems.Single(x => x.SalesOrderItemId == suratJalanItem.SalesOrderItemId);
                suratJalanItem.ItemQtyBatas = suratJalanItem.ItemQtyOrder - salesOrderItem.ItemQtyDelivered + suratJalanItem.ItemQtyDelivered;
            }

            return Ok(suratJalan);

            //cara alternatif: salesOrderItems dibawa keluar ke JS

            ////filter relatedSalesOrderItem karena mungkin gak semuanya itu ada di suratJalan.SuratJalanItems
            ////misalnya, salah satu item sudah terpenuhi
            //List<SalesOrderItem> filteredSalesOrderItems = new List<SalesOrderItem>();
            //foreach (var suratJalanItem in suratJalan.SuratJalanItems)
            //{
            //    var salesOrderItem = relatedSalesOrderItems.Single(x => x.SalesOrderItemId == suratJalanItem.SalesOrderItemId);
            //    filteredSalesOrderItems.Add(salesOrderItem);
            //}
            ////saya perlu bawa keluar SuratJalan dan relatedSalesOrderItems sekaligus, jadi saya pakai struktur data yang di bawah ini
            //ArrayList sJPlusSO = new ArrayList { suratJalan, filteredSalesOrderItems };
            //return Ok(sJPlusSO);

            //kalau pakai cara ini, di JS berhasil dibawa keluar, tinggal diindexing aja hasil "onSuccess.data"-nya jadi "onSuccess.data[0]" dan "onSuccess.data[1]"

        }

        [HttpPost] //save
        [Route("api/SuratJalan")]
        public IHttpActionResult PostSale([FromBody] SuratJalan suratJalan)
        {

            string message = VerifyTransaction(suratJalan);
            if (!string.IsNullOrWhiteSpace(message))
            {
                return BadRequest(message);
            }

            SaveAction(suratJalan);
            db.SaveChanges();
            return Ok();
        }

        [HttpPut] //edit
        [Route("api/SuratJalan")]
        public IHttpActionResult Put([FromBody] SuratJalan suratJalan)
        {
            string message = VerifyTransaction(suratJalan);
            if (!string.IsNullOrWhiteSpace(message))
            {
                return BadRequest(message);
            }

            UndoAction(suratJalan.SuratJalanId);
            SaveAction(suratJalan);
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete] //delete
        [Route("api/SuratJalan")]
        public IHttpActionResult Delete(Guid id)
        {
            UndoAction(id);
            db.SaveChanges();
            return Ok();
        }


        public string VerifyTransaction(SuratJalan suratJalan)
        {
            string message = "";

            //if (db.SuratJalans.Any(x => x.NamaSuratJalan == suratJalan.NamaSuratJalan && x.SuratJalanId != suratJalan.SuratJalanId))
            //{
            //    message = "Sorry, kode data ini sudah adaaa";
            //}


            foreach (var suratJalanItem in suratJalan.SuratJalanItems)
            {
                // checks if ItemQtyDelivered is negative
                if (suratJalanItem.ItemQtyDelivered < 0)
                {
                    message += $"Sorry, kuantitas item ({suratJalanItem.NamaBarang}) dikirim tidak boleh negatif.\n";
                    continue;
                }
                // checks if itemQtyDelivered is more than the limit
                if (suratJalanItem.ItemQtyDelivered > suratJalanItem.ItemQtyBatas)
                {
                    message += $"Sorry, kuantitas item ({suratJalanItem.NamaBarang}) melebihi Batas.\n";
                    continue;
                }


            }

            return message;
        }

        public void SaveAction(SuratJalan suratJalan)
        {

            suratJalan.SuratJalanId = Guid.NewGuid();

            var salesOrderItemsFromDB = db.SalesOrderItems.Where(x => x.SalesOrderId == suratJalan.SalesOrderId).ToList();



            //for (var i = 0; i < suratJalan.SuratJalanItems.Count; i++)
            //{
            //    //assigning many ids to SuratJalanItems
            //    suratJalan.SuratJalanItems[i].SuratJalanItemId = Guid.NewGuid();
            //    suratJalan.SuratJalanItems[i].SuratJalanId = suratJalan.SuratJalanId;
            //    suratJalan.SuratJalanItems[i].SalesOrderId = suratJalan.SalesOrderId;
            //    suratJalan.SuratJalanItems[i].SalesOrderItemId = salesOrderFromDB.SalesOrderItems[i].SalesOrderItemId;
            //    suratJalan.SuratJalanItems[i].NomorBaris = i;
            //    //assigning itemQtyDelivered back to SalesOrderItem
            //    //untuk menghindari bug itemQtyDelivered ketimpa kalau 1 S.O. ada >2 S.J, ganti = jadi +=
            //    salesOrderFromDB.SalesOrderItems[i].ItemQtyDelivered += suratJalan.SuratJalanItems[i].ItemQtyDelivered;
            //}

            var i = 1;
            foreach (var suratJalanItem in suratJalan.SuratJalanItems)
            {

                suratJalanItem.SuratJalanItemId = Guid.NewGuid();
                suratJalanItem.SuratJalanId = suratJalan.SuratJalanId;
                suratJalanItem.NomorBaris = i;

                //Update Saloes Oredr Item punya Qty Kirim
                var salesOrderItemFromDb = salesOrderItemsFromDB.Single(x => x.SalesOrderItemId == suratJalanItem.SalesOrderItemId);
                salesOrderItemFromDb.ItemQtyDelivered += suratJalanItem.ItemQtyDelivered;
                db.Entry(salesOrderItemFromDb).State = EntityState.Modified;

                //update bagian inventory (BOSO dan item qty outgoing)
                var inventoryFromDB = db.Inventories.Single(x => x.InventoryCode == suratJalanItem.KodeBarang);
                inventoryFromDB.BackOrderSalesOrder -= (double)suratJalanItem.ItemQtyDelivered;
                inventoryFromDB.InventoryQtyOutgoing += (double)suratJalanItem.ItemQtyDelivered;
                db.Entry(inventoryFromDB).State = EntityState.Modified;

                i++;

            }


            //db.Entry(suratJalan).State = System.Data.Entity.EntityState.Modified;
            //db.SalesOrders.Attach(salesOrderFromDB);
            db.SuratJalans.Add(suratJalan);
        }

        public void UndoAction(Guid id)
        {
            var suratJalanToBeRemoved = db.SuratJalans.Include(x => x.SuratJalanItems).SingleOrDefault(x => x.SuratJalanId == id);
            var salesOrderItemsFromDB = db.SalesOrderItems.Where(x => x.SalesOrderId == suratJalanToBeRemoved.SalesOrderId).ToList();

            foreach (var suratJalanItem in suratJalanToBeRemoved.SuratJalanItems)
            {
                var salesOrderItemFromDB = salesOrderItemsFromDB.Single(x => x.SalesOrderItemId == suratJalanItem.SalesOrderItemId);
                salesOrderItemFromDB.ItemQtyDelivered -= suratJalanItem.ItemQtyDelivered;
                db.Entry(salesOrderItemFromDB).State = EntityState.Modified;

                //update bagian inventory (BOSO dan item qty outgoing)
                var inventoryFromDB = db.Inventories.Single(x => x.InventoryCode == suratJalanItem.KodeBarang);
                inventoryFromDB.BackOrderSalesOrder += (double)suratJalanItem.ItemQtyDelivered;
                inventoryFromDB.InventoryQtyOutgoing -= (double)suratJalanItem.ItemQtyDelivered;
                db.Entry(inventoryFromDB).State = EntityState.Modified;

            }

            db.SuratJalans.Remove(suratJalanToBeRemoved);
        }


    }
}