using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System.Data;
using System.Configuration;

namespace GatePass.Controllers
{
    public class InboxController : Controller
    {
        private readonly CustomHelper customHelper = new CustomHelper();
        private readonly Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly SupplierModels supplierModel = new SupplierModels();
        private readonly TransactionModels transModel = new TransactionModels();
        private readonly InboxModel inboxModel = new InboxModel();
        private readonly TransactionHeaderObject transHeaderObj = new TransactionHeaderObject();
        private readonly Details transAddItems = new Details();
        private readonly ItemMasterObject itemMasterObj = new ItemMasterObject();
        private readonly UploaderHelper uploaderHelper = new UploaderHelper();

        /// <summary>
        /// description: display 'my inbox' page
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        public ActionResult InboxView()
        {

            try
            {
                ViewBag.Menu = customHelper.PrepareMenu(child: 14, parent: 0, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "My Inbox";
                ViewBag.PageHeader = "My Inbox";
                //ViewBag.Breadcrumbs = "Home / Gate Pass";

                string userdept = Session["department"].ToString();
                ViewBag.deptapprover = transModel.Getdeptapprover(userdept);
                return View();

            }
            catch (Exception)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }

          
        }




        /// <summary>
        /// description: get and display gate pass record with status 'with correction'
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        #region GetWithIncompleteTrans
        [HttpGet]
        public JsonResult GetWithIncompleteTrans()
        {
            //create a dictionary that will contain the column + datafied config for the grid
            var resultConfig = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = inboxModel.GetAllTransCols();

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
                int totalRows = inboxModel.GetAllTransCount(where);

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
                    transactions = inboxModel.GetAllTrans(offset: 0, next: 0, where: where, sorting: sorting);
                }
                else
                {
                    transactions = inboxModel.GetAllTrans(start, pagesize, where, sorting);
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
                resultConfig.Add(key: "TotalRows", value: inboxModel.GetAllTransCount(where));
            }

            response.Add(key: "success", value: true);
            response.Add(key: "error", value: false);
            response.Add(key: "message", value: resultConfig);

            return Json(response, JsonRequestBehavior.AllowGet);
        }//End
        #endregion





        /// <summary>
        /// description: get and display item details of gatepass with correction
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        #region GetItemDetails_Inbox
        [HttpGet]
        public JsonResult GetItemDetails_Inbox()
        {
            //create a dictionary that will contain the column + datafied config for the grid
            var resultConfig = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = inboxModel.GetItemDetailsInboxCols();

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

            string headerKey = Request["HeaderKey"];
            if (String.IsNullOrEmpty(headerKey))
            {
                if (where == "")
                {
                    where = " WHERE SessionId = '" + Session.SessionID + "'";
                }
                else
                {
                    where += " AND SessionId = '" + Session.SessionID + "'";
                }
            }
            else
            {
                if (where == "")
                {
                    where = " WHERE HeaderCode = '" + headerKey + "'";
                }
                else
                {
                    where += " AND HeaderCode = '" + headerKey + "'";
                }
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
                int totalRows = inboxModel.GetItemDetailsInboxCount(where, headerKey);

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
                    transactions = inboxModel.GetTransDetails(offset: 0, next: 0, where: where, sorting: sorting, headerKey: headerKey);
                }
                else
                {
                    transactions = inboxModel.GetTransDetails(start, pagesize, where, sorting, headerKey);
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
                resultConfig.Add(key: "TotalRows", value: inboxModel.GetItemDetailsInboxCount(where, headerKey));
            }

            response.Add(key: "success", value: true);
            response.Add(key: "error", value: false);
            response.Add(key: "message", value: resultConfig);

            return Json(response, JsonRequestBehavior.AllowGet);
        }// End
        #endregion




    }
}