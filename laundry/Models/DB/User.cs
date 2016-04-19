using System;
using System.Collections.Generic;
namespace laundry.Models.DB
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
