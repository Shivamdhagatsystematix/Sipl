using Sipl.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipl.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }



    public class NetUserViewModel
    {
        //public Entities db = new Entities();

        public byte? UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required]

        public string RoleId { get; set; }
        public IEnumerable<SelectListItem> Role { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<SelectListItem> Course { get; set; }



        public string Gender { get; set; }

        [Required]
        [Display(Name = "Email")]

        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString =
                "{0:yyyy-MM-dd}",
          ApplyFormatInEditMode = true)]
        public System.DateTime DOB { get; set; }

        public bool IsActive { get; set; }

        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }
        [Display(Name = " Permanant Address")]
        public string PermanantAddress { get; set; }
        //[Display(Name = "Country")]
        public int CountryId { get; set; }
        public string  Country { get; set; }

        //[Display(Name = "State")]
        public int StateId { get; set; }
        public string States { get; set; }
        
        //[Display(Name = "City")]

        public int CityId { get; set; }
        public string Cities { get; set; }


    }
    
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
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
    public class  CityModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }





}

