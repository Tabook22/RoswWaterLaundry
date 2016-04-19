using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace laundry.Models.DB
{

    public class tbl_images
    {
        [Key]
        public int Id { get; set; }
        public string imgTitle { get; set; }
        public string imgL { get; set; }
        public string imgS { get; set; }
        public string imgUrl { get; set; }
    }
}
