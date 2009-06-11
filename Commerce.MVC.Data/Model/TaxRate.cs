using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data
{
    public class TaxRate
    {
        public int ID { get; set; }
        public decimal Rate { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public TaxRate(decimal rate, string region, string country, string zip)
        {
            Rate = rate;
            Region = region;
            Country = country;
            Zip = zip;
        }

        public TaxRate() { }

    }
}
