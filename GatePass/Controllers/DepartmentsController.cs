using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GatePass.Controllers
{
    public class DepartmentsController : Controller
    {
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private DepartmentModels dept_mod = new DepartmentModels();
        private CustomHelper custom_helper = new CustomHelper();
        private DepartmentModels department_model = new DepartmentModels();
        private DepartmentObject departmentObj = new DepartmentObject();
        // GET: /Maintenance/




        #region UpdateDepartmentsTable
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 4, 2016 5:16 PM
         * @description     :   get all departments from AD and insert it into a local table
         */
        public JsonResult UpdateDepartmentsTable()
        {
            try
            {
                //prepare client handlers for endpoint
                HttpClientHandler hndlr = new HttpClientHandler();
                hndlr.UseDefaultCredentials = true;
                HttpClient client = new HttpClient(hndlr);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //specify the endpoint URL
                var url = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_api_base_url"].ToString() + "departments/ShowAll?json=true";
                HttpResponseMessage res = client.GetAsync(url).Result;

                //check if the enpoint is online
                if (!res.IsSuccessStatusCode)
                {
                    throw new Exception(res.IsSuccessStatusCode.ToString());
                }

                //get string result from endpoint
                string strJson = res.Content.ReadAsStringAsync().Result;

                //convert the string from endpoint into json object
                JavaScriptSerializer j = new JavaScriptSerializer();
                var dx = j.Deserialize<Dictionary<string, object>>(strJson);
                ArrayList msg = (ArrayList) dx["message"];

                DateTime dateadded = DateTime.Now;
                List<string> d = new List<string>();

                //loop through all the department names returned by the endpoint
                foreach (string s in msg)
                {
                    DepartmentObject dept_obj = new DepartmentObject();
                    dept_obj.Code = string.Empty;
                    dept_obj.Name = s;
                    dept_obj.Description = s;
                    dept_obj.IsActive = true;
                    dept_obj.Source = "AD";
                    dept_obj.AddedBy = "0";
                    dept_obj.DateAdded = dateadded;

                    //add it to the database
                    dept_mod.UpSertDepartment(dept_obj);

                    d.Add(s);
                }

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", d);
            }
            catch (Exception ex)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", ex.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion





        /// <summary>
        /// @author: rherejias
        /// @desc: get all department list 
        /// @version : 1.0
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDepartment()
        {
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = department_model.GetDepartment_cols();

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
                int totalRows = department_model.GetDepartment_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));

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
                    transactions = department_model.GetDepartment(0, 0, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
                }
                else
                {
                    transactions = department_model.GetDepartment(start, pagesize, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
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
                result_config.Add("TotalRows", department_model.GetDepartment_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString())));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}