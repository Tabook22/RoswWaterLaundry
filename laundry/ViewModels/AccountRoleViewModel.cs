using System;
using System.ComponentModel.DataAnnotations;

namespace laundry.ViewModels
{
    public class AccountRoleViewModel
    {
        public int AID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CDate { get; set; }
        public int Branch { get; set; }
        public string Role { get; set; }

    }
}