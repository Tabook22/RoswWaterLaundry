using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace laundry.Security
{
    public static class SessionPersister
    {
        static string usernameSessionvar = "username"; // this is the name of the session
        static string branchIDSessionvar = "branch"; // this is the Id of the Branch
        static string branchNameSessionvar = "branchID"; // this is the name of the Branch
        static string printbillSessionvar = "printbillno"; // this is the name of the Branch
        //the following property Username will take the current session value which is username, prediffined before in static string usernameSessionvar
        public static string Username 
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionVar = HttpContext.Current.Session[usernameSessionvar];//the get the current session, because the HttpContext.Current, contains all the information about the current threat including the session information
                if (sessionVar != null) // checking the see if the session with the name username sessionvar is emplty or not
                    return sessionVar as string;

                return null;
            }
            set
            {
                HttpContext.Current.Session[usernameSessionvar] = value;//taking the session value

            }
        }
        public static string BranchID
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionVar2 = HttpContext.Current.Session[branchIDSessionvar];
                if (sessionVar2 != null) // checking the see if the session with the name username sessionvar is emplty or not
                    return sessionVar2 as string;

                return null;
            }
            set
            {
                HttpContext.Current.Session[branchIDSessionvar] = value;//taking the session value
            }
        }

        public static string BranchName
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionVar3 = HttpContext.Current.Session[branchNameSessionvar];
                if (sessionVar3!= null) // checking the see if the session with the name username sessionvar is emplty or not
                    return sessionVar3 as string;

                return null;
            }
            set
            {
                HttpContext.Current.Session[branchNameSessionvar] = value;//taking the session value
            }
        }

        //print bill number
        public static string printBillNo
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionVar4 = HttpContext.Current.Session[printbillSessionvar];
                if (sessionVar4 != null) // checking the see if the session with the name username sessionvar is emplty or not
                    return sessionVar4 as string;

                return null;
            }
            set
            {
                HttpContext.Current.Session[printbillSessionvar] = value;//taking the session value
            }
        }
    }
}
