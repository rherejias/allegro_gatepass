using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class AuditTrailController : Controller
    {
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private CustomHelper custom_helper = new CustomHelper();
        private AuditTrailModels audittrail_model = new AuditTrailModels();
        private AuditTrailObject audittrailObj = new AuditTrailObject();
        private UploaderHelper uploader_helper = new UploaderHelper();
        // GET: AuditTrail


        /// <summary>
        /// author: rherejias@allegromicro.com
        /// description : Audit trail page view
        /// version : 1.0
        /// </summary>
        /// <returns></returns>  
        public ActionResult Audit()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(0, 16, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Audit Trail";
                ViewBag.PageHeader = "Audit Trail";
                ViewBag.Breadcrumbs = "Audit Trail";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }
        }




        /// <summary>
        /// author: rherejias@allegromicro.com
        /// description : get all the record of audit trail from table
        /// version : 1.0
        /// </summary>
        /// <returns></returns>  
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAuditTrail()
        {
            try
            {
                //create a dictionary that will contain the column + datafied config for the grid
                Dictionary<string, object> result_config = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = audittrail_model.GetAuditTrail_cols();

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
                    int totalRows = audittrail_model.GetAuditTrail_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));

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
                        transactions = audittrail_model.GetAuditTrail(0, 0, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
                    }
                    else
                    {
                        transactions = audittrail_model.GetAuditTrail(start, pagesize, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
                    }
                    //Ver 1.0 @uathor aabasolo custom datatable for date time formatting
                    //create custom datatable
                    DataTable customDT = new DataTable();
                    // add columns to custom datatable
                    foreach (DataColumn colse in transactions.Columns)
                    {
                        // edit this to correct the right datatype (if you want)
                        customDT.Columns.Add(colse.ColumnName, typeof(string));
                    }
                    // populate custom datatable
                    foreach (DataRow row in transactions.Rows)
                    {
                        // row to be added
                        var customRow = new object[transactions.Columns.Count];
                        // row counter
                        int ctr = 0;
                        foreach (DataColumn colse in transactions.Columns)
                        {
                            // assign value to custom row
                            customRow[ctr] = row[colse].ToString();
                            ctr++;
                        }
                        // add to custom datatable the single row made above
                        customDT.Rows.Add(customRow);
                    }



                    //convert data into json object
                    var data = custom_helper.DataTableToJson(customDT);

                    result_config.Add("data", data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                    result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                    result_config.Add("TotalRows", audittrail_model.GetAuditTrail_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString())));
                }

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", result_config);
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