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
    
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.ReciveDates = new HashSet<ReciveDate>();
        }
    
        public string Client_ID { get; set; }
        public string Name { get; set; }
        public string phone_Number { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public Nullable<int> age { get; set; }
        public Nullable<int> number_of_licience { get; set; }
        public Nullable<System.DateTime> date_of_licience_expiry { get; set; }
        public string photo_Client { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReciveDate> ReciveDates { get; set; }
    }
}
