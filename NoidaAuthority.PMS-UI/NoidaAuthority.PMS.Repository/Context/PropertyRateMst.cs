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
    
    public partial class PropertyRateMst
    {
        public int Id { get; set; }
        public Nullable<int> SchemeId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BLockId { get; set; }
        public string SchemeName { get; set; }
        public Nullable<int> PropertyTypeId { get; set; }
        public string AreaRange { get; set; }
        public string AllotmentYear { get; set; }
        public Nullable<decimal> PropertyRate { get; set; }
        public Nullable<decimal> NormalInterest { get; set; }
        public Nullable<decimal> PenalInterest { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Phase { get; set; }
        public Nullable<decimal> PlotPremium { get; set; }
        public string AllotmentMethod { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string SectorName { get; set; }
        public Nullable<decimal> TransferRateInPercent { get; set; }
        public Nullable<decimal> TransferRateInINR { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Comment { get; set; }
    }
}
