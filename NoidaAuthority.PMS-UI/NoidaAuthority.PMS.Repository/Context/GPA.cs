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
    
    public partial class GPA
    {
        public int Id { get; set; }
        public Nullable<int> Rid { get; set; }
        public string GPA_Holder_Name { get; set; }
        public string GPA_Holder_Address { get; set; }
        public Nullable<System.DateTime> Effcetd_From { get; set; }
        public Nullable<System.DateTime> Effected_To { get; set; }
        public Nullable<bool> Is_Active { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_date { get; set; }
        public string Document_No { get; set; }
        public string File_Path { get; set; }
        public string Relation { get; set; }
        public Nullable<System.DateTime> Application_Date { get; set; }
        public Nullable<System.DateTime> Acceptance_date { get; set; }
        public Nullable<bool> Registered { get; set; }
        public string Registration_No { get; set; }
        public Nullable<int> OnlineRequestNo { get; set; }
        public string BahiNo { get; set; }
        public string BahiZildNo { get; set; }
        public string BahiPageNo { get; set; }
        public string BahiSINo { get; set; }
        public string GPASOA { get; set; }
        public string AgreementType { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public string SOABahiNo { get; set; }
        public string SOABahiZildNo { get; set; }
        public string SOABahiPageNo { get; set; }
        public string SOASINo { get; set; }
        public string AdvertismentInNewsPaper { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
    }
}
