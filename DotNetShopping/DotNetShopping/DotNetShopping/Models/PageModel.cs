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
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }


    public class PageCreateModel
    {
        public string PageId { get; set; }
        public string PageTitle { get; set; }
        public string PageBody { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class PageEditModel
    {
        public string PageId { get; set; }
        public string PageTitle { get; set; }
        public string PageBody { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class PageViewModel
    {
        public string PageId { get; set; }
        public string PageTitle { get; set; }
        public string PageBody { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class PageDeleteModel
    {
        public string PageId { get; set; }
        public string PageTitle { get; set; }
        public string PageBody { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}