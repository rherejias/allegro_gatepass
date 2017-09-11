using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.ViewModels
{
    public class DepartmentObject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Source { get; set; }
        public string AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
    }
}