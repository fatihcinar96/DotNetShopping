using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DotNetShopping.Models
{
    public class Page
    {
        [Key]
        public string PageId { get; set; }
        public string PageTitle { get; set; }
        public string PageBody { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateDate { get; set; }
    }
}