//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sipl.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class SubjectInCourse
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "please select Subject")]
        [Display(Name = " Subjects")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "please select Course")]
        [Display(Name = " Courses")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "please select Course")]
        [Display(Name = " Courses")]

        public virtual Courses Courses { get; set; }
        [Required(ErrorMessage = "please select Subject")]
        [Display(Name = " Subjects")]
        public virtual Subjects Subjects { get; set; }
    }
}
