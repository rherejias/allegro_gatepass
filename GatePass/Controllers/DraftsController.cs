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
    public class DraftsController : Controller
    {


        private readonly DraftsModel draftsModel = new DraftsModel();
        private readonly CustomHelper customHelper = new CustomHelper();
        private readonly Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly SupplierModels supplierModel = new SupplierModels();
        private readonly TransactionModels transModel = new TransactionModels();
        private readonly TransactionHeaderObject trans_header_Obj = new TransactionHeaderObject();
        private readonly Details trans_addItems = new Details();
        private readonly ItemMasterObject ItemMasterObj = new ItemMasterObject();
        private readonly UploaderHelper uploaderHelper = new UploaderHelper();
        private readonly CreateNewModel createnewModel = new CreateNewModel();


        /// <summary>
        /// @author: avillena
        /// @desc: draft page view
        /// @version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult DraftsView()
        {
            try
            {
                ViewBag.Menu = customHelper.PrepareMenu(child: 13, parent: 0, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "My Drafts";
                ViewBag.PageHeader = "My Drafts";
                return View();
            }
            catch (Exception)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }
        }



        /// <summary>
        /// description : get all drafted transaction
        /// by: avillena@allegromicro.com
        /// date: january 10 2017
        /// </summary>
        /// <returns></returns>
        #region GetAllDraftedTrans
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllDraftedTrans()
        {
            try
            {

                int addedby = Convert.ToInt32(Session["userId_local"]);
                //string addedby = draftsModel.GetAddedby(Session["user-code"].ToString());

                //create a dictionary that will contain the column + datafied config for the grid
                Dictionary<string, object> result_config = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = draftsModel.GetAllTrans_cols();

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
                    int totalRows = draftsModel.GetAllTrans_count(where, addedby);

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
                        transactions = draftsModel.GetAllTrans(0, 0, where, sorting, addedby);
                    }
                    else
                    {
                        transactions = draftsModel.GetAllTrans(start, pagesize, where, sorting, addedby);
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
                    result_config.Add("TotalRows", draftsModel.GetAllTrans_count(where, addedby));
                }

                response.Add(key: "success", value: true);
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
        }//End
        #endregion





        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   addition of item details
      */
        #region UpdateItemsDetailsDraft
        public JsonResult UpdateItemsDetailsDraft()
        {
            try
            {
                var res = new Dictionary<string, object>();
                var transDetails = new Details();

                transDetails.Id = Convert.ToInt32(Request["AddedItem_ID"]);
                transDetails.HeaderCode = Request["IdEdit"].ToString();
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

                int ctr = draftsModel.IsItemAndSerialisExistToDrafted_GP(Request["IdEdit"].ToString(), Request["ItemCodeEdit"].ToString(), Request["SerialNbrEdit"].ToString(), transDetails.Id);


                if (ctr == 0)
                {

                    draftsModel.UpdateItemsDraft(transDetails);
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
                int draftId = Convert.ToInt32(Request["draftId"].ToString());
                int countOfNonreturnableItem = draftsModel.Hasnonreturnableitem(draftId);

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