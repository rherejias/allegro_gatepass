using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.ViewModels
{
    public class ContactPersonObject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string SupplierKey { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
    }
}