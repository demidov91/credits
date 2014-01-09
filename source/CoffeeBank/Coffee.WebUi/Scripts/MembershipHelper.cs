using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.WebUi.Scripts
{
    public class MembershipHelper
    {
        public static bool IsExternalUser(System.Security.Principal.IPrincipal user) {
            return !(user.IsInRole("Cashier") || user.IsInRole("Clerk") || user.IsInRole("Committee"));
        } 
    }
}