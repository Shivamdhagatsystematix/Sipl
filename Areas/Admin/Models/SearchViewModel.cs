using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipl.Areas.Admin.Models
{
    public class SearchViewModel
    {


        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string RoleId { get; set; }
        public List<SelectListItem> Role { get; set; }
        public string RoleName { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<SelectListItem> Course { get; set; }
        public string CourseName { get; set; }
        public string Gender { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString =
                "{0:yyyy-MM-dd}",
          ApplyFormatInEditMode = true)]

        public System.DateTime DOB { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateCreated { get; set; }

        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }
        [Display(Name = " Permanant Address")]
        public string PermanantAddress { get; set; }
        [Display(Name = "Country")]

        public int CountryId { get; set; }
        public string Country { get; set; }
        [Display(Name = "State")]
        public int StateId { get; set; }
        public string States { get; set; }
        [Display(Name = "City")]
        public int CityId { get; set; }
        public string Cities { get; set; }
    }

    public class FilterViewModel
    {
        public List<SearchViewModel> List { get; set; }
        [Display(Name = "First Name")]

        public string FirstName { get; set; }
        [Display(Name = "Last Name")]


        public string LastName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<SelectListItem> Role { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public IEnumerable<SelectListItem> Course { get; set; }
        public string Gender { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString =
                "{0:yyyy-MM-dd}",
          ApplyFormatInEditMode = true)]

        public System.DateTime? DOB { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateCreated { get; set; }

        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }
        [Display(Name = " Permanant Address")]
        public string PermanantAddress { get; set; }
        [Display(Name = "Country")]

        public int CountryId { get; set; }
        public string Country { get; set; }
        [Display(Name = "State")]
        public int StateId { get; set; }
        public string States { get; set; }
        [Display(Name = "City")]
        public int CityId { get; set; }
        public string Cities { get; set; }
    }

    public class CountryModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class StateModel
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
    public class CityModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
