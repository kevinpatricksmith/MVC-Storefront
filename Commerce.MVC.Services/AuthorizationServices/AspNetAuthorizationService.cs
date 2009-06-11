using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Collections.Specialized;

namespace Commerce.Services {
    public class AspNetAuthorizationService:IAuthorizationService {
        #region IAuthorizationService Members

        public AspNetAuthorizationService(){}
        object _lock = null;
        const string SUPER_ADMIN_ROLE = "SuperAdmin";

        public AspNetAuthorizationService(string connectionStringName, string appName) {
            if (Roles.Providers.Count == 0) {

                //lock it off to avoid collisions
                lock (_lock) {

                    SqlRoleProvider provider = new SqlRoleProvider();

                    //The settings
                    NameValueCollection settings = new NameValueCollection();
                    settings.Add("connectionStringName", connectionStringName);

                    //initialize it
                    provider.Initialize(appName, settings);

                    Roles.Providers.Add(provider);

                }
            }
        }

        public bool IsSuperAdmin(string userName) {
            return Roles.IsUserInRole(userName, SUPER_ADMIN_ROLE);
        }

        #endregion
    }
}
