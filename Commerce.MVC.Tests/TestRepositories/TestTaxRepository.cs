using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Tests
{
    public class TestTaxRepository:ITaxRepository
    {

        public IQueryable<TaxRate> GetTaxRates()
        {

            List<TaxRate> rates = new List<TaxRate>();
            rates.Add(new TaxRate(0.05M, "HI", "US", ""));
            rates.Add(new TaxRate(0.06M, "NV", "US", ""));
            rates.Add(new TaxRate(0.08M, "CA", "US", ""));

            return rates.AsQueryable();

        }

    }
}
