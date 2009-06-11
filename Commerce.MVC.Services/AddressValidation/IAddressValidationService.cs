using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services {
    public interface IAddressValidationService {
        Address VerifyAddress(Address address);
    }
}
