using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Services;

namespace Commerce.Tests {
    public class TestAddressValidator:IAddressValidationService {
        #region IAddressValidationService Members

        public Commerce.Data.Address VerifyAddress(Commerce.Data.Address address) {

            if (address.City == "Fail Town")
                throw new InvalidOperationException("Throw condition");
            
            return address;
        }

        #endregion
    }
}
