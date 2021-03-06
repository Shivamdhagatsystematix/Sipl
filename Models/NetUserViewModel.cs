﻿using Sipl.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipl.Models
{
    //public class ExternalLoginConfirmationViewModel
    //{
    //    [Required]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }
    //}

    //public class ExternalLoginListViewModel
    //{
    //    public string ReturnUrl { get; set; }
    //}

    //public class SendCodeViewModel
    //{
    //    public string SelectedProvider { get; set; }
    //    public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    //    public string ReturnUrl { get; set; }
    //    public bool RememberMe { get; set; }
    //}

    //public class VerifyCodeViewModel
    //{
    //    [Required]
    //    public string Provider { get; set; }

    //    [Required]
    //    [Display(Name = "Code")]
    //    public string Code { get; set; }
    //    public string ReturnUrl { get; set; }

    //    [Display(Name = "Remember this browser?")]
    //    public bool RememberBrowser { get; set; }

    //    public bool RememberMe { get; set; }
    //}

    //public class ForgotViewModel
    //{
    //    [Required]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }
    //}

    /// <summary>
    /// ViewModel For Registration
    /// </summary>
    public class NetUserViewModel
    {
        public byte? UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
       
        public string RoleId { get; set; }
        public string RoleName { get; set; }
       
        public List<SelectListItem> Role { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }

        public IEnumerable<SelectListItem> Course { get; set; }

        public string Gender { get; set; }


        [Required]
        [Remote("CheckEmailExists", "NetUserView", HttpMethod = "POST", ErrorMessage = "Email address already registered.")]
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString =
                "{0:yyyy-MM-dd}",
          ApplyFormatInEditMode = true)]

        public System.DateTime DOB { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
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
        [Required(ErrorMessage = "Zip is Required")]
        [RegularExpression(@"[0-9]{5}", ErrorMessage = "Invalid Zip")]
        public string Pincode { get; set; }
        public List<NetUserViewModel> List { get; set; }

    }
    public class CrudViewModel
    {
        public byte UserId { get; set; }
        public int Id { get; set; }
        public int AddressId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "- Select Role -")]
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<SelectListItem> Role { get; set; }
        public int? CourseId { get; set; }
        public int SubjectId { get; set; }
        public string CourseName { get; set; }

        public IEnumerable<SelectListItem> Course { get; set; }
        [Required]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

    
        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString =
                "{0:yyyy-MM-dd}",
          ApplyFormatInEditMode = true)]

        public System.DateTime DOB { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }
        [Display(Name = " Permanant Address")]
        public string PermanantAddress { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public string Country { get; set; }

        [Required]
        [Display(Name = "States")]
        public int StateId { get; set; }
        public string States { get; set; }

        [Required]
        [Display(Name = "City")]
        public int CityId { get; set; }
        public string Cities { get; set; }
        [Required(ErrorMessage = "Zip is Required")]
        [RegularExpression(@"[0-9]{5}", ErrorMessage = "Invalid Zip")]
        public string Pincode { get; set; }
        public List<CrudViewModel> List { get; set; }

    }
    //public class ResetPasswordViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm password")]
    //    [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }

    //    public string Code { get; set; }
    //}

    //public class ForgotPasswordViewModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }
    //}

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

