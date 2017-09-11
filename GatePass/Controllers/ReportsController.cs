using GatePass.Helpers;
using GatePass.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class ReportsController : Controller
    {
        private CustomHelper customHelper = new CustomHelper();
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private ReportsModels reportsModel = new ReportsModels();

        // GET: Reports
        public ActionResult Index()
        {
            try
            {
                ViewBag.Menu = customHelper.PrepareMenu(3, 0, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Reports";
                ViewBag.PageHeader = "Reports";
                ViewBag.Breadcrumbs = "Home / Reports";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }

        }

        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 13, 2017 9:30 AM
         * @description     :   using prince xml wrapper to generate pdf file (https://www.princexml.com/)
         * 
         * THIS IS JUST A TEST METHOD
         */
        #region test
        public ActionResult Foo()
        {
            try
            {
                // instantiate Prince by specifying the full path to the engine executable
                //Prince prn = new Prince("C:\\Program Files\\Prince\\Engine\\bin\\prince.exe");
                //Prince prn = new Prince("~/ThirdPartyApps/PrinceXML/prince.exe");
                //Prince prn = new Prince("\\\\4it-aabasolo\\c$\\Program Files (x86)\\Prince\\engine\\bin");
                Prince prn = new Prince(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_princexml_path"].ToString() + "prince.exe");

                // specify the log file for error and warning messages
                // make sure that you have write permissions for this file
                //prn.SetLog("C:\\docs\\log.txt");
                prn.SetLog("~/logs/Prince/log.txt");

                // apply a CSS style sheet (optional)
                prn.AddStyleSheet("~/Scripts/template/plugins/bootstrap/css/bootstrap.min.css");

                // apply an external JavaScript file (optional)
                prn.AddScript("~/Scripts/template/plugins/bootstrap/js/bootstrap.min.js");

                // convert a HTML document into a PDF file

                if (ConfigurationManager.AppSettings["env"].ToString() == "local")
                {
                    prn.Convert("~/Views/Prince/View.cshtml",
                        "~/" + ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_system_generated_files"].ToString() + "/PDF/test1.pdf");
                }
                else
                {
                    prn.Convert("~/Views/Prince/View.cshtml",
                        ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_system_generated_files"].ToString() + "/PDF/test1.pdf");
                }


                prn.Convert("~/Views/Prince/View.cshtml", "~/" + ConfigurationManager.AppSettings["system_generated_files"].ToString() + "/PDF/test1.pdf");


                // To combine multiple HTML documents into a single PDF file, call ConvertMultiple:

                //String[] doc_array = { "C:\\docs\\test1.html", "C:\\docs\\test2.html" };

                //prn.ConvertMultiple(doc_array, "C:\\docs\\pdf\\merged.pdf");

                //response.Add("success", true);
                //response.Add("error", false);
                //response.Add("message", "hello world!");
                return View("~/Views/Prince/View.cshtml");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public FileResult Bar()
        {
            //var path = System.Web.HttpContext.Current.Server.MapPath("~/SystemGeneratedFiles/PDF/test1.pdf"); ;
            //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //var stream = new FileStream(path, FileMode.Open);
            //result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //result.Content.Headers.ContentLength = stream.Length;
            //return result;

            //using (var memoryStream = new MemoryStream())
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-disposition", "attachment;  filename=helloworld.pdf");
            //    memoryStream.Equals();
            //    memoryStream.WriteTo(Response.OutputStream);
            //    Response.Flush();
            //    Response.End();
            //}
            string p = "";
            if (ConfigurationManager.AppSettings["env"].ToString() == "local")
            {
                p = "~/SystemGeneratedFiles/PDF/test1.pdf";
            }
            else
            {
                p = "D:/FTP/Gpass/SystemGeneratedFiles/PDF/test1.pdf";
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/SystemGeneratedFiles/PDF/test1.pdf"));

            string fileName = "hello_world.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion
        /// <summary>
        /// get all report data
        /// </summary>
        /// ver 1.0 rherejias 3/16/2017
        /// <returns>json </returns>
        #region get all reports
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]

        public JsonResult GetReports()
        {
            try
            {
                string userdept = Session["department"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = reportsModel.GetReports_cols();

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
                    int totalRows = reportsModel.GetReports_count(where);

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
                        transactions = reportsModel.GetReports(offset: 0, next: 0, where: where, sorting: sorting);
                    }
                    else
                    {
                        transactions = reportsModel.GetReports(start, pagesize, where, sorting);
                    }

                    //convert data into json object
                    object data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add(key: "data", value: data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: reportsModel.GetReports_count(where));
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
        #endregion

        /// <summary>
        /// get department report data
        /// </summary>
        /// ver 1.0 rherejias 3/16/2017
        /// <returns>json</returns>
        #region dept report
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetDeptReports()
        {
            try
            {
                string view = Request["view"].ToString();
                string userdept = Session["department"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = reportsModel.GetDeptReports_cols();

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
                    int totalRows = reportsModel.GetDeptReports_count(where, view);

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
                        transactions = reportsModel.GetDeptReports(offset: 0, next: 0, where: where, sorting: sorting, view: view);
                    }
                    else
                    {
                        transactions = reportsModel.GetDeptReports(start, pagesize, where, sorting, view);
                    }

                    //convert data into json object
                    object data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add(key: "data", value: data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: reportsModel.GetDeptReports_count(where, view));
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
        #endregion

        /// <summary>
        /// get IT report data
        /// </summary>
        /// ver 1.0 rherejias 3/16/2017
        /// <returns></returns>
        #region it report
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetITReports()
        {
            try
            {
                string view = Request["view"].ToString();
                string isApproved = Request["IsApproved"].ToString();
                string userdept = Session["department"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = reportsModel.GetITReports_cols();

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
                    int totalRows = reportsModel.GetITReports_count(where, isApproved, view);

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
                        transactions = reportsModel.GetITReports(offset: 0, next: 0, where: where, sorting: sorting, isApproved: isApproved, view: view);
                    }
                    else
                    {
                        transactions = reportsModel.GetITReports(start, pagesize, where, sorting, isApproved, view);
                    }

                    //convert data into json object
                    object data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add(key: "data", value: data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: reportsModel.GetITReports_count(where, isApproved, view));
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
        #endregion

        /// <summary>
        /// get purch report data
        /// </summary>
        /// ver 1.0 rherejias 3/16/2017
        /// <returns></returns>
        #region purch report
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetPurchReports()
        {
            try
            {
                string view = Request["view"].ToString();
                string isApproved = Request["IsApproved"].ToString();
                string userdept = Session["department"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = reportsModel.GetPurchReports_cols();

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
                    int totalRows = reportsModel.GetPurchReports_count(where, isApproved, view);

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
                        transactions = reportsModel.GetPurchReports(offset: 0, next: 0, where: where, sorting: sorting, IsApproved: isApproved, view: view);
                    }
                    else
                    {
                        transactions = reportsModel.GetPurchReports(start, pagesize, where, sorting, isApproved, view);
                    }

                    //convert data into json object
                    object data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add(key: "data", value: data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: reportsModel.GetPurchReports_count(where, isApproved, view));
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
        #endregion

        /// <summary>
        /// get accounting report data
        /// </summary>
        /// ver 1.0 rherejias 3/16/2017
        /// <returns></returns>
        #region accounting report
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAccountingReports()
        {
            try
            {
                string view = Request["view"].ToString();
                string isApproved = Request["IsApproved"].ToString();
                string userdept = Session["department"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = reportsModel.GetAcctReports_cols();

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
                    int totalRows = reportsModel.GetAcctReports_count(where, isApproved, view);

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
                        transactions = reportsModel.GetAcctReports(offset: 0, next: 0, where: where, sorting: sorting, IsApproved: isApproved, view: view);
                    }
                    else
                    {
                        transactions = reportsModel.GetAcctReports(start, pagesize, where, sorting, isApproved, view);
                    }

                    //convert data into json object
                    object data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add(key: "data", value: data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: reportsModel.GetAcctReports_count(where, isApproved, view));
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
        #endregion

        /// <summary>
        /// get override report data
        /// </summary>
        /// ver 1.0 rherejias 3/16/2017
        /// <returns></returns>
        #region override report
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetOverrideReports()
        {
            try
            {
                string view = Request["view"].ToString();
                string isApproved = Request["IsApproved"].ToString();
                string userdept = Session["department"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = reportsModel.GetOverrideReports_cols();

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
                    int totalRows = reportsModel.GetOverrideReports_count(where, isApproved, view);

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
                        transactions = reportsModel.GetOverrideReports(offset: 0, next: 0, where: where, sorting: sorting, IsApproved: isApproved, view: view);
                    }
                    else
                    {
                        transactions = reportsModel.GetOverrideReports(start, pagesize, where, sorting, isApproved, view);
                    }

                    //convert data into json object
                    object data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add(key: "data", value: data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: reportsModel.GetOverrideReports_count(where, isApproved, view));
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
        #endregion

        /// <summary>
        /// get override reject report data
        /// </summary>
        /// ver 1.0 rherejias 3/16/2017
        /// <returns></returns>
        #region override reject report
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetOverrideRejectReports()
        {
            try
            {
                string view = Request["view"].ToString();
                string isApproved = Request["IsApproved"].ToString();
                string userdept = Session["department"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = reportsModel.GetOverrideRejectReports_cols();

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
                    int totalRows = reportsModel.GetOverrideRejectReports_count(where, isApproved, view);

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
                        transactions = reportsModel.GetOverrideRejectReports(offset: 0, next: 0, where: where, sorting: sorting, IsApproved: isApproved, view: view);
                    }
                    else
                    {
                        transactions = reportsModel.GetOverrideRejectReports(start, pagesize, where, sorting, isApproved, view);
                    }

                    //convert data into json object
                    object data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add(key: "data", value: data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: reportsModel.GetOverrideRejectReports_count(where, isApproved, view));
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
        #endregion

        /// description : export to excel file
        /// version : 1.0
        /// author : rherejias
        [HttpGet]
        public JsonResult export()
        {
            reportsModel.export(Request["operation"].ToString(), Request["table"].ToString());
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   MARCH 10, 2017 11:25 AM
         * @description :   custom export into excel
         */
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public void ExportToExcel()
        {
            try
            {
                string export_filename = "Gatepass Report " + Request["title"].ToString();

                //initialize select clause
                string select = Request["select"].ToString();

                // initialize where clause 
                string where = Request["where"].ToString();
                string viewName = Request["viewName"].ToString();

                // initialize excel package
                ExcelPackage excel = new ExcelPackage();

                // get datatable from model
                DataTable dt = reportsModel.GetHeaderTransactions(((where == "null") ? string.Empty : where), viewName, select);

                // column headers color
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#B7DEE8");

                // starting row of the new sheet
                int startingRow = 1;

                // create new worksheet
                var workSheet_c = excel.Workbook.Worksheets.Add("Combined");

                // set column headers to bold
                workSheet_c.Cells[startingRow, 1, startingRow, dt.Columns.Count].Style.Font.Bold = true;
                workSheet_c.Cells[startingRow, 1, startingRow, dt.Columns.Count].Style.Font.Color.SetColor(Color.White);

                // exclude columns here
                // string[] exluded_columns = { "EQUIPMENT_PACKAGE_FAMILY_STRING", "PRODUCT_FAMILY_STRING", "STATUS_STRING" };
                //string[] exluded_columns = {};
                List<string> exluded_columns = new List<string>();
                //exluded_columns.Add(");

                // loop through column names and replace _ with space
                var col_ctr = 0;
                foreach (DataColumn column in dt.Columns)
                {

                    if (!exluded_columns.Contains(column.ColumnName))
                    {
                        col_ctr += 1;
                        string col_name = string.Empty;
                        if (column.ColumnName.IndexOf('_') != -1)
                        {
                            string[] col_arr = column.ColumnName.Split('_');

                            for (int x = 0; x < (col_arr.Length - 1); x++)
                            {
                                col_name += col_arr[x] + " ";
                            }
                        }
                        else
                        {
                            col_name = column.ColumnName;
                        }

                        workSheet_c.Cells[startingRow, col_ctr].Value = col_name;
                    }

                }

                // set the 2nd parameter to true to print column titles
                // auto insert datatable to excel
                //workSheet_s.Cells[startingRow + 1, 1].LoadFromDataTable(dt, false);

                // manual cell value insertion
                int row_ctr = 0;
                string fk = string.Empty;
                string loc = string.Empty;
                foreach (DataRow row in dt.Rows)
                {
                    row_ctr++;
                    col_ctr = 0;
                    fk = string.Empty;
                    foreach (DataColumn column in dt.Columns)
                    {
                        col_ctr++;
                        workSheet_c.Cells[startingRow + row_ctr, col_ctr].Value = row[column].ToString();

                        if (row["code"].ToString() != "")
                        {
                            loc = "p";
                            // check on transaction details
                            fk = row["code"].ToString();
                        }
                        else
                        {
                            loc = "t";
                            // check on transaction details temp
                            fk = row["Id"].ToString();
                        }
                    }

                    workSheet_c.Cells[startingRow + row_ctr, 1, startingRow + row_ctr, dt.Columns.Count].Style.Font.Bold = true;
                    workSheet_c.Cells[startingRow + row_ctr, 1, startingRow + row_ctr, dt.Columns.Count].Style.Font.Color.SetColor(Color.Black);
                    workSheet_c.Cells[startingRow + row_ctr, 1, startingRow + row_ctr, dt.Columns.Count - exluded_columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet_c.Cells[startingRow + row_ctr, 1, startingRow + row_ctr, dt.Columns.Count - exluded_columns.Count].Style.Fill.BackgroundColor.SetColor(colFromHex);

                    if (fk != string.Empty)
                    {
                        row_ctr++;
                        DataTable dt_details = reportsModel.GetTransactionDetails(loc, "WHERE HeaderCode = '" + fk + "';");
                        workSheet_c.Cells[startingRow + row_ctr, 1].LoadFromDataTable(dt_details, true);
                        row_ctr = row_ctr + dt_details.Rows.Count;
                    }

                }

                // set the borders
                colFromHex = System.Drawing.ColorTranslator.FromHtml("#202020");
                //workSheet_c.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Length].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //workSheet_c.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Length].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //workSheet_c.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Length].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //workSheet_c.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Length].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //workSheet_c.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Length].Style.Border.Top.Color.SetColor(colFromHex);
                //workSheet_c.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Length].Style.Border.Left.Color.SetColor(colFromHex);
                //workSheet_c.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Length].Style.Border.Right.Color.SetColor(colFromHex);
                //workSheet_c.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Length].Style.Border.Bottom.Color.SetColor(colFromHex);
                workSheet_c.Cells[startingRow, 1, startingRow, dt.Columns.Count - exluded_columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet_c.Cells[startingRow, 1, startingRow, dt.Columns.Count - exluded_columns.Count].Style.Fill.BackgroundColor.SetColor(colFromHex);

                //autofit columns
                workSheet_c.Cells[workSheet_c.Dimension.Address].AutoFitColumns();

                // create new worksheet headers
                startingRow = 1;
                col_ctr = 0;
                var workSheet_h = excel.Workbook.Worksheets.Add("Headers");
                workSheet_h.Cells[startingRow, 1, startingRow, dt.Columns.Count].Style.Font.Bold = true;
                workSheet_h.Cells[startingRow, 1, startingRow, dt.Columns.Count].Style.Font.Color.SetColor(Color.White);

                foreach (DataColumn column in dt.Columns)
                {
                    col_ctr += 1;
                    string col_name = string.Empty;
                    if (column.ColumnName.IndexOf('_') != -1)
                    {
                        string[] col_arr = column.ColumnName.Split('_');

                        for (int x = 0; x < (col_arr.Length - 1); x++)
                        {
                            col_name += col_arr[x] + " ";
                        }
                    }
                    else
                    {
                        col_name = column.ColumnName;
                    }

                    workSheet_h.Cells[startingRow, col_ctr].Value = col_name;
                }

                workSheet_h.Cells[startingRow + 1, 1].LoadFromDataTable(dt, false);

                // set the borders
                colFromHex = System.Drawing.ColorTranslator.FromHtml("#202020");
                workSheet_h.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Count].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet_h.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Count].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet_h.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Count].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet_h.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Count].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet_h.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Count].Style.Border.Top.Color.SetColor(colFromHex);
                workSheet_h.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Count].Style.Border.Left.Color.SetColor(colFromHex);
                workSheet_h.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Count].Style.Border.Right.Color.SetColor(colFromHex);
                workSheet_h.Cells[startingRow, 1, startingRow + dt.Rows.Count, dt.Columns.Count - exluded_columns.Count].Style.Border.Bottom.Color.SetColor(colFromHex);
                workSheet_h.Cells[startingRow, 1, startingRow, dt.Columns.Count - exluded_columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet_h.Cells[startingRow, 1, startingRow, dt.Columns.Count - exluded_columns.Count].Style.Fill.BackgroundColor.SetColor(colFromHex);

                //autofit columns
                workSheet_h.Cells[workSheet_c.Dimension.Address].AutoFitColumns();

                // create new worksheet details
                startingRow = 1;
                col_ctr = 0;
                DataTable dt_details_all = reportsModel.GetTransactionDetails("p", string.Empty);
                var workSheet_d = excel.Workbook.Worksheets.Add("Details");

                workSheet_d.Cells[startingRow, 1, startingRow, dt_details_all.Columns.Count].Style.Font.Bold = true;
                workSheet_d.Cells[startingRow, 1, startingRow, dt_details_all.Columns.Count].Style.Font.Color.SetColor(Color.White);

                foreach (DataColumn column in dt_details_all.Columns)
                {
                    col_ctr += 1;
                    string col_name = string.Empty;
                    if (column.ColumnName.IndexOf('_') != -1)
                    {
                        string[] col_arr = column.ColumnName.Split('_');

                        for (int x = 0; x < (col_arr.Length - 1); x++)
                        {
                            col_name += col_arr[x] + " ";
                        }
                    }
                    else
                    {
                        col_name = column.ColumnName;
                    }

                    workSheet_d.Cells[startingRow, col_ctr].Value = col_name;
                }

                workSheet_d.Cells[startingRow + 1, 1].LoadFromDataTable(dt_details_all, false);

                // set the borders
                colFromHex = System.Drawing.ColorTranslator.FromHtml("#202020");
                workSheet_d.Cells[startingRow, 1, startingRow + dt_details_all.Rows.Count, dt_details_all.Columns.Count - exluded_columns.Count].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet_d.Cells[startingRow, 1, startingRow + dt_details_all.Rows.Count, dt_details_all.Columns.Count - exluded_columns.Count].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet_d.Cells[startingRow, 1, startingRow + dt_details_all.Rows.Count, dt_details_all.Columns.Count - exluded_columns.Count].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet_d.Cells[startingRow, 1, startingRow + dt_details_all.Rows.Count, dt_details_all.Columns.Count - exluded_columns.Count].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet_d.Cells[startingRow, 1, startingRow + dt_details_all.Rows.Count, dt_details_all.Columns.Count - exluded_columns.Count].Style.Border.Top.Color.SetColor(colFromHex);
                workSheet_d.Cells[startingRow, 1, startingRow + dt_details_all.Rows.Count, dt_details_all.Columns.Count - exluded_columns.Count].Style.Border.Left.Color.SetColor(colFromHex);
                workSheet_d.Cells[startingRow, 1, startingRow + dt_details_all.Rows.Count, dt_details_all.Columns.Count - exluded_columns.Count].Style.Border.Right.Color.SetColor(colFromHex);
                workSheet_d.Cells[startingRow, 1, startingRow + dt_details_all.Rows.Count, dt_details_all.Columns.Count - exluded_columns.Count].Style.Border.Bottom.Color.SetColor(colFromHex);
                workSheet_d.Cells[startingRow, 1, startingRow, dt_details_all.Columns.Count - exluded_columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet_d.Cells[startingRow, 1, startingRow, dt_details_all.Columns.Count - exluded_columns.Count].Style.Fill.BackgroundColor.SetColor(colFromHex);

                //autofit columns
                workSheet_d.Cells[workSheet_c.Dimension.Address].AutoFitColumns();

                // use memorystream to return the excel file
                using (var memoryStream = new MemoryStream())
                {
                    // initialize the reponse variables for the browser
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + export_filename + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", ex.ToString());
            }

        }
    }
}