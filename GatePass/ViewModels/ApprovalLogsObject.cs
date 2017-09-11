using System;

namespace GatePass.ViewModels
{
    public class ApprovalLogsObject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string TransHeaderCode { get; set; }
        public string ApprovalTypeCode { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateModified { get; set; }
        public string ToBeApprovedBy { get; set; }
        public int ApprovedBy { get; set; }
        public int RejectedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public DateTime DateRejected { get; set; }
        public Int32 AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public string Remarks { get; set; }
        public bool IsOverride { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }


    }
}