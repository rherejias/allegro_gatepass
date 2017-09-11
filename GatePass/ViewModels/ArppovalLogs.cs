using System;

namespace GatePass.ViewModels
{
    public class ArppovalLogs
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string TransHeaderCode { get; set; }
        public string ApprovalTypeCode { get; set; }
        public bool IsApprovedBy { get; set; }
        public DateTime DateModified { get; set; }
        public string ToBeApprovedBy { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public Int32 AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public string Remarks { get; set; }
        public bool IsOverride { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedDate { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }

    }
}