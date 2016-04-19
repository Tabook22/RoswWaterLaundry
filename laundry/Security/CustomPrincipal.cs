using System.Linq;
using laundry.Models.DB;
using laundry.ViewModels;
using System.Security.Principal;

namespace laundry.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private LundryDbContext db = new LundryDbContext();
        private tbl_Account Account;
        //private AccountModel am = new AccountModel();
        public CustomPrincipal(tbl_Account account )
        {
            this.Account = account;
            this.Identity = new GenericIdentity(account.Username);
        }
       public IIdentity Identity
        {
            get; set;
        }

        public bool IsInRole(string role)
        {
            var getAR = from a in db.tbl_Account
                        select new AccountRoleViewModel
                        {
                            AID = a.AID,
                            Username = a.Username,
                            FName = a.FName,
                            LName = a.LName,
                            CDate=a.CDate,
                            Branch =a.Branch,
                            Role =a.Role
                        };
            
            if(getAR.Count()>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public bool IsInRole(string role)
        // {
        //     var roles = role.Split(new char[] {','});
        //     return roles.Any(r => this.Account.Roles.Contains(r));
        // }
    }
}
