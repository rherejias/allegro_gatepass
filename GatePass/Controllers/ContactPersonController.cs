using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class ContactPersonController : Controller
    {
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private CustomHelper custom_helper = new CustomHelper();
        private ContactPersonModels ContactPerson_model = new ContactPersonModels();
        private ContactPersonObject contactpersonObj = new ContactPersonObject();



        public ActionResult Supplier()
        {
            return View();
        }




        /// <summary>
        /// author: rherejias@allegromicro.com
        /// description : Get all supplier record from table
        /// version : 1.0
        /// </summary>
        /// <returns></returns>  
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetSuppliers()
        {
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = ContactPerson_model.GetSuppliers_cols();

            //check for filters
            string where = "";
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
            {
                for (int i = 0; i < Int32.Parse(Request["filterscount"]); i++)
                {
                    filters.Add("filtervalue" + i, Request["filtervalue" + i]);
                    filters.Add("filtercondition" + i, Request["filtercondition" + i]);
                    filters.Add("filterdatafield" + i, Request["filterdatafield" + i]);
                    filters.Add("filteroperator" + i, Request["filteroperator" + i]);
                }
                where = custom_helper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
            }

            //check for sorting ops
            string sorting = "";
            if (Request["sortdatafield"] != null)
            {
                sorting = Request["sortdatafield"].ToString() + " " + Request["sortorder"].ToString().ToUpper();
            }

            //determine if cols_only
            if (Request["cols_only"] != null && bool.Parse(Request["cols_only"]) == true)
            {
                //get total row count
                int totalRows = ContactPerson_model.GetSuppliers_count(where);

                //prepare column config
                var cols = new List<string>();
                foreach (var item in columns)
                {
                    cols.Add(item.Value);
                }

                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", totalRows);
            }
            else
            {
                //pagination initialization
                int pagenum = Request["pagenum"] == null ? 0 : Int32.Parse(Request["pagenum"].ToString());
                int pagesize = Request["pagesize"] == null ? 0 : Int32.Parse(Request["pagesize"].ToString());
                int start = pagenum * pagesize;

                //get data
                DataTable transactions = new DataTable();
                if (Request["showAll"] != null && bool.Parse(Request["showAll"]) == true)
                {
                    transactions = ContactPerson_model.GetSuppliers(0, 0, where, sorting);
                }
                else
                {
                    transactions = ContactPerson_model.GetSuppliers(start, pagesize, where, sorting);
                }

                //convert data into json object
                var data = custom_helper.DataTableToJson(transactions);

                result_config.Add("data", data);

                //prepare column config
                var cols = new List<string>();
                foreach (DataColumn column in transactions.Columns)
                {
                    cols.Add(column.ColumnName);
                }
                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", ContactPerson_model.GetSuppliers_count(where));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// @author: rherejias
        /// @desc: to add contact details 
        /// @version : 1.0
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AddContactPerson()
        {
            try
            {
                contactpersonObj.SupplierKey = Request["SupplierKey"].ToString();
                contactpersonObj.FirstName = Request["FirstName"].ToString();
                contactpersonObj.MiddleName = Request["MiddleName"].ToString();
                contactpersonObj.LastName = Request["LastName"].ToString();
                contactpersonObj.Email = Request["Email"].ToString();
                contactpersonObj.ContactNumber = Request["ContactNumber"].ToString();
                contactpersonObj.Department = Request["Department"].ToString();
                contactpersonObj.DateAdded = DateTime.Now;
                contactpersonObj.IsActive = true;
                contactpersonObj.AddedBy = Convert.ToInt32(Session["userId_local"]);


                int ctr = ContactPerson_model.IsContactPersonExistToSupplier(Request["SupplierKey"].ToString(), Request["LastName"].ToString(), Request["FirstName"].ToString(), Request["MiddleName"].ToString());


                if (ctr == 0)
                {

                    ContactPerson_model.SaveNewContactPerson(contactpersonObj);
                    response.Add(key: "success", value: true);
                    response.Add(key: "error", value: false);
                    response.Add(key: "message", value: "");
                }
                else
                {
                    response.Add(key: "success", value: false);
                    response.Add(key: "error", value: true);
                    response.Add(key: "message", value: "");
                }

            }
            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }






        /// <summary>
        /// @author: rherejias
        /// @desc: update contact details 
        /// @version : 1.0
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult UpdateContactPerson()
        {
            try
            {
                contactpersonObj.Id = Convert.ToInt32(Request["Id"]);
                contactpersonObj.Code = Request["Code"].ToString();
                contactpersonObj.FirstName = Request["FirstName"].ToString();
                contactpersonObj.MiddleName = Request["MiddleName"].ToString();
                contactpersonObj.LastName = Request["LastName"].ToString();
                contactpersonObj.Email = Request["Email"].ToString();
                contactpersonObj.ContactNumber = Request["ContactNumber"].ToString();
                contactpersonObj.Department = Request["Department"].ToString();
                contactpersonObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                contactpersonObj.DateAdded = DateTime.Now;
                //contactpersonObj.IsActive = Convert.ToBoolean(Request["IsActive"].ToString());

                ContactPerson_model.UpdateContactPerson(contactpersonObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// @author: rherejias
        /// @desc: deactivate contact details 
        /// @version : 1.0
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DeactivateContact()
        {
            try
            {
                contactpersonObj.Id = Convert.ToInt32(Request["Id"]);
                contactpersonObj.Code = Request["code"].ToString();
                contactpersonObj.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                contactpersonObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                contactpersonObj.DateAdded = DateTime.Now;

                ContactPerson_model.DeactivateContact(contactpersonObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region GetContactPersonsForCombo
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 12, 2017
         * @description     :   return json of contact persons
         */
        [HttpGet]
        public JsonResult GetContactPersonsForCombo()
        {
            try
            {
                string fk = Request["fk"] == null ? string.Empty : Request["fk"].ToString();
                DataTable dt = ContactPerson_model.GetContactPersonsForCombo(fk);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", custom_helper.DataTableToJson(dt));
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetContactPerson
        /*
         * @author          :  AVILLENA <avillena@allegromicro.com>
         * @date            :   JAN 123 2017
         * @description     :   return json of contact persons
         */
        [HttpGet]
        public JsonResult GetContactPerson()
        {
            try
            {
                DataTable dt = ContactPerson_model.GetContactPerson();

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", custom_helper.DataTableToJson(dt));
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}