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
    
    public partial class SpAccountLedger_Result
    {
        public int Id { get; set; }
        public Nullable<int> Rid { get; set; }
        public string RECEIPT_SUB_HEAD { get; set; }
        public Nullable<System.DateTime> Entry_Date { get; set; }
        public decimal Debit_Amount { get; set; }
        public decimal Credit_Amount { get; set; }
        public Nullable<decimal> Balance_Amount { get; set; }
    }
}