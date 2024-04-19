using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latihan2.Models;

namespace latihan2.Controllers
{
    public class InventoryController : ApiController
    {
        private Context db;
        public InventoryController()
        {
            db = new Context();
        }

        [HttpGet]
        [Route("api/Inventory")]
        //public IHttpActionResult GetInventory(int itemPerPage, int pageNumber)
        public IHttpActionResult GetInventory()
        {
            var dataInventory = db.Inventories.OrderBy(x => x.InventoryCode).ToList();

            return Ok(dataInventory);
        }

        [HttpGet]
        [Route("api/Inventory")]
        public IHttpActionResult Get(Guid id)
        {
            var dataInventory = db.Inventories.SingleOrDefault(x => x.InventoryId == id);
            return Ok(dataInventory);
        }

        [HttpPost]  //save
        [Route("api/Inventory")]
        public IHttpActionResult PostInventory([FromBody] Inventory inventory)
        {
            if (db.Inventories.Any(x => x.InventoryCode == inventory.InventoryCode))
            {
                return BadRequest("Sorry, kode data ini sudah adaaa");
            }
            else
            {
                inventory.InventoryId = Guid.NewGuid();
                db.Inventories.Add(inventory);
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpPut] //edit
        [Route("api/Inventory")]
        public IHttpActionResult Put([FromBody] Inventory inventory)
        {
            var tes = VerifyData(inventory);
            if (!string.IsNullOrWhiteSpace(tes))
            {
                return BadRequest(VerifyData(inventory));
            }

            db.Entry(inventory).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [HttpDelete] //delete
        [Route("api/Inventory")]
        public IHttpActionResult Delete(Guid id)
        {
            var inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
            db.SaveChanges();
            return Ok();
        }

        [NonAction]
        public string VerifyData(Inventory inventory)
        {
            var message = string.Empty; //""
            if (db.Inventories.Any(x => x.InventoryCode == inventory.InventoryCode && x.InventoryId != inventory.InventoryId))
            {
                message = "Sorry, kode data ini sudah adaaa";
            }

            return message;
        }

        private List<Inventory> InventoriesShortcut()
        {
            return db.Inventories.ToList();
        }

        private void InventoriesShortcut(List<Inventory> inventories)
        {
            inventories = db.Inventories.ToList();
        }

        // nama keduanya bisa sama karena..."method overloading"?

        private System.Data.Entity.DbSet<Inventory> Tes()
        {
            return db.Inventories;
        }

    }
}
