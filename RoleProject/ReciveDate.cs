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
    
    public partial class ReciveDate
    {
        public int id { get; set; }
        public System.DateTime Start_Recive_Date { get; set; }
        public System.DateTime End_Recive_Date { get; set; }
        public double Total_Cost { get; set; }
        public Nullable<int> cars_Car_Id { get; set; }
        public string client_Client_ID { get; set; }
    
        public virtual Car Car { get; set; }
        public virtual Client Client { get; set; }
    }
}
