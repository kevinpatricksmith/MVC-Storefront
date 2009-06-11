using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Commerce.Data.SqlRepository;

namespace Commerce.Data {
    public class LinqRepository<T> where T:class,new() {

        internal DB _db = null;
        public LinqRepository() {
            _db = new DB();
        }


        /// <summary>
        /// Gets the table provided by the type T and returns for querying
        /// </summary>
        private Table<T> Table {
            get { return _db.GetTable<T>(); }
        }

        /// <summary>
        /// Returns all T records in the repository
        /// </summary>
        public virtual IQueryable<T> GetAll() {
            return Table;
        }

        /// <summary>
        /// Returns a PagedList of items
        /// </summary>
        /// <param name="pageIndex">zero-based index to be used for lookup</param>
        /// <param name="pageSize">the size of the paged items</param>
        /// <returns></returns>
        public virtual PagedList<T> GetPaged(int pageIndex, int pageSize) {
            return new PagedList<T>(Table, pageIndex, pageSize);
        }

        /// <summary>
        /// Finds an item using a passed-in expression lambda
        /// </summary>
        public virtual IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> expression) {
            return Table.Where(expression);
        }

        public virtual void Insert(T item) {
            Table.InsertOnSubmit(item);
            _db.SubmitChanges();

        }

        public virtual void Update(T Item) {
            _db.SubmitChanges();

        }

        /// <summary>
        /// Deletes an item from the database
        /// </summary>
        /// <param name="item"></param>
        public virtual void Delete(T item) {
            Table.DeleteOnSubmit(item);
            _db.SubmitChanges();
        }



    }
}
