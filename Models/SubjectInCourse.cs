//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sipl.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubjectInCourse
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int CourseId { get; set; }
    
        public virtual Cours Cours { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
