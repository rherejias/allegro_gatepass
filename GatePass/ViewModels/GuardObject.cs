using System;

namespace GatePass.ViewModels
{
    public class GuardObject
    {
        public int Id { get; set; }
        public string code { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string givenName { get; set; }
        public string lastName { get; set; }
        public bool IsActive { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public string deptname { get; set; }
        public string description { get; set; }

    }


}