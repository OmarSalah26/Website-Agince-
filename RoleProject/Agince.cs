//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RoleProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class Agince
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agince()
        {
            this.Cars = new HashSet<Car>();
        }
    
        public string Agince_ID { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string photo_Agince { get; set; }
        public string userAccount_Id { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }
    }
}
