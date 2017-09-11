using System;

namespace GatePass.ViewModels
{
    public class TransactionHeaderObject
    {

        public int Id { get; set; }
        public string SessionId { get; set; }
        public string Code { get; set; }
        public string ImpexRefNbr { get; set; }
        public string SupplierCode { get; set; }
        public string ContactPersonCode { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime ReturnDate { get; set; }
        public string TransType { get; set; }
        public string CategoryCode { get; set; }
        public string TypeCode { get; set; }
        public string Purpose { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public string ReturnSlipStatus { get; set; }
        public string IP { get; internal set; }
        public string MAC { get; internal set; }

        public string Attachment { get; set; }

        public string ReturnSlipAttachment { get; set; }
        public string UserCode { get; set; }
        public string Comment { get; set; }
        public string DepartmentApproverCode { get; set; }


    }
}