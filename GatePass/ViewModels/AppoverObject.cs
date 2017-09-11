using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.ViewModels
{
    public class AppoverObject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ApprovalTypeCode { get; set; }
        public string DepartmentCode { get; set; }
        public string UserCode { get; set; }
        public DateTime? DateAdded { get; set; }
        public int AddedBy { get; set; }
        public bool IsActive { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
        public string  Module { get; set; }
    }
}