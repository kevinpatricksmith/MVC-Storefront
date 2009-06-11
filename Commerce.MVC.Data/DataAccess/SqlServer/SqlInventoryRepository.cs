using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data.SqlRepository;

namespace Commerce.Data {
    public class SqlInventoryRepository:IInventoryRepository {
        DB _db;
        public SqlInventoryRepository(DB dataContext) {
            _db = dataContext;
        }

        public IQueryable<InventoryRecord> GetInventory() {

            return from i in _db.InventoryRecords
                   select new InventoryRecord
                   {
                       ID = i.InventoryRecordID,
                       DateEntered = i.DateEntered,
                       Increment = i.Increment,
                       Notes = i.Notes,
                       ProductID = i.ProductID
                   };


        }

        public void Increment(int productID, int amount, string notes) {


            using (DB db = new DB()) {
                Commerce.Data.SqlRepository.InventoryRecord record = new Commerce.Data.SqlRepository.InventoryRecord();
                record.ProductID = productID;
                record.Notes = notes;
                record.Increment = amount;
                record.DateEntered = DateTime.Now;
                db.InventoryRecords.InsertOnSubmit(record);

                db.SubmitChanges();
            }

        }


    }
}
