using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public interface IIncentiveRepository {

        IQueryable<Incentive> GetIncentives();
    }
}
