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
    
    public partial class countries
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public countries()
        {
            this.Address = new HashSet<Address>();
            this.Cities = new HashSet<Cities>();
            this.states = new HashSet<states>();
        }
    
        public string countryID { get; set; }
        public string countryName { get; set; }
        public string localName { get; set; }
        public string webCode { get; set; }
        public string region { get; set; }
        public string continent { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double surfaceArea { get; set; }
        public int population { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cities> Cities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<states> states { get; set; }
    }
}