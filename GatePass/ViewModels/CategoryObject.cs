using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.Models
{
    public class CategoryObject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
    }
}