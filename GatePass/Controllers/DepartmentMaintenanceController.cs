using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class DepartmentMaintenanceController : Controller
    {

        private readonly Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly CustomHelper customHelper = new CustomHelper();
        private readonly DepartmentMaintenanceModel departmentMaintenance = new DepartmentMaintenanceModel();
        private readonly GuardObject guardObj = new GuardObject();

        // GET: DepartmentMaintenance
        public ActionResult Index()
        {
            return View();
        }
        // GET: /Maintenance/

        /// <summary>
        /// @author avillena 06/06/2017
        /// @description : for getting guard data
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetDepartmentData()
        {
            try
            {

                string isActive = Request["IsActive"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = departmentMaintenance.GetGuardDataCols();

                //check for filters
                string where = "";
                var filters = new Dictionary<string, string>();
                if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
                {
                    for (int i = 0; i < Int32.Parse(Request["filterscount"]); i++)
                    {
                        filters.Add("filtervalue" + i, Request["filtervalue" + i]);
                        filters.Add("filtercondition" + i, Request["filtercondition" + i]);
                        filters.Add("filterdatafield" + i, Request["filterdatafield" + i]);
                        filters.Add("filteroperator" + i, Request["filteroperator" + i]);
                    }
                    where = customHelper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
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
                    int totalRows = departmentMaintenance.GetGuardDataCount(where, isActive);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (var item in columns)
                    {
                        cols.Add(item.Value);
                    }

                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: totalRows);
                }
                else
                {
                    //pagination initialization
                    int pagenum = Request["pagenum"] == null ? 0 : Int32.Parse(Request["pagenum"].ToString());
                    int pagesize = Request["pagesize"] == null ? 0 : Int32.Parse(Request["pagesize"].ToString());
                    int start = pagenum * pagesize;

                    //get data
                    var transactions = new DataTable();
                    if (Request["showAll"] != null && bool.Parse(Request["showAll"]) == true)
                    {
                        transactions = departmentMaintenance.GetGuardData(0, 0, where, sorting, isActive);
                    }
                    else
                    {
                        transactions = departmentMaintenance.GetGuardData(start, pagesize, where, sorting, isActive);
                    }

                    //convert data into json object
                    var data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add(key: "data", value: data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: departmentMaintenance.GetGuardDataCount(where, isActive));
                }

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: resultConfig);

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
        /// @author avillena 06/06/2017
        /// @description : addition of new department
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult AddDept()
        {
            try
            {
                guardObj.deptname = Request["deptname"].ToString();
                guardObj.description = Request["description"].ToString();


                bool isExisst = departmentMaintenance.Add(guardObj);

                if (isExisst == true)
                {
                    response.Add(key: "success", value: true);
                    response.Add(key: "error", value: false);
                    response.Add(key: "message", value: "New Department Added.");
                }
                else
                {
                    response.Add(key: "success", value: false);
                    response.Add(key: "error", value: true);
                    response.Add(key: "message", value: "Department Already Exist.");
                }
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
        /// @author avillena 06/06/2017
        /// @description : edit department list
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>

        public JsonResult DeptEdit()
        {
            try
            {
                string deptnameCount = departmentMaintenance.DepartmentExist(Request["deptname"].ToString());
                string deptname = departmentMaintenance.GetDeptName(Request["code"].ToString());

                guardObj.Id = Convert.ToInt32(Request["id"]);
                guardObj.code = Request["code"].ToString();
                guardObj.deptname = Request["deptname"].ToString();
                guardObj.description = Request["description"].ToString();







                if (deptname == Request["deptname"].ToString())
                {
                    bool isExisst = departmentMaintenance.Edit(guardObj);
                    response.Add(key: "success", value: true);
                    response.Add(key: "error", value: false);
                    response.Add(key: "message", value: "Department Updated");
                }
                else
                {
                    if (deptnameCount != "1")
                    {
                        bool isExisst = departmentMaintenance.Edit(guardObj);
                        response.Add(key: "success", value: true);
                        response.Add(key: "error", value: false);
                        response.Add(key: "message", value: "Department Updated");
                    }
                    else
                    {
                        response.Add(key: "success", value: false);
                        response.Add(key: "error", value: true);
                        response.Add(key: "message", value: "Department Already Exist.");
                    }

                }


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
        /// @author avillena 06/06/2017
        /// @description : deactivate department list
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public JsonResult Deactivate()
        {
            try
            {
                guardObj.Id = Convert.ToInt32(Request["id"]);
                guardObj.code = Request["code"].ToString();
                guardObj.IsActive = false;
                guardObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                guardObj.DateAdded = DateTime.Now;

                bool isExisst = departmentMaintenance.Deactivate(guardObj);

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: "Department Deactivated.");
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
        /// @author avillena 06/06/2017
        /// @description : activate department list
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public JsonResult Activate()
        {
            try
            {
                guardObj.Id = Convert.ToInt32(Request["id"]);
                guardObj.code = Request["code"].ToString();

                bool isExisst = departmentMaintenance.Activate(guardObj);

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: "Department Activated.");
            }
            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}