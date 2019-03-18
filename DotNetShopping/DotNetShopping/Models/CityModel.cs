using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class City
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 CityId { get; set; }
        [Required]
        [Display(Name = "City Name")]
        public string Name { get; set; }
        public string Code { get; set; }
        public Int16 StateId { get; set; }
        [ForeignKey("Country")]
        public Int16 CountryId { get; set; }
        public Country Country { get; set; }
    }
    public class CityListModel
    {
        public Int16 CityId { get; set; }
        [Display(Name = "City Name")]
        public string Name { get; set; }
        public string Code { get; set; }
        public string StateName { get; set; }
        public Int16 StateId { get; set; }
        public Int16 CountryId { get; set; }
        public string CountryName { get; set; }
    }
}