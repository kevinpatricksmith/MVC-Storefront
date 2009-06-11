using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services
{
    public interface ISalesTaxService
    {
        decimal CalculateTaxAmount(Order order);
    }
}
