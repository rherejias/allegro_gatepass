using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.ViewModels
{
    public class SupplierObject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNbr { get; set; }
        public int AddedBy { get; set; }
        public string UnitNbr { get; set; }
        public string StreetName { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Zip { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateAdded { get; set; }
        public string Input { get; set; }
        public string ImpexRefNbr { get; set; }
    }
}