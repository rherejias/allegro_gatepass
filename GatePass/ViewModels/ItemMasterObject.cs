using System;

namespace GatePass.Models
{
    public class ItemMasterObject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierContactCode { get; set; }
        public string ItemCategoryCode { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemDepRelCode { get; set; }
        public string ItemUOM { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
    }
}