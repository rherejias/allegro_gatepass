using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.ViewModels
{
    public class ItemReturnLogsObject
    {
        public int Id { get; set; }
        public string HeaderCode { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public string UoM { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public int AddedBy { get; set; }

        public DateTime DateAdded { get; set; }

        public string GUID { get; set; }
    }
}