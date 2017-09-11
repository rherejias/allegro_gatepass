using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class ApproverTypeController : Controller
    {
        private CustomHelper custom_helper = new CustomHelper();
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private ApproverTypeModels approverType_model = new ApproverTypeModels();
        private ApproverTypeObject approverTypeObj = new ApproverTypeObject();
        // GET: ApproverType


        /// <summary>
        /// author: rherejias@allegromicro.com
        /// description : Approver type maintenance view
        /// version : 1.0
        /// </summary>
        /// <returns>view</returns>
        public ActionResult ApproverType()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(17, 7, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Approver Type";
                ViewBag.PageHeader = "Approver Type";
                ViewBag.Breadcrumbs = "Maintenance / Approver Type";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }
        }



        /// <summary>
        /// author: rherejias@allegromicro.com
        /// description : Get the approver type if its Department, IT Purchasing and Accounting
        /// version : 1.0
        /// </summary>
        /// <returns>view</returns>
        [HttpGet]
        public JsonResult GetApproverType()
        {
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = approverType_model.GetApproverType_cols();

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
                int totalRows = approverType_model.GetApproverType_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));

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
                    transactions = approverType_model.GetApproverType(0, 0, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
                }
                else
                {
                    transactions = approverType_model.GetApproverType(start, pagesize, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
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
                result_config.Add("TotalRows", approverType_model.GetApproverType_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString())));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// author: rherejias@allegromicro.com
        /// description : rherejias 1/5/2017 for approverType addtion
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult addApproverType()
        {
            try
            {
                approverTypeObj.Name = Request["approvalType"].ToString();
                approverTypeObj.Description = Request["approvalDescription"].ToString();
                approverTypeObj.IsActive = true;
                approverTypeObj.DateAdded = DateTime.Now;

                approverType_model.addApproverType(approverTypeObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "Record added successfully!");

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
        /// author: rherejias@allegromicro.com
        /// description : rherejias 1/5/2017 for approverType update
        /// version : 1.0
        /// </summary>
        /// <returns></returns>        
        [HttpGet]
        public JsonResult EditApproverType()
        {
            try
            {
                approverTypeObj.Code = Request["code"].ToString();
                approverTypeObj.Name = Request["approvalType"].ToString();
                approverTypeObj.Description = Request["approvalDescription"].ToString();

                approverType_model.EditApproverType(approverTypeObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "Record update successfully!");

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
        /// author: rherejias@allegromicro.com
        /// description : rherejias 1/5/2017 for approverType Inactive
        /// version : 1.0
        /// </summary>
        /// <returns></returns>  
        [HttpGet]
        public JsonResult InactiveApproverType()
        {
            try
            {
                approverTypeObj.Id = Convert.ToInt32(Request["id"]);
                approverTypeObj.Code = Request["code"].ToString();

                approverType_model.InactiveApproverType(approverTypeObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "Record deactivation successfully!");

            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}