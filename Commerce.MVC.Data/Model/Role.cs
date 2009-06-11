using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    
    public class Role {

        public string Name { get; set; }
        public bool IsAdmin { get; set; }


        public Role() { }
        public Role(string roleName, bool isAdmin) {
            this.Name = roleName;
            this.IsAdmin = isAdmin;
        }


    }
}
