using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class ITPurchasingController : Controller
    {
        // GET: ITPurchasing
        private Dictionary<string, object> response = new Dictionary<string, object>();

        private CustomHelper custom_helper = new CustomHelper();
        private ITPurchasingModel ITPurchasing = new ITPurchasingModel();
        private AppoverObject approverObj = new AppoverObject();
        private UploaderHelper uploader_helper = new UploaderHelper();
        public string test { get; set; }
        // GET: /Maintenance/




        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get all the IT and Purchasing approver
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetITPurchasingApprovers()
        {
            string ApproverType = Request["ApproverType"].ToString();
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = ITPurchasing.GetITPurchasingApprovers_cols();

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
                int totalRows = ITPurchasing.GetITPurchasingApprovers_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), ApproverType);

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
                    transactions = ITPurchasing.GetITPurchasingApprovers(0, 0, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), ApproverType);
                }
                else
                {
                    transactions = ITPurchasing.GetITPurchasingApprovers(start, pagesize, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), ApproverType);
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
                result_config.Add("TotalRows", ITPurchasing.GetITPurchasingApprovers_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), ApproverType));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}