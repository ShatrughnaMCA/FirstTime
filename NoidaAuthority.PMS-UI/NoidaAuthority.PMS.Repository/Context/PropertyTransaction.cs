//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoidaAuthority.PMS.Repository.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class PropertyTransaction
    {
        public int Id { get; set; }
        public Nullable<int> Rid { get; set; }
        public string Property_Number { get; set; }
        public string Service_Name { get; set; }
        public Nullable<int> User_Identity { get; set; }
        public Nullable<System.DateTime> Service_Date { get; set; }
        public Nullable<bool> Is_Active { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}
