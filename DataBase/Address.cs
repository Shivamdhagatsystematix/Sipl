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
    
    public partial class Address
    {
        public int AddressId { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanantAddress { get; set; }
        public byte UserId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Pincode { get; set; }
    
        public virtual Cities Cities { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual States States { get; set; }
        public virtual NetUsers NetUsers { get; set; }
    }
}
