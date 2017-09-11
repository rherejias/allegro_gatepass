using System;

namespace GatePass.ViewModels
{
    public class Details
    {

        public int Id { get; set; }
        public string SessionId { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public string SerialNbr { get; set; }
        public string CategoryCode { get; set; }
        public string ItemTypeCode { get; set; }
        public string TagNbr { get; set; }
        public string PONbr { get; set; }

        public bool IsActive { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public string Remarks { get; set; }
        public string ReturnSlipStatus { get; set; }

        public string HeaderCode { get; set; }
        public string Action { get; internal set; }

        public string Image { get; set; }


        //public string Supplier { get; set; }
        //public string Item { get; set; }
        //public string UnitofMeasure { get; set; }
        //public string Category { get; set; }
        //public string Type { get; set; }

    }
}