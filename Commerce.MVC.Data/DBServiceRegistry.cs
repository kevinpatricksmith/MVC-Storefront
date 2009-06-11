using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using StructureMap;
using Commerce.Data.SqlRepository;
using StructureMap.Attributes;

namespace Commerce.Data {
    
    public class DBServiceRegistry:Registry {
        protected override void configure() {
           
            ForRequestedType<DB>()
                .TheDefaultIs(() => new DB())
                .CacheBy(InstanceScope.Hybrid);

        }
    }
}
