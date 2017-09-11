using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class SupplierController : Controller
    {
        private Dictionary<string, object> response = new Dictionary<string, object>();

        private CustomHelper custom_helper = new CustomHelper();
        private SupplierModels supplier_model = new SupplierModels();
        private SupplierObject supplierObj = new SupplierObject();
        private UploaderHelper uploader_helper = new UploaderHelper();


        public ActionResult Supplier()
        {
            return View();
        }




        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get all the supplier record
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetSuppliers()
        {
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = supplier_model.GetSuppliers_cols();

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
                int totalRows = supplier_model.GetSuppliers_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));

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
                    transactions = supplier_model.GetSuppliers(0, 0, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
                }
                else
                {
                    transactions = supplier_model.GetSuppliers(start, pagesize, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
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
                result_config.Add("TotalRows", supplier_model.GetSuppliers_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString())));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : add new supplier details
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [HttpGet]
        public JsonResult AddSupplier()
        {
            try
            {
                int zip = 0;
                if (Request["Zip"].ToString() != "")
                {
                    zip = Convert.ToInt32(Request["Zip"]);
                }
                supplierObj.Name = Request["Name"].ToString();
                supplierObj.Email = Request["Email"].ToString();
                supplierObj.ContactNbr = Request["ContactNbr"].ToString();
                supplierObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                supplierObj.UnitNbr = Request["UnitNbr"].ToString();
                supplierObj.StreetName = Request["StreetName"].ToString();
                supplierObj.Municipality = Request["Municipality"].ToString();
                supplierObj.City = Request["City"].ToString();
                supplierObj.Country = Request["Country"].ToString();
                supplierObj.Zip = zip;
                supplierObj.ImpexRefNbr = Request["ImpexRefNbr"].ToString();
                supplierObj.IsActive = true;
                supplierObj.DateAdded = DateTime.Now;

                supplier_model.SaveNewSupplier(supplierObj);

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: "");
            }
            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : update supplier details
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [HttpGet]
        public JsonResult UpdateSupplier()
        {
            try
            {
                int zip = 0;
                if (Request["Zip"].ToString() != "")
                {
                    zip = Convert.ToInt32(Request["Zip"]);
                }
                supplierObj.Id = Convert.ToInt32(Request["Id"]);
                supplierObj.Code = Request["code"].ToString();
                supplierObj.Name = Request["Name"].ToString();
                supplierObj.Email = Request["Email"].ToString();
                supplierObj.ContactNbr = Request["ContactNbr"].ToString();
                supplierObj.UnitNbr = Request["UnitNbr"].ToString();
                supplierObj.StreetName = Request["StreetName"].ToString();
                supplierObj.Municipality = Request["Municipality"].ToString();
                supplierObj.City = Request["City"].ToString();
                supplierObj.Country = Request["Country"].ToString();
                supplierObj.Zip = zip;
                supplierObj.ImpexRefNbr = Request["ImpexRefNbr"].ToString();
                supplierObj.DateAdded = DateTime.Now;
                supplierObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                supplier_model.UpdateSupplier(supplierObj);

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: "");
            }
            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : deactivate supplier details
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [HttpGet]
        public JsonResult DeactivateSupplier()
        {
            try
            {
                supplierObj.Id = Convert.ToInt32(Request["Id"]);
                supplierObj.Code = Request["code"].ToString();
                supplierObj.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                supplierObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                supplierObj.DateAdded = DateTime.Now;

                supplier_model.DeactivateSupplier(supplierObj);

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
        /// @author rherejias 2/27/2017 
        /// @description : generate excel file
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [HttpPost]
        public JsonResult TargetUpload()
        {
            //HttpContext.Server.ScriptTimeout = 300;
            Dictionary<string, object> res = new Dictionary<string, object>();

            //get upload directory from web config file
            string uploadDir = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_upload_target"].ToString();
            res = uploader_helper.Uploader(Request.Files, uploadDir, Request.Browser.Browser.ToUpper());
            foreach (string file in res["files"] as List<string>)
            {
                response = uploader_helper.UploadTargets(file);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult export()
        {
            supplier_model.export(Request["operation"].ToString(), Request["table"].ToString());
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult upload()
        {
            supplier_model.upload();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #region GetSuppliersForCombo
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 12, 2017
         * @description     :   return json of suppliers
         */
        public JsonResult GetSuppliersForCombo()
        {
            try
            {
                DataTable dt = supplier_model.GetSuppliersForCombo();

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