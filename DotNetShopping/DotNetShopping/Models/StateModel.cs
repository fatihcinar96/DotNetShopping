using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class State
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 StateId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [Display(Name = "State Name")]
        public string Name { get; set; }

        [ForeignKey("Country")]
        public Int16 CountryId { get; set; }
        public Country Country { get; set; }
        
    }
}