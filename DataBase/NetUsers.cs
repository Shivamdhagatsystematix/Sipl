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
    
    public partial class NetUsers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NetUsers()
        {
            this.UserRole = new HashSet<UserRole>();
        }
    
        public byte UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> CourseId { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public System.DateTime DOB { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateModified { get; set; }
        public bool IsVerified { get; set; }
    
        public virtual Courses Courses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
