using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services
{
    
    /// <summary>
    /// This is a ridiculously simple tax calculator
    /// </summary>
    public class RegionalSalesTaxService:ISalesTaxService
    {

        ITaxRepository _taxRepository;
        public RegionalSalesTaxService(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        public decimal CalculateTaxAmount(Order order)
        {
            decimal result = 0;

            //get the rates from the DB

            //sales tax is a tricky thing
            //you should speak to an accountant or CPA with regards to your needs
            //This sample tax calculator applies SalesTax by state
            //Sales tax is generally applied when you your business has a physical presence, or "Nexxus"
            //in the same location as the shipping address.
            //In the US, taxes are applied based on jurisdictions, which are address-based.
            //It is your responsibility to calculate taxes correctly

            //if the order's shipping address is located in the same state as our business
            //apply the tax
            
            //this assumes the input uses ISO 2-letter codes
            TaxRate rate = GetRate(order);

            if (rate != null)
            {
                result = rate.Rate * order.TaxableGoodsSubtotal;
            }

            return result;


        }
        TaxRate GetRate(Order order)
        {
            var rates = _taxRepository.GetTaxRates().ToList();

            //check the ZIP first - this allows zip-based overrides for places
            //like Universities, that may have 4% lower tax
            TaxRate result = (from r in rates
                              where r.Zip == order.ShippingAddress.Zip
                              select r).SingleOrDefault();

            if (result == null)
                result = (from rs in rates
                          where rs.Region == order.ShippingAddress.StateOrProvince &&
                          rs.Country == order.ShippingAddress.Country
                          select rs).SingleOrDefault();

            return result;

        }
 
    }
}
