using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GatePass.ViewModels
{
    public class MenuObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<MenuObject> Items { get; set; }
        public bool IsActive { get; set; }
        public string URL { get; set; }
        public int ParentId { get; set; }
    }
}