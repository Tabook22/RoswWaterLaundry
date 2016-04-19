using laundry.Models.DB;
using laundry.ViewModels;
using System.Linq;

namespace laundryTest.Models
{
    public class AccountModel
    {
        private LundryDbContext db = new LundryDbContext();
        private tbl_Account listAccounts = new tbl_Account();
       
        //checks to see if there is any role associated with the Username which sotred in the session Username
        public tbl_Account find(string username)
        {
            
        var getAR = (from a in db.tbl_Account
                        select new AccountRoleViewModel
                        {
                            AID = a.AID,
                            Username = a.Username,
                            FName = a.FName,
                            LName = a.LName,
                            Role = a.Role,
                            Branch = a.Branch,
                            CDate = a.CDate
                        }).FirstOrDefault();
            listAccounts.AID = getAR.AID;
            listAccounts.Username = getAR.Username;
            listAccounts.Password = getAR.Password;
            listAccounts.FName = getAR.FName;
            listAccounts.LName = getAR.LName;
            listAccounts.CDate = getAR.CDate;
            return listAccounts;
        }

        public tbl_Account login(string username, string password, int branch)
        {
            return db.tbl_Account.Where(x=>x.Username==username && x.Password ==password && x.Branch==branch).FirstOrDefault() ;
        }
    }
}
