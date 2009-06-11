using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services
{
    public class SimpleShippingService:IShippingService
    {

        IShippingRepository _shippingRepository;

        public SimpleShippingService(IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        public IList<ShippingMethod> CalculateRates(Order order)
        {
            //pull the methods out
            var methods = _shippingRepository.GetRates(order);

            //their might be restrictions on the shipping
            //such as no ground delivery to HI...

            return methods.ToList();
        }

    }
}
