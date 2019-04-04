using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class Category
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 CategoryId { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Int16 ParentId { get; set; }

    }
    public class CategoryListModel
    {
        public Int16 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Int16 ParentId { get; set; }
        public string ParentName { get; set; }
    }
}