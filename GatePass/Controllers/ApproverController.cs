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
    public class ApproverController : Controller
    {
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private CustomHelper custom_helper = new CustomHelper();
        private ApproverModels approver_model = new ApproverModels();
        private AppoverObject approverObj = new AppoverObject();
        private UploaderHelper uploader_helper = new UploaderHelper();
        private AuditTrailObject auditTrailObj = new AuditTrailObject();
        // GET: /Maintenance/


        /// <summary>
        /// author: avillena@allegromicro.com
        /// description : Get the department approver base on the user login departmetn
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetApprovers()
        {
            //@ver 1.0 rherejias 1/18/2017 used for where clause IsActive
            string IsActive;
            string type;

            IsActive = Request["IsActive"].ToString();
            type = Request["Type"].ToString();

            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = approver_model.GetApprovers_cols();

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
                int totalRows = approver_model.GetApprovers_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), IsActive, type);

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
                    transactions = approver_model.GetApprovers(0, 0, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), IsActive, type);
                }
                else
                {
                    transactions = approver_model.GetApprovers(start, pagesize, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), IsActive, type);
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
                result_config.Add("TotalRows", approver_model.GetApprovers_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()), IsActive, type));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// author : rherejias@allegromicro.com
        /// description : for approver addtion        
        /// <returns>JSON string "Good"</returns>
        /// ADDAPPROVER @ver 1.0 @author rherejias 1/4/2017
        /// ADDAPPROVER @ver 2.0 @author rherejias 1/16/2017 *added audit trail function
        /// </summary>
        public JsonResult addApprover()
        {
            try
            {
                string IsGuard = approver_model.getUserDepartmentName(Request["userCode"].ToString());
                if (IsGuard == ConfigurationManager.AppSettings["GuardDept"])
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", " Guards can't be assigned as approvers.");
                }
                else
                {
                    string userDept = "";

                    if (Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["secondaryITApprover"] || Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["secondaryPurchasingApprover"] ||
                        Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["secondaryAccountingApprover"])
                    {
                        string dept = approver_model.getUserDepartmentName(Request["userCode"].ToString());
                        userDept = approver_model.getDepartmentCode(dept).ToString();
                    }
                    else
                    {
                        userDept = Request["departmentCode"].ToString();
                    }
                    approverObj.ApprovalTypeCode = Request["approvalTypeCode"].ToString();
                    approverObj.DepartmentCode = userDept;
                    approverObj.UserCode = Request["userCode"].ToString();
                    approverObj.DateAdded = DateTime.Now;
                    approverObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                    approverObj.IsActive = true;
                    approverObj.IP = CustomHelper.GetLocalIPAddress();
                    approverObj.MAC = CustomHelper.GetMACAddress();

                    bool IsExist = approver_model.IsApproverExistAndDept(approverObj.UserCode, approverObj.DepartmentCode, approverObj.ApprovalTypeCode);

                    if (IsExist == true)
                    {
                        approver_model.addApprover(approverObj);

                        response.Add("success", true);
                        response.Add("error", false);
                        response.Add("message", "Record added successfully!");
                    }
                    else
                    {
                        response.Add("success", false);
                        response.Add("error", true);
                        response.Add("message", " User already exist.");
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
        /// author : rherejias@allegromicro.com
        /// description : for approver update
        /// version : 1.0    
        /// <returns>JSON string "Good"</returns>
        /// UPDATEAPRROVER @ver 1.0 @author rherejias 1/4/2017
        /// UPDATEAPRROVER @ver 2.0 @author rherejias 1/16/2017 *added audit trail function
        /// </summary>
        public JsonResult editApprover()
        {
            try
            {
                string userDept = "";
                string guardDept = approver_model.getGuardDept(Request["userCode"].ToString());
                if (guardDept == ConfigurationManager.AppSettings["GuardDept"])
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", " Guards can't be assigned as approvers.");
                }
                else
                {
                    if (Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["secondaryITApprover"] || Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["secondaryPurchasingApprover"] ||
                   Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["secondaryAccountingApprover"] || Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["itApprover"] ||
                   Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["purchasingApprover"] || Request["approvalTypeCode"].ToString() == ConfigurationManager.AppSettings["accountingApprover"])
                    {
                        string test = approver_model.getUserDepartmentName(Request["userCode"].ToString());
                        userDept = approver_model.getDepartmentCode(test).ToString();
                    }
                    else
                    {
                        userDept = Request["departmentCode"].ToString();
                    }

                    string ApprovalTypeCode = Request["approvalTypeCode"].ToString();

                    approverObj.Id = Convert.ToInt32(Request["id"]);
                    approverObj.Code = Request["code"].ToString();
                    approverObj.DepartmentCode = userDept;
                    approverObj.UserCode = Request["userCode"].ToString();
                    approverObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                    approverObj.IP = CustomHelper.GetLocalIPAddress();
                    approverObj.MAC = CustomHelper.GetMACAddress();
                    approverObj.DateAdded = DateTime.Now;

                    bool IsApproverExist = approver_model.IsApproverExistAndDept(approverObj.UserCode, userDept, ApprovalTypeCode);
                    long isExist = approver_model.isUsernameExistEdit(approverObj.UserCode, ApprovalTypeCode);
                    long isPrimaryExist = approver_model.isPrimaryExist(approverObj.DepartmentCode);

                    if (IsApproverExist == true)
                    {
                        approver_model.editApprover(approverObj);

                        response.Add("success", true);
                        response.Add("error", false);
                        response.Add("message", "Record updated successfully!");
                    }
                    else
                    {
                        //if (isExist.ToString() == approverObj.Code)
                        //{
                        //    approver_model.editApprover(approverObj);

                        //    response.Add("success", true);
                        //    response.Add("error", false);
                        //    response.Add("message", "Record updated successfully!");
                        //}
                        //else
                        //{
                        response.Add("success", false);
                        response.Add("error", true);
                        response.Add("message", " User already exist.");
                        //}
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
        /// author : rherejias@allegromicro.com
        /// description : for approver inactive
        /// </summary>
        /// <returns>JSON string "Good"</returns>
        /// INACTIVEAPPROVER @ver 1.0 @author rherejias 1/5/2017  
        /// INACTIVEAPPROVER @ver 2.0 @author rherejias 1/16/2017 *added audit trail function
        public JsonResult inactiveApprover()
        {
            try
            {
                approverObj.Id = Convert.ToInt32(Request["id"]);
                approverObj.Code = Request["code"].ToString();
                approverObj.ApprovalTypeCode = Request["approverType"].ToString();
                approverObj.DateAdded = DateTime.Now;
                approverObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                approverObj.IP = CustomHelper.GetLocalIPAddress();
                approverObj.MAC = CustomHelper.GetMACAddress();

                if (approverObj.ApprovalTypeCode == ConfigurationManager.AppSettings["itApprover"] || approverObj.ApprovalTypeCode == ConfigurationManager.AppSettings["purchasingApprover"] || approverObj.ApprovalTypeCode == ConfigurationManager.AppSettings["accountingApprover"])
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", " Primary approvers cannot be de-activated.");
                }
                else
                {
                    approver_model.inactiveApprover(approverObj);

                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", "Record deactivation successfully!");
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
        /// author : rherejias@allegromicro.com
        /// description : for approver re-activate
        /// </summary>
        /// <returns>JSON string "Good"</returns>
        /// INACTIVEAPPROVER @ver 1.0 @author rherejias 1/18/2017  
        public JsonResult ActiveApprover()
        {
            try
            {
                approverObj.Id = Convert.ToInt32(Request["id"]);
                approverObj.Code = Request["code"].ToString();
                approverObj.DateAdded = DateTime.Now;
                approverObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                approverObj.IP = CustomHelper.GetLocalIPAddress();
                approverObj.MAC = CustomHelper.GetMACAddress();


                approver_model.ActiveApprover(approverObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "Record deactivation successfully!");

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
        /// author : rherejias@allegromicro.com
        /// description : for approver assigning
        /// </summary>
        /// <returns>JSON string "Good"</returns>
        /// ASSIGN APPROVER @ver 1.0 @author rherejias 2/6/2017  
        [HttpGet]
        public JsonResult AssignApprover()
        {
            try
            {
                approverObj.Code = Request["code"].ToString();
                approverObj.Module = Request["module"].ToString();
                approverObj.ApprovalTypeCode = Request["type"].ToString();
                approverObj.DepartmentCode = Request["department"].ToString();
                approverObj.Id = Convert.ToInt32(Request["id"]);
                approverObj.DateAdded = DateTime.Now;
                approverObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                approverObj.IP = CustomHelper.GetLocalIPAddress();
                approverObj.MAC = CustomHelper.GetMACAddress();

                approver_model.AssignApprover(approverObj);

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
        }
    }
}