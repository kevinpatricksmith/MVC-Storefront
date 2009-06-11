using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data.SqlRepository;

namespace Commerce.Data
{
    public class SqlTaxRepository:ITaxRepository
    {

        DB _db;
        public SqlTaxRepository(DB db)
        {
            _db = db;
        }


        public IQueryable<TaxRate> GetTaxRates()
        {
            return from rates in _db.TaxRates
                   select new TaxRate
                   {
                       ID = rates.TaxRateID,
                       Country = rates.Country,
                       Region = rates.Region,
                       Rate = rates.Rate
                   };
        }

    }
}
