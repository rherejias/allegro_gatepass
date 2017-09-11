using GatePass.Helpers;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace GatePass.Controllers
{


    public class TransactionsController : Controller
    {
        private readonly CustomHelper customHelper = new CustomHelper();
        private readonly EmailHelper emailHelper = new EmailHelper();
        private readonly Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly SupplierModels supplierModel = new SupplierModels();
        private readonly CreateNewModel createNewModel = new CreateNewModel();
        private readonly TransactionModels transModel = new TransactionModels();
        private readonly TransactionHeaderObject transHeaderObj = new TransactionHeaderObject();
        private readonly Details transAddItems = new Details();
        private readonly ItemMasterObject itemMasterObj = new ItemMasterObject();
        private readonly UploaderHelper uploaderHelper = new UploaderHelper();
        readonly Library.EncryptDecrypt.EncryptDecryptPassword encrypting = new Library.EncryptDecrypt.EncryptDecryptPassword();

        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   to load my transaction page
        */
        #region Index
        public ActionResult Index()
        {
            try
            {
                ViewBag.Menu = customHelper.PrepareMenu(12, 0, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "My Transactions";
                ViewBag.PageHeader = "My Transactions";
                //ViewBag.Breadcrumbs = "Home / My Transactions";

                ViewBag.Loginfullname = transModel.Getfullnamelogin(Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.department = Session["department"].ToString();
                ViewBag.currentdate = DateTime.Today.ToString(format: "yyyy-dd-MM");
                //ViewBag.AddedBy = Convert.ToInt32(Session["userId_local"]);
                return View();

            }
            catch (Exception)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }
        }// End
        #endregion

        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   addition of header information of gatepass
        */
        #region AddHeader
        [HttpPost]
        public JsonResult AddHeader()
        {
            try
            {
                var res = new Dictionary<string, object>();
                var transObject = new TransactionHeaderObject();

                string dateofreturn;
                transObject.SessionId = Session.SessionID;
                transObject.ImpexRefNbr = Request["ImpexRefNbr"].ToString();
                transObject.SupplierCode = Request["Supplier"].ToString();
                transObject.ContactPersonCode = Request["ContactPerson"].ToString();

                string returndate = Request["ReturnDate"].ToString();


                //if (rdate != "")
                //{
                //    trans_object.ReturnDate = DateTime.Parse(Request["ReturnDate"]).Date;
                //}
                //else
                //{
                //    trans_object.ReturnDate = Convert.ToDateTime(value: null);
                //}

                //trans_object.ReturnDate = DateTime.Parse(Request["ReturnDate"]).Date);
                transObject.TransType = Request["TransType"].ToString();
                transObject.Purpose = Request["Purpose"].ToString();
                transObject.IsActive = Convert.ToBoolean(Request["IsActive"].ToString());
                transObject.AddedBy = Convert.ToInt32(Session["userId_local"]);
                transObject.UserCode = Session["user_code"].ToString();
                transObject.Status = Request["Status"].ToString();
                transObject.DateAdded = DateTime.Now;
                transObject.Attachment = Request["Attachment"].ToString();
                transObject.DepartmentApproverCode = Request["ApproverCode"].ToString();

                //trans_object.ReturnSlipStatus = Request["ReturnSlipSats"].ToString();
                string[] deptApproverEmailAdd = { createNewModel.GEtDeptApproverEmail(transObject.DepartmentApproverCode) };


                string uploadDir = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_upload_attachment"].ToString();
                res = uploaderHelper.Uploader(Request.Files, uploadDir, Request.Browser.Browser.ToUpper());

                string generatedHeaderCode = transModel.AddHeader(transObject, returndate);



                if (transObject.Status == "Submitted")
                {

                    var emailMod = new EmailModels();
                    string emailContent = EmailContent(generatedHeaderCode, transObject.DepartmentApproverCode);

                    if (!SendEmail(deptApproverEmailAdd, emailContent, generatedHeaderCode))
                    {
                        throw new Exception(message: "Email not sent.");
                    }
                }

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: "Record added successfully!");

            }
            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }//End
        #endregion

        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   submit and update the drafted gatepass
        */
        #region UpdateHeader
        [HttpPost]
        public JsonResult UpdateHeader()
        {
            try
            {
                var res = new Dictionary<string, object>();
                var transObject = new TransactionHeaderObject();

                transObject.Id = Int32.Parse(Request["Id"]);
                transObject.ImpexRefNbr = Request["ImpexRefNbr"].ToString();
                transObject.DepartmentCode = "";
                //transObject.ReturnDate = DateTime.Parse(Request["ReturnDate"]).Date;

                string returndate = Request["ReturnDate"].ToString();

                transObject.TransType = Request["TransType"].ToString();
                transObject.SupplierCode = Request["Supplier"].ToString();
                transObject.ContactPersonCode = Request["ContactPerson"].ToString();
                transObject.CategoryCode = "";
                transObject.TypeCode = "";
                transObject.Purpose = Request["Purpose"].ToString();
                transObject.IsActive = Convert.ToBoolean(Request["IsActive"].ToString());
                transObject.Status = Request["Status"].ToString();
                transObject.AddedBy = Convert.ToInt32(Session["userId_local"]);
                transObject.DateAdded = DateTime.Now;
                transObject.DepartmentApproverCode = Request["ApproverCode"].ToString();

                if (Request["OriginalFileName"].ToString() == Request["CurrentFileName"].ToString())
                {
                    transObject.Attachment = Request["inpt_file"].ToString();
                }
                else
                {
                    if (Request["CurrentFileName"].ToString() == "No file chosen")
                    {
                        transObject.Attachment = "";
                    }
                    else
                    {
                        string uploadDir = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_upload_attachment"].ToString();
                        res = uploaderHelper.Uploader(Request.Files, uploadDir, Request.Browser.Browser.ToUpper());

                        transObject.Attachment = UploaderHelper.FileName;
                    }
                }

                transModel.UpdateHeader(transObject, returndate);
                string generatedHeaderCode = transModel.getHeaderCodeEdit(Request["Id"].ToString());

                string[] deptApproverEmailAdd = { createNewModel.GEtDeptApproverEmail(transObject.DepartmentApproverCode) };

                if (transObject.Status == "Submitted")
                {

                    var emailMod = new EmailModels();
                    string emailContent = EmailContent(generatedHeaderCode, transObject.DepartmentApproverCode);

                    if (!SendEmail(deptApproverEmailAdd, emailContent, generatedHeaderCode))
                    {
                        throw new Exception(message: "Email not sent.");
                    }
                }


                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: "Record submitted successfully");

            }
            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   to get and load transaction details for update
        */
        #region GetTransDetails
        [HttpGet]
        public JsonResult GetTransDetails()
        {
            try
            {
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = transModel.GetTransDetails_cols();

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
                    int totalRows = transModel.GetTransDetails_count(where, headerKey);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (var item in columns)
                    {
                        cols.Add(item.Value);
                    }

                    Dictionary<string, object> cols_arr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(cols_arr));
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
                        transactions = transModel.GetTransDetails(0, 0, where, sorting, headerKey);
                    }
                    else
                    {
                        transactions = transModel.GetTransDetails(start, pagesize, where, sorting, headerKey);
                    }

                    //convert data into json object
                    var data = customHelper.DataTableToJson(transactions);

                    resultConfig.Add("data", data);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> colsArr = customHelper.PrepareStaticColumns(cols);
                    resultConfig.Add(key: "column_config", value: customHelper.PrepareColumns(colsArr));
                    resultConfig.Add(key: "TotalRows", value: transModel.GetTransDetails_count(where, headerKey));
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
        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   to get and load the supplier details with contact info
        */
        #region GetSupplierCombo
        [HttpGet]
        public JsonResult GetSupplierCombo()
        {
            //create a dictionary that will contain the column + datafied config for the grid
            var resultConfig = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = supplierModel.GetSuppliersContactPersonCombo_cols();

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
                int totalRows = supplierModel.GetSuppliersContactPersonCombo_count(where);

                //prepare column config
                var cols = new List<string>();
                foreach (var item in columns)
                {
                    cols.Add(item.Value);
                }

                Dictionary<string, object> cols_arr = customHelper.PrepareStaticColumns(cols);
                resultConfig.Add("column_config", customHelper.PrepareColumns(cols_arr));
                resultConfig.Add("TotalRows", totalRows);
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
                    transactions = supplierModel.GetSuppliersContactPersonCombo(0, 0, where, sorting);
                }
                else
                {
                    transactions = supplierModel.GetSuppliersContactPersonCombo(start, pagesize, where, sorting);
                }

                //convert data into json object
                var data = customHelper.DataTableToJson(transactions);

                resultConfig.Add("data", data);
                //prepare column config
                var cols = new List<string>();
                foreach (DataColumn column in transactions.Columns)
                {
                    cols.Add(column.ColumnName);
                }
                Dictionary<string, object> cols_arr = customHelper.PrepareStaticColumns(cols);
                resultConfig.Add("column_config", customHelper.PrepareColumns(cols_arr));
                resultConfig.Add("TotalRows", supplierModel.GetSuppliersContactPersonCombo_count(where));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", resultConfig);

            return Json(response, JsonRequestBehavior.AllowGet);
        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   addition of item details
        */
        #region AddItemsDetails
        public JsonResult AddItemsDetails()
        {
            try
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                Details trans_details = new Details();

                trans_details.HeaderCode = Request["Id"].ToString();
                trans_details.SessionId = Session.SessionID;
                trans_details.Quantity = Convert.ToDecimal(Request["Quantity"].ToString());
                trans_details.UnitOfMeasureCode = Request["UnitOfMeasureCode"].ToString();
                trans_details.ItemCode = Request["ItemCode"].ToString();
                trans_details.CategoryCode = Request["CategoryCode"].ToString();
                trans_details.ItemTypeCode = Request["ItemTypeCode"].ToString();
                trans_details.SerialNbr = Request["SerialNbr"].ToString();
                trans_details.TagNbr = Request["TagNbr"].ToString();
                trans_details.PONbr = Request["PONbr"].ToString();
                trans_details.IsActive = true;
                trans_details.AddedBy = Convert.ToInt32(Session["userId_local"]);
                trans_details.DateAdded = DateTime.Now;
                trans_details.Remarks = Request["Remarks"].ToString();
                trans_details.Image = Request["image"].ToString();
                string uploadDir = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_upload_images"].ToString();
                res = uploaderHelper.Uploader(Request.Files, uploadDir, Request.Browser.Browser.ToUpper());

                if (Request["Id"].ToString() == null || Request["Id"].ToString() == "")
                {

                    int ctr = createNewModel.IsItemAndSerialisExist(Request["ItemCode"].ToString(), Request["SerialNbr"].ToString());


                    if (ctr == 0)
                    {

                        transModel.AddItems(trans_details);
                        response.Add(key: "success", value: true);
                        response.Add(key: "error", value: false);
                        response.Add(key: "message", value: " ");
                    }
                    else
                    {
                        response.Add(key: "success", value: false);
                        response.Add(key: "error", value: true);
                        response.Add(key: "message", value: "");
                    }


                }
                else
                {

                    int ctr = createNewModel.IsItemAndSerialisExistToDrafted_GP(Request["Id"].ToString(), Request["ItemCode"].ToString(), Request["SerialNbr"].ToString());


                    if (ctr == 0)
                    {

                        transModel.AddItems(trans_details);
                        response.Add(key: "success", value: true);
                        response.Add(key: "error", value: false);
                        response.Add(key: "message", value: " ");
                    }
                    else
                    {
                        response.Add(key: "success", value: false);
                        response.Add(key: "error", value: true);
                        response.Add(key: "message", value: "");
                    }


                }
                // return Json(response, JsonRequestBehavior.AllowGet);
            }


            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }// End
        #endregion



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get and display gate pass transaction records
        */
        //modification: author avillena 6/5/2017 *dded MyDeptTrans to filter the gird by user or by department
        #region GetAllTrans
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllTrans()
        {
            int addedby = Convert.ToInt32(Session["userId_local"]);
            string filterType = "";
            filterType = Request["filterType"];

            bool viewAllTrans = Convert.ToBoolean(Request["MyDeptTrans"].ToString());
            string department = Session["department"].ToString();
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = transModel.GetAllTrans_cols();

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
                where = customHelper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
                //filterType = filters["filtervalue0"];

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
                int totalRows = transModel.GetAllTrans_count(where, addedby, viewAllTrans, department);

                //prepare column config
                var cols = new List<string>();
                foreach (var item in columns)
                {
                    cols.Add(item.Value);
                }

                Dictionary<string, object> cols_arr = customHelper.PrepareStaticColumns(cols);
                result_config.Add("column_config", customHelper.PrepareColumns(cols_arr));
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
                    transactions = transModel.GetAllTrans(0, 0, where, sorting, filterType, addedby, viewAllTrans, department);
                }
                else
                {
                    transactions = transModel.GetAllTrans(start, pagesize, where, sorting, filterType, addedby, viewAllTrans, department);
                }

                //convert data into json object
                var data = customHelper.DataTableToJson(transactions);

                result_config.Add("data", data);

                //prepare column config
                var cols = new List<string>();
                foreach (DataColumn column in transactions.Columns)
                {
                    cols.Add(column.ColumnName);
                }
                Dictionary<string, object> cols_arr = customHelper.PrepareStaticColumns(cols);
                result_config.Add("column_config", customHelper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", transModel.GetAllTrans_count(where, addedby, viewAllTrans, department));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }//End
        #endregion


        /*
       * @author      :   AV <avillena@allegromicro.com>
       * @date        :   DEC. 15, 2016
       * @description :   get and display gate pass transaction records
       */
        //modification: author avillena 6/5/2017 *dded MyDeptTrans to filter the gird by user or by department
        #region GetAllTransTry
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllTransTry()
        {
            int addedby = Convert.ToInt32(Session["userId_local"]);
            string filterType = "";
            filterType = Request["filterType"];

            bool viewAllTrans = Convert.ToBoolean(Request["MyDeptTrans"].ToString());
            string department = Session["department"].ToString();
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = transModel.GetAllTrans_cols();

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
                where = customHelper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
                //filterType = filters["filtervalue0"];

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
                int totalRows = transModel.GetAllTrans_count(where, addedby, viewAllTrans, department);

                //prepare column config
                var cols = new List<string>();
                foreach (var item in columns)
                {
                    cols.Add(item.Value);
                }

                Dictionary<string, object> cols_arr = customHelper.PrepareStaticColumns(cols);
                result_config.Add("column_config", customHelper.PrepareColumns(cols_arr));
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
                    transactions = transModel.GetAllTrans(0, 0, where, sorting, filterType, addedby, viewAllTrans, department);
                }
                else
                {
                    transactions = transModel.GetAllTrans(start, pagesize, where, sorting, filterType, addedby, viewAllTrans, department);
                }

                //convert data into json object
                var data = customHelper.DataTableToJson(transactions);

                result_config.Add("data", data);

                //prepare column config
                var cols = new List<string>();
                foreach (DataColumn column in transactions.Columns)
                {
                    cols.Add(column.ColumnName);
                }
                Dictionary<string, object> cols_arr = customHelper.PrepareStaticColumns(cols);
                result_config.Add("column_config", customHelper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", transModel.GetAllTrans_count(where, addedby, viewAllTrans, department));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }//End
        #endregion






        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   update of gatepass drafted
        */
        #region UpdateTransDraft
        [HttpPost]
        public JsonResult UpdateTransDraft()
        {
            try
            {
                Dictionary<string, object> res = new Dictionary<string, object>();

                transHeaderObj.Id = Convert.ToInt32(Request["id"]);
                transHeaderObj.ImpexRefNbr = Request["ImpexRefNbr"].ToString();
                //trans_header_Obj.ReturnDate = DateTime.Parse(Request["ReturnDate"]).Date;
                string returndate = Request["ReturnDate"].ToString();
                transHeaderObj.TransType = Request["TransType"].ToString();
                transHeaderObj.SupplierCode = Request["Supplier"].ToString();
                transHeaderObj.ContactPersonCode = Request["ContactPerson"].ToString();
                transHeaderObj.Purpose = Request["Purpose"].ToString();
                transHeaderObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                transHeaderObj.DateAdded = DateTime.Now;
                transHeaderObj.DepartmentApproverCode = Request["ApproverCode"].ToString();
                if (Request["OriginalFileName"].ToString() == Request["CurrentFileName"].ToString())
                {
                    transHeaderObj.Attachment = Request["headerAttachment"].ToString();
                }
                else
                {
                    if (Request["CurrentFileName"].ToString() == "No file chosen")
                    {
                        transHeaderObj.Attachment = "";
                    }
                    else
                    {
                        string uploadDir = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_upload_attachment"].ToString();
                        res = uploaderHelper.Uploader(Request.Files, uploadDir, Request.Browser.Browser.ToUpper());

                        transHeaderObj.Attachment = UploaderHelper.FileName;
                    }
                }


                transModel.UpdateTransHeader(transHeaderObj, returndate);

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
        }// End
        #endregion

        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   addition of item on the drafted gatepass
        */
        #region AddItemsDetails_Update
        public JsonResult AddItemsDetails_Update()
        {
            try
            {
                Details trans_addItems = new Details();

                trans_addItems.Id = Convert.ToInt32(Request["Id"]);
                trans_addItems.SessionId = Session.SessionID;
                trans_addItems.Quantity = Convert.ToDecimal(Request["Quantity"].ToString());
                trans_addItems.UnitOfMeasureCode = Request["UnitOfMeasureCode"].ToString();
                trans_addItems.SerialNbr = Request["SerialNbr"].ToString();
                trans_addItems.TagNbr = Request["TagNbr"].ToString();
                trans_addItems.PONbr = Request["PONbr"].ToString();
                trans_addItems.IsActive = true;
                trans_addItems.AddedBy = Convert.ToInt32(Session["userId_local"]);
                trans_addItems.DateAdded = DateTime.Now;


                transModel.AddItems_Update(trans_addItems);
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
        }// End
        #endregion


        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   NOV. 22, 2016 9:04AM
         * @description :   remove items from temp table with the specific session id
         */
        #region RemoveItems
        public JsonResult RemoveItems()
        {
            try
            {
                transModel.RemoveItems_bySessionId(Session.SessionID);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", Session.SessionID);
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }// End
        #endregion


        /*
       * @author      :   AV <avillena@allegromicro.com>
       * @date        :   DEC. 15, 2016
       * @description :   inactive the items of draft
       */
        public JsonResult DeactivateAddItemDraft()
        {
            try
            {
                transAddItems.Id = Convert.ToInt32(Request["id"]);
                transAddItems.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                transAddItems.Action = Request["Action"].ToString();

                transModel.DeactivateAddItemDraft(transAddItems);

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
        // End //


        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : deactivate transaction draft
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public JsonResult DeactivateTransDraft()
        {
            try
            {
                transHeaderObj.Id = Convert.ToInt32(Request["id"]);
                transHeaderObj.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                transHeaderObj.DateAdded = DateTime.Now;
                transHeaderObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                transModel.DeactivateTransDraft(transHeaderObj);

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
        }// End


        /*
         * @author      :   AV <avillena@allegromicro.com>
         * @date        :   DEC. 15, 2016
         * @description :   get and display all records of return slip
         */
        #region GetAll_ReturnSlip
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAll_ReturnSlip()
        {

            int addedby = Convert.ToInt32(Session["userId_local"]);
            var isSearch = 0;
            //create a dictionary that will contain the column + datafied config for the grid
            var resultConfig = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = transModel.GetAllReturnSlipCols();

            //check for filters
            string where = "";
            var filters = new Dictionary<string, string>();
            if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
            {
                isSearch = 1;
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
                int totalRows = transModel.GetAllTransCount(where, isSearch, addedby);

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
                    transactions = transModel.GetAllReturnSlip(offset: 0, next: 0, where: where, sorting: sorting, isSearch: isSearch, addedby: addedby);
                }
                else
                {
                    transactions = transModel.GetAllReturnSlip(start, pagesize, where, sorting, isSearch, addedby);
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
                resultConfig.Add(key: "TotalRows", value: transModel.GetAllTransCount(where, isSearch, addedby));
            }

            response.Add(key: "success", value: true);
            response.Add(key: "error", value: false);
            response.Add(key: "message", value: resultConfig);

            return Json(response, JsonRequestBehavior.AllowGet);
        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all item details of return slip
        */
        #region GetAll_Item_ReturnSlip
        [HttpGet]
        public JsonResult GetAll_Item_ReturnSlip()
        {
            //create a dictionary that will contain the column + datafied config for the grid
            var resultConfig = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = transModel.GetItem_ReturnSlipCols();

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
                    //where = " WHERE HeaderCode = '" + headerKey + "' AND ReturnSlipStatus = 'Not Returned'";
                    where = " WHERE HeaderCode = '" + headerKey + "'";
                }
                else
                {
                    //where += " AND HeaderCode = '" + headerKey + "' AND ReturnSlipStatus = 'Not Returned'";
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
                int totalRows = transModel.GetTransDetailsCount(where, headerKey);

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
                    transactions = transModel.GetItem_ReturnSlip(offset: 0, next: 0, where: where, sorting: sorting, headerKey: headerKey);
                }
                else
                {
                    transactions = transModel.GetItem_ReturnSlip(start, pagesize, where, sorting, headerKey);
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
                resultConfig.Add(key: "TotalRows", value: transModel.GetTransDetailsCount(where, headerKey));
            }

            response.Add(key: "success", value: true);
            response.Add(key: "error", value: false);
            response.Add(key: "message", value: resultConfig);

            return Json(response, JsonRequestBehavior.AllowGet);
        }// End
        #endregion



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   update the transaction details table ReturnSlipStatus column to Returned and PartialReturn
        */
        [HttpGet]
        public JsonResult UpdateItemReturned(string returnslipUsercode, string headercode)

        {
            try
            {

                string[] itemidarr = Request["itemId"].ToString().Split(separator: new char[] { ',' });

                foreach (var item in itemidarr)
                {
                    transAddItems.Id = Convert.ToInt32(item);
                    transAddItems.HeaderCode = Request["headercode"].ToString();
                    transAddItems.ReturnSlipStatus = "Returned";

                    transModel.UpdateItemsforReturn(transAddItems);
                }

                EmailModels email_mod = new EmailModels();
                DataTable dt_emails = email_mod.ReadEmailForReturnSlip(usercode: returnslipUsercode, type: "DeptHead");
                string[] email_arr = new string[1];

                int ctr = 0;
                foreach (DataRow row in dt_emails.Rows)
                {
                    //email_arr.Add(row["Email"].ToString());
                    email_arr[ctr] = row["Email"].ToString();
                    ctr++;
                }

                //string email_content = EmailContent(headercode);

                //if (!SendEmail(email_arr, email_content))
                //{
                //    throw new Exception("Email not sent.");
                //}

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
        } // End



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   constract the content and information of email notification
        */
        //modification: author avillena 6/5/2017 *added SupplierName and to filter the gird by user or by department
        [HttpGet]
        public string EmailContent(string headercode, string approverCode)
        {

            string str = "<table style='font-family:Arial; font-size:12px;width:95%' align='center'>";
            DataTable dtTransheader = transModel.GetGatepassByCode(headercode);

            foreach (DataRow row in dtTransheader.Rows)
            {

                str += "<tr><td style='width:15%'><strong>Gate Pass ID</strong></td><td>: &nbsp;&nbsp</td><td>" + row["code"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Gate Pass Owner</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Requestor"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Department</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Department"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Return Date</strong></td><td>: &nbsp;&nbsp</td><td>" + row["ReturnDate"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Purpose</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Purpose"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Supplier Name</strong></td><td>: &nbsp;&nbsp</td><td>" + row["SupplierName"] + "</td></tr>";
            }
            str += "</table>";

            str += "<br/>";

            DataTable dtTransDetails = transModel.GetGatepassDetailsByCode(headercode);
            str += "<style>table, th, td {border: 1px solid black;border-collapse:collapse;}</style>";
            str += "<table style='width:95%; font-family:Arial;font-size:12px' align='center'>";
            str += "<tr style='background-color:#12AFCB; color:white;font-weight:bold;height:25px'><td>Quantity</td><td style='width:30%'>Item Name</td><td>Serial Number</td><td>Tag Number</td><td>PO Number</td><td>Unit of Measure</td><td>Item Type</td></tr>";



            foreach (DataRow row in dtTransDetails.Rows)

            {
                double quantitywithoutzero = double.Parse(row["Quantity"].ToString().Split(separator: new char[] { ',' })[0]);
                str += "<tr><td>" + quantitywithoutzero + "</td><td>" + row["ItemName"] + "</td><td>" + row["SerialNbr"] + "</td><td>" + row["TagNbr"] + "</td><td>" + row["PONbr"] + "</td><td>" + row["UOMName"] + "</td><td>" + row["ItemTypeName"] + "</td></ tr >";

            }

            str += "</table>";
            str += "<br/>";
            str += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='" + ConfigurationManager.AppSettings["link_into_email"].ToString() + headercode + "&approverCode=" + approverCode + "'>Click here to approve/reject the request</a>";


            str += "<br/>";
            str += "<p style='font-size:12px;font-family:Arial'>&nbsp;&nbsp;&nbsp;&nbsp;<strong>Note :</strong> Best viewed on Google Chrome</p>";

            return str;

        } // End



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   sending of email notification and contract the email template.
        */
        public bool SendEmail(string[] deptApproverEmailAdd, string emailContent, string headercode)
        {

            try

            {

                var service = new EmailServiceReference.ServiceClient();
                var emailObject = new EmailServiceReference.EmailObject();
                var output = new EmailServiceReference.Output();

                emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
                emailObject.Alias = "Online Gate Pass";

                emailObject.Recipient = deptApproverEmailAdd;

                emailObject.Subject = "Gate Pass Id : " + headercode + " - For Approval (Department)";
                emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp FOR APPROVAL (Department)</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailContent + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";

                var carbonCopy = new List<string>();
                carbonCopy.Add(item: Session["mail"].ToString());
                emailObject.CarbonCopy = carbonCopy.ToArray();

                var blindCarbonCopy = new List<string>();
                blindCarbonCopy.Add(item: ConfigurationManager.AppSettings["blind_carbon_copy_email"].ToString());
                emailObject.BlindCarbonCopy = blindCarbonCopy.ToArray();
                emailObject.isHtml = true;


                output = service.SendEmailNotification(emailObject);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }//End



        #region rherejias

        /// <summary>
        /// for returnslip attachment controller
        /// </summary>
        /// <returns>JSON string "Good"</returns>
        /// ReturnSlipAttachment @ver 1.0 @author rherejias 1/3/2017
        /// ReturnSlipAttachment @ver 2.0 @author rherejias 1/16/2017 *added audit trail
        public JsonResult ReturnSlipAttachment()
        {
            try
            {
                Dictionary<string, object> res = new Dictionary<string, object>();

                transHeaderObj.Id = Convert.ToInt32(Request["id"]);
                transHeaderObj.Code = Request["code"].ToString();
                transHeaderObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                transHeaderObj.DateAdded = DateTime.Now;
                transHeaderObj.IP = CustomHelper.GetLocalIPAddress();
                transHeaderObj.MAC = CustomHelper.GetMACAddress();

                string uploadDir = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_upload_returnAttachment"].ToString();
                res = uploaderHelper.Uploader(Request.Files, uploadDir, Request.Browser.Browser.ToUpper());

                transModel.returnAttachment(transHeaderObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", " ");
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
        /// @description :  //rherejias 1/3/2017 for return slip attachment file delete
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public JsonResult ReturnSlipDelAttachment()
        {
            try
            {
                transHeaderObj.Id = Convert.ToInt32(Request["id"]);
                transHeaderObj.Code = Request["Code"].ToString();
                transHeaderObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                transHeaderObj.DateAdded = DateTime.Now;
                transHeaderObj.IP = CustomHelper.GetLocalIPAddress();
                transHeaderObj.MAC = CustomHelper.GetMACAddress();

                transModel.returnDelAttachment(transHeaderObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", " ");
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
        public JsonResult ReadOverdueTransactionHeaders()
        {
            try
            {
                //JobScheduler.Start();
                TransactionModels trans_model = new TransactionModels();

                string datenow = DateTime.Now.ToString("yyyy-MM-dd");

                DataTable dt = trans_model.ReadOverdueTransactionHeaders(datenow);

                foreach (DataRow row in dt.Rows)
                {
                    //get header details
                    string email_content = "<table style='width:95%; font-size:12; font-family:Arial, Helvetica, sans-serif' align='center'>" +
                                                    "<tr>" +
                                                        "<td><strong>Gate Pass Id :</strong></td>" +
                                                        "<td>" +
                                                            row["Code"] +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td><strong>Gate Pass Owner :</strong></td>" +
                                                        "<td>" +
                                                            row["Column1"] +
                                                        "</td>" +
                                                    "</tr>" +

                                                     "<tr>" +
                                                        "<td><strong>Department :</strong></td>" +
                                                        "<td>" +
                                                            row["Department"] +
                                                        "</td>" +
                                                    "</tr>" +

                                                    "<tr>" +
                                                        "<td><strong>Return Date :</strong></td>" +
                                                        "<td>" +
                                                            row["ReturnDate"] +
                                                        "</td>" +
                                                    "</tr>" +
                                                     "<tr>" +
                                                        "<td><strong>Purpose :</strong></td>" +
                                                        "<td>" +
                                                            row["Purpose"] +
                                                        "</td>" +
                                                    "</tr>" +

                                                "</table>";

                    //get details
                    email_content += "<br /><br /><table style='width:95%; font-size:12; font-family:Arial, Helvetica, sans-serif; border: 1px solid black; border-collapse: collapse; font-size:12;' cellpadding='8' align='center'>" +
                                            "<tr>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff'><strong>To be Return</strong></td>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff'><strong>Returned Qty</strong></td>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff'><strong>Total Qty</strong></td>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff; width:30%'><strong>Item Name</strong></td>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff'><strong>Serial Number</strong></td>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff'><strong>Tag Number</strong></td>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff'><strong>PO Number</strong></td>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff'><strong>UoM</strong></td>" +
                                                "<td style='border: 1px solid black; background-color:#12AFCB; color:#ffffff'><strong>Status</strong></td>" +

                                            "</tr>";
                    DataTable dt_items = trans_model.ReadOverdueTransactionItems(row["Code"].ToString());
                    foreach (DataRow row_items in dt_items.Rows)

                    {


                        double quantitywithoutzero = double.Parse(row_items["Quantity"].ToString().Split(separator: new char[] { ',' })[0]);
                        double quantityReturnedwithoutzero = double.Parse(row_items["QtyReturned"].ToString().Split(separator: new char[] { ',' })[0]);
                        double tobereturn = quantitywithoutzero - quantityReturnedwithoutzero;

                        string itemStatus;

                        if (quantitywithoutzero > quantityReturnedwithoutzero && quantityReturnedwithoutzero != 0)
                        {
                            itemStatus = "Partially Returned";

                        }
                        else if (quantityReturnedwithoutzero == 0)
                        {

                            itemStatus = "Not Returned";
                        }
                        else { itemStatus = "Returned"; }



                        string unreturned_style = (itemStatus == "Not Returned") ? "background-color:#F25656; " : "";
                        email_content += "<tr>" +
                                            "<td style='border: 1px solid black; border-collapse: collapse;'>" + tobereturn + "</td>" +
                                            "<td style='border: 1px solid black; border-collapse: collapse;'>" + quantityReturnedwithoutzero + "</td>" +
                                            "<td style='border: 1px solid black; border-collapse: collapse;'>" + quantitywithoutzero + "</td>" +
                                             "<td style='border: 1px solid black; border-collapse: collapse;'>" + row_items["ItemName"] + "</td>" +
                                              "<td style='border: 1px solid black; border-collapse: collapse;'>" + row_items["SerialNbr"] + "</td>" +
                                               "<td style='border: 1px solid black; border-collapse: collapse;'>" + row_items["TagNbr"] + "</td>" +
                                                "<td style='border: 1px solid black; border-collapse: collapse;'>" + row_items["PONbr"] + "</td>" +
                                                "<td style='border: 1px solid black; border-collapse: collapse;'>" + row_items["UOMName"] + "</td>" +
                                                 "<td style='border: 1px solid black; border-collapse: collapse;'>" + itemStatus + "</td>" +
                                         "</tr>";
                    }
                    email_content += "</table><br /><br /><br />";








                    string sender = "GatePass Notification ampinoreply@allegromicro.com";

                    using (var message = new MailMessage(sender, row["Email"].ToString()))
                    {
                        message.Subject = "Items Overdue on Return Date";
                        message.Body = emailHelper.EmailTemplate(email_content);
                        message.IsBodyHtml = true;
                        message.CC.Add(ConfigurationManager.AppSettings["ampiguard_email"].ToString());
                        message.Bcc.Add(ConfigurationManager.AppSettings["blind_carbon_copy_email"].ToString());
                        using (SmtpClient client = new SmtpClient
                        {
                            EnableSsl = false,
                            Host = "maoutlook.allegro.msad",
                            Port = 25,
                            Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ampiguard_email"].ToString(), ConfigurationManager.AppSettings["ampiguard_emailpassword"].ToString())
                        })
                        {
                            client.Send(message);
                        }
                    }
                }

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", customHelper.DataTableToJson(dt));
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   JAN 10, 2017 8:26 AM
         * @description :   this method will return item details for the custom grid under the guard user account (items with a count on how many items still should be returned)
         */
        [HttpGet]
        public JsonResult GetTransactionDetailsWithItemsToBeReturned()
        {
            try
            {
                DataTable dt = transModel.GetTransactionDetailsWithItemsToBeReturned(Request["HeaderKey"].ToString());

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", customHelper.DataTableToJson(dt));
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /*
        * @author      :   AC <aabasolo@allegromicro.com>
        * @date        :   JAN 10, 2017 8:26 AM
        * @description :   this method will return item details for the custom grid under the guard user account (items with a count on how many items still should be returned)
        */
        [HttpGet]
        public JsonResult GetTransactionDetailsOfGatePass()
        {
            try
            {
                DataTable dt = transModel.GetTransactionDetailsOfGatePass(Request["HeaderKey"].ToString());

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", customHelper.DataTableToJson(dt));
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
        /// @author avillena
        /// @description :  get hardcopy of transaction
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public ActionResult hardcopyapproved()
        {

            try
            {
                string headercode = Request["Id"].ToString();

                DataTable approvalstatus = new DataTable();
                approvalstatus = transModel.GetApprovalStatus(headercode);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", customHelper.DataTableToJson(approvalstatus));
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
        /// @author avillena
        /// @description :  //rherejias 1/3/2017 for return slip attachment file delete
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public ActionResult Approvaltracking()
        {

            try
            {
                string headercode = Request["Id"].ToString();
                int nextapproverId = transModel.GatePassApprovalTracking(headercode);
                string isapproved = transModel.GetIfApproved(headercode);

                string nextapprovername = "";
                if (nextapproverId == 0 && isapproved == "Submitted")
                {
                    nextapprovername = "Guard Approver";
                }
                else if (isapproved != "Submitted")
                {
                    nextapprovername = "ApprovedOrRejected";
                }
                else
                {

                    nextapprovername = transModel.GatePassApprovalTrackingName(nextapproverId);
                }


                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", nextapprovername);
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