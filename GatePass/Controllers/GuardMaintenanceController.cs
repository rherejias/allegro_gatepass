using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class GuardMaintenanceController : Controller
    {

        private Dictionary<string, object> response = new Dictionary<string, object>();
        private CustomHelper custom_helper = new CustomHelper();
        private GuardMaintenanceModels guardModels = new GuardMaintenanceModels();
        private GuardObject guardObj = new GuardObject();
        public string test { get; set; }
        // GET: /Maintenance/

        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : for getting guard data
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetGuardData()
        {
            try
            {

                string IsActive = Request["IsActive"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                Dictionary<string, object> result_config = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = guardModels.GetGuardData_cols();

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
                    int totalRows = guardModels.GetGuardData_count(where, IsActive);

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
                        transactions = guardModels.GetGuardData(0, 0, where, sorting, IsActive);
                    }
                    else
                    {
                        transactions = guardModels.GetGuardData(start, pagesize, where, sorting, IsActive);
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
                    result_config.Add("TotalRows", guardModels.GetGuardData_count(where, IsActive));
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



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : addition of new guard user account
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public JsonResult Add()
        {
            try
            {
                guardObj.username = Request["username"].ToString();
                guardObj.password = Request["password"].ToString();
                guardObj.givenName = Request["givenname"].ToString();
                guardObj.lastName = Request["lastname"].ToString();

                bool IsExisst = guardModels.Add(guardObj);

                if (IsExisst == true)
                {
                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", "Guard profile added.");
                }
                else
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", "User already exist.");
                }
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
        /// @description : edit guard user account
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public JsonResult Edit()
        {
            try
            {
                string uname = guardModels.usernameIsExist(Request["username"].ToString());
                string fname = guardModels.getGivenName(Request["code"].ToString());

                guardObj.Id = Convert.ToInt32(Request["id"]);
                guardObj.code = Request["code"].ToString();
                guardObj.username = Request["username"].ToString();
                guardObj.password = Request["password"].ToString();
                guardObj.givenName = Request["givenname"].ToString();
                guardObj.lastName = Request["lastname"].ToString();

                if (fname == Request["username"].ToString())
                {
                    bool IsExisst = guardModels.Edit(guardObj);
                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", "Guard profile updated.");
                }
                else
                {
                    if (uname != "1")
                    {
                        bool IsExisst = guardModels.Edit(guardObj);
                        response.Add("success", true);
                        response.Add("error", false);
                        response.Add("message", "Guard profile updated.");
                    }
                    else
                    {
                        response.Add("success", false);
                        response.Add("error", true);
                        response.Add("message", "User already exist.");
                    }

                }


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
        /// @description : deactivate guard user account
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public JsonResult Deactivate()
        {
            try
            {
                guardObj.Id = Convert.ToInt32(Request["id"]);
                guardObj.code = Request["code"].ToString();

                bool IsExisst = guardModels.Deactivate(guardObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "Guard profile deactivated.");
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
        /// @description : activate guard user account
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public JsonResult Activate()
        {
            try
            {
                guardObj.Id = Convert.ToInt32(Request["id"]);
                guardObj.code = Request["code"].ToString();

                bool IsExisst = guardModels.Activate(guardObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "Guard profile activated.");
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