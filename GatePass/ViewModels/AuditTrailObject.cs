using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.ViewModels
{
    public class AuditTrailObject
    {
        public int Id { get; set; }
        public string ProjectCode { get; set; }
        public string Module { get; set; }
        public string Operation { get; set; }
        public string Object { get; set; }
        public int ObjectId { get; set; }
        public string ObjectCode { get; set; }
        public int UserCode { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}