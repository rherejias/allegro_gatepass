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

    public class CreateNewController : Controller
    {
        private readonly CustomHelper customHelper = new CustomHelper();
        private readonly Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly SupplierModels supplierModel = new SupplierModels();
        private readonly TransactionModels transModel = new TransactionModels();
        private readonly TransactionHeaderObject transHeaderObj = new TransactionHeaderObject();
        private readonly Details transAddItems = new Details();
        private readonly ItemMasterObject itemMasterObj = new ItemMasterObject();
        private readonly UploaderHelper uploaderHelper = new UploaderHelper();
        private readonly CreateNewModel createnewModel = new CreateNewModel();
        private readonly ItemModels itemsModel = new ItemModels();
        private readonly DepartmentApproverModels departmentApproverModel = new DepartmentApproverModels();


        /// <summary>
        /// desc: display 'create new gate' page
        /// by: avillena
        /// date: 01-10-17
        /// </summary>
        /// <returns></returns>
        public ActionResult GatePass()

        {
            try
            {
                string userLogin = Session.SessionID;

                ViewBag.Menu = customHelper.PrepareMenu(child: 2, parent: 0, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Create New";
                ViewBag.PageHeader = "Create Gate Pass";
                ViewBag.path = ConfigurationManager.AppSettings["local_upload_attachment"];
                DataTable dt = transModel.DeleteAllItem_Temptbl(userLogin);
                return View();

            }
            catch (Exception e)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }
        }




        /// <summary>
        /// @author: avillena
        /// @desc: remove the added items
        /// @version : 1.0
        /// </summary>
        /// <returns></returns>
        public JsonResult RemoveITem()
        {
            try
            {

                DataTable dt = transModel.DeleteOneItem(Request["id"].ToString());

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: " ");
            }
            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }



        #region GetItemforEdit
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 12, 2017
         * @description     :   return json of suppliers
         */
        public JsonResult GetItemforEdit()
        {
            try
            {
                DataTable dt = createnewModel.GetItemforEdit();

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: customHelper.DataTableToJson(dt));
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



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   addition of item details
        */
        #region UpdateItemsDetails
        public JsonResult UpdateItemsDetails()
        {
            try
            {
                var res = new Dictionary<string, object>();
                var transDetails = new Details();

                transDetails.Id = Convert.ToInt32(Request["IdEdit"]);
                transDetails.SessionId = Session.SessionID;
                transDetails.Quantity = Convert.ToDecimal(Request["QuantityEdit"].ToString());
                transDetails.UnitOfMeasureCode = Request["UnitOfMeasureCodeEdit"].ToString();
                transDetails.ItemCode = Request["ItemCodeEdit"].ToString();
                transDetails.CategoryCode = Request["CategoryCodeEdit"].ToString();
                transDetails.ItemTypeCode = Request["ItemTypeCodeEdit"].ToString();
                transDetails.SerialNbr = Request["SerialNbrEdit"].ToString();
                transDetails.TagNbr = Request["TagNbrEdit"].ToString();
                transDetails.PONbr = Request["PONbrEdit"].ToString();
                transDetails.IsActive = true;
                transDetails.AddedBy = Convert.ToInt32(Session["userId_local"]);
                transDetails.DateAdded = DateTime.Now;
                transDetails.Remarks = Request["RemarksEdit"].ToString();
                transDetails.Image = Request["imageEdit"].ToString();
                string uploadDir = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_upload_images"].ToString();
                res = uploaderHelper.Uploader(Request.Files, uploadDir, Request.Browser.Browser.ToUpper());

                int ctr = createnewModel.IsItemAndSerialisExistUpdate(Request["ItemCodeEdit"].ToString(), Request["SerialNbrEdit"].ToString(), Convert.ToInt32(Request["IdEdit"]));


                if (ctr == 0)
                {

                    createnewModel.UpdateItems(transDetails);
                    response.Add(key: "success", value: true);
                    response.Add(key: "error", value: false);
                    response.Add(key: "message", value: "Update successful.");
                }
                else
                {
                    response.Add(key: "success", value: false);
                    response.Add(key: "error", value: true);
                    response.Add(key: "message", value: "Item already exist with the same serial number.");
                }

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




        /// <summary>
        /// Author : avillena
        /// Desc: to get department approver
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetDepartmentApprovers()
        {
            try
            {
                string userdept = Session["department"].ToString();
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = createnewModel.GetDepartmentApproversCols();

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
                    int totalRows = createnewModel.GetDepartmentApproversCount(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), userdept);

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
                        transactions = createnewModel.GetDepartmentApprovers(offset: 0, next: 0, where: where, sorting: sorting, searchStr: ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), userdept: userdept);
                    }
                    else
                    {
                        transactions = createnewModel.GetDepartmentApprovers(start, pagesize, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), userdept);
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
                    resultConfig.Add(key: "TotalRows", value: createnewModel.GetDepartmentApproversCount(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), userdept));
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


        #region GetItemforEdit
        /*
         * @author          :   AC <avillena@allegromicro.com>
         * @date            :   JAN 12, 2017
         * @description     :   check if the item has non returnable type
         */
        public JsonResult HasItemreturnable()
        {
            try
            {
                string sessionid = Session.SessionID;
                int countOfNonreturnableItem = createnewModel.Hasnonreturnableitem(sessionid);

                response.Add(key: "success", value: true);
                response.Add(key: "error", value: false);
                response.Add(key: "message", value: countOfNonreturnableItem);
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


    }
}