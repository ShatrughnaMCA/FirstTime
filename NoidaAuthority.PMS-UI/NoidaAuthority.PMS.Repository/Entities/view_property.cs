//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoidaAuthority.PMS.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class view_property
    {
        [Key]
        public string PRDVREGISTRATION_ID { get; set; }
        public Nullable<int> PLDIPROPERTY_ID { get; set; }
        public string PLDIPROPERTY_NO { get; set; }
        public string BLOCK { get; set; }
        public string PLDVSECTOR_ID { get; set; }
        public string PLDVSUBSCHEME_ID { get; set; }
        public string SECTOR { get; set; }
        public string PLDVDISP_PROP_TYPE_ID { get; set; }
        public string PLDVDISP_PROP_SUBTYPE_ID { get; set; }
        public string PLDVLAND_USE_TYPE_ID { get; set; }
        public string PropertyType { get; set; }
        public string PRDVREGIST_APPLICANT_NAME { get; set; }
        public string PRDVAPPLICANT_FATHER_NAME { get; set; }
        public string PRDVREGIST_APPLICANT_ADDRESS { get; set; }
        public string PRDVSUCC_MUT_TRANS_FLAG { get; set; }
        public Nullable<double> PLDRACTUAL_LAND_AREA { get; set; }
        public string AreaSqm { get; set; }
        public int AreaId { get; set; }
        public Nullable<System.DateTime> PRDDALLOTMENT_DATE { get; set; }
        public Nullable<System.DateTime> PRDDACTUAL_FUNCTIONAL_DATE { get; set; }
        public string PRDVPRODUCT_NAME { get; set; }
        public Nullable<System.DateTime> PRDDREGISTRY_DONE_DATE { get; set; }
        public Nullable<System.DateTime> PRDDPOSSESSION_DATE { get; set; }
        public Nullable<System.DateTime> PRDDMUTATION_DATE { get; set; }
        public string STATUS { get; set; }
        public string AllotmentStatus { get; set; }
        public int AllotmentStatusId { get; set; }
        public string PlotNumber { get; set; }
        public Nullable<int> Prop_Type { get; set; }
        public Nullable<int> ConstructionPeriodAllowed { get; set; }
        public string mobile_no { get; set; }
        public string PLDVLAND_USE_SUBTYPE_ID { get; set; }
        public string Expr1 { get; set; }
    }
}