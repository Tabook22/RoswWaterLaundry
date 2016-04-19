using laundryTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace laundry.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //The OnAuthorization() helper method checks the user authentication with AuthorizationContext class. 
        //If the Result property returns null, then the Authorization is successful and user can continue the operation; 
        //else if the user is authenticated but not authorized, then an Error Page will be returned.

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionPersister.Username)) //check if there is any value in the seassion Username, if there is no value or null then the user will be redirected to login page
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            else
            {
                //here if there is a value in the session Username, then we are going to check to see if the user have the right role to see the selected page
                AccountModel am = new AccountModel();
                CustomPrincipal mp = new CustomPrincipal(am.find(SessionPersister.Username)); //check to see if there is a session, contains the username. that can be done through AccountModel, which contains the method find, which returns a tbl_Account object
                if (!mp.IsInRole(Roles))
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccessDenied", action = "Index" }));
            }

        }
    }
}
