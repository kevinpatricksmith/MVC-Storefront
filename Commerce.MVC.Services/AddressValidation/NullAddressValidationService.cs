using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Services
{
    public class NullAddressValidationService:IAddressValidationService
    {

        public Commerce.Data.Address VerifyAddress(Commerce.Data.Address address)
        {
            return address;
        }

    }
}
