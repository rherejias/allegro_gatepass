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
    public class OverrideController : Controller
    {
        // GET: Override
        private Dictionary<string, object> response = new Dictionary<string, object>();

        private CustomHelper custom_helper = new CustomHelper();
        private TransactionModels trans_model = new TransactionModels();
        private TransactionModels transModel = new TransactionModels();
        private OverrideModels overrideModels = new OverrideModels();
        private ApprovalLogsObject approverLog = new ApprovalLogsObject();
        private UploaderHelper uploader_helper = new UploaderHelper();
        public string test { get; set; }
        // GET: /Maintenance/

        /// <summary>
        /// description : for getting approvallogs data
        /// version : 1.0
        /// </summary>
        /// <returns>json</returns>
        /// @ver @author rherejias 2/10/2017 
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetOverrideData()
        {
            try
            {
                //create a dictionary that will contain the column + datafied config for the grid
                Dictionary<string, object> result_config = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = overrideModels.GetOverrideData_cols();

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
                    int totalRows = overrideModels.GetOverrideData_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));

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
                        transactions = overrideModels.GetOverrideData(0, 0, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
                    }
                    else
                    {
                        transactions = overrideModels.GetOverrideData(start, pagesize, where, sorting, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString()));
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
                    result_config.Add("TotalRows", overrideModels.GetOverrideData_count(where, ((Request["searchStr"] == null) ? "" : Request["searchStr"].ToString())));
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
        /// description : will hold data from javascript to override model
        /// version : 1.0
        /// </summary>
        /// <returns>true or false</returns>
        /// @ver 1.0 @author rherjeias 2/8/2017 rennn
        public JsonResult Override()
        {
            try
            {
                DateTime dateToday = DateTime.Now;
                DataTable approverdb = overrideModels.GetAprrover(Request["headercodeAdd"].ToString());
                string approvalCode = Request["approvalType"].ToString();
                int approvalcount = overrideModels.GetApprovalLogsCount(Request["headercodeAdd"]);

                List<string> IsApproved = new List<string>();
                List<string> ApproverId = new List<string>();
                List<string> ApproverHeaderCode = new List<string>();
                List<string> ApproverApprovalCode = new List<string>();
                List<string> toBeApprovedByIT = new List<string>(new string[] { ConfigurationManager.AppSettings["departmentApprover"].ToString(), ConfigurationManager.AppSettings["itApprover"] });
                List<string> toBeApprovedByPurch = new List<string>(new string[] { ConfigurationManager.AppSettings["departmentApprover"].ToString(), ConfigurationManager.AppSettings["purchasingApprover"] });
                List<string> toBeApprovedByDept = new List<string>(new string[] { ConfigurationManager.AppSettings["departmentApprover"].ToString() });

                foreach (DataRow row in approverdb.Rows)
                {
                    if (row["IsApproved"].ToString() == "0")
                    {
                        if (approvalCode == ConfigurationManager.AppSettings["itApprover"] && toBeApprovedByIT.Contains(row["ApprovalTypeCode"].ToString()))
                        {
                            overrideFunction(row, dateToday, Request["headercodeAdd"].ToString(), Request["remarks"].ToString());
                        }
                        else if (approvalCode == ConfigurationManager.AppSettings["purchasingApprover"] && toBeApprovedByPurch.Contains(row["ApprovalTypeCode"].ToString()))
                        {
                            overrideFunction(row, dateToday, Request["headercodeAdd"].ToString(), Request["remarks"].ToString());
                        }
                        else if (approvalCode == ConfigurationManager.AppSettings["accountingApprover"])
                        {
                            overrideFunction(row, dateToday, Request["headercodeAdd"].ToString(), Request["remarks"].ToString());
                        }
                        else if (approvalCode == ConfigurationManager.AppSettings["departmentApprover"].ToString() && toBeApprovedByDept.Contains(row["ApprovalTypeCode"].ToString()))
                        {
                            overrideFunction(row, dateToday, Request["headercodeAdd"].ToString(), Request["remarks"].ToString());
                        }

                    }
                }
                //ren
                if (approvalcount == 4)
                {
                    if (approvalCode == ConfigurationManager.AppSettings["itApprover"])
                    {

                        string emailLabel = "Purchasing";
                        string IsApprovedPurchasing = overrideModels.GetIsApprovedPurch(Request["headercodeAdd"].ToString());
                        string purchEmail = overrideModels.GetPurchasingEmail(Request["headercodeAdd"].ToString());
                        string userCode = overrideModels.GetUserCode(purchEmail);
                        if (IsApprovedPurchasing == "0")
                        {

                            ApprovalEmailContent(Request["headercodeAdd"].ToString(), userCode, purchEmail, emailLabel);

                        }
                    }
                    else if (approvalCode == ConfigurationManager.AppSettings["purchasingApprover"])
                    {

                        string emailLabel = "IT";
                        string IsApprovedIT = overrideModels.GetIsApprovedIT(Request["headercodeAdd"].ToString());
                        string ITemail = overrideModels.GetITEmail(Request["headercodeAdd"].ToString());
                        string userCode = overrideModels.GetUserCode(ITemail);
                        if (IsApprovedIT == "0")
                        {
                            ApprovalEmailContent(Request["headercodeAdd"].ToString(), userCode, ITemail, emailLabel);

                        }
                    }
                }

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "Record added successfully!");
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
        /// for adding of data in approvallogs table
        /// </summary>
        /// <param name="row"> specify row data</param>
        /// <param name="dateToday">date today</param>
        /// <param name="code">null</param>
        /// <param name="remarks">remarks for overriding data</param>
        /// <returns>boolean true or false</returns>
        /// @ver @author rherejias 2/10/2017 
        public bool overrideFunction(DataRow row, DateTime dateToday, string code, string remarks)
        {
            approverLog.Id = Convert.ToInt32(row["Id"]);
            approverLog.Code = code;
            approverLog.IsApproved = true;
            approverLog.ApprovedBy = Convert.ToInt32(Session["userId_local"]);
            approverLog.DateApproved = dateToday;
            approverLog.Remarks = remarks;
            approverLog.IsOverride = true;
            approverLog.IP = CustomHelper.GetLocalIPAddress();
            approverLog.MAC = CustomHelper.GetMACAddress();

            return overrideModels.Override(approverLog);
        }




        /// <summary>
        /// for override email
        /// </summary>
        /// <param name="headercode">for gatepass details</param>
        /// <returns> string html</returns>
        /// @ver 1.0 @author rherejias 2/9/2017
        [HttpGet]
        public void EmailContent(string headercode, string approvalType, string distinguisher)
        {
            string str = "<table style='font-family:Arial; font-size:12px;width:95%' align='center'>";
            DataTable dtTransheader = trans_model.GetGatepassByCode(headercode);
            string remarks = overrideModels.GetRemarks(headercode, approvalType);
            string name = overrideModels.GetName(Session["userId_local"].ToString());
            DataTable approverEmail;
            if (distinguisher == "approve")
                approverEmail = overrideModels.GetApproverEmails(headercode, approvalType);
            else
                approverEmail = overrideModels.GetApproverEmailsReject(headercode, approvalType);


            string requestor = overrideModels.GetRequestor(headercode);

            List<string> Email = new List<string>();
            foreach (DataRow row in approverEmail.Rows)
            {
                Email.Add(row["Email"].ToString());
            }


            foreach (DataRow row in dtTransheader.Rows)
            {
                str += "<tr><td style='width:15%'><strong>Overridden By</strong></td><td>: &nbsp;&nbsp</td><td>" + name + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Override Reason</strong></td><td>: &nbsp;&nbsp</td><td>" + remarks + "</td></tr>";
                str += "<tr><td style='width:15%'><strong></strong></td><td>&nbsp;&nbsp</td><td></td></tr>";
                str += "<tr><td style='width:15%'><strong>Gate Pass ID</strong></td><td>: &nbsp;&nbsp</td><td>" + row["code"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Gate Pass Owner</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Requestor"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Department</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Department"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Return Date</strong></td><td>: &nbsp;&nbsp</td><td>" + row["ReturnDate"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Purpose</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Purpose"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Supplier Name</strong></td><td>: &nbsp;&nbsp</td><td>" + row["SupplierName"] + "</td></tr>";

            }
            str += "</table>";

            str += "<br/>";

            DataTable dtTransDetails = trans_model.GetGatepassDetailsByCode(headercode);
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
            str += "<br/>";
            str += "<br/>";


            SendEmail(str, Email, requestor, distinguisher);
        }




        /// <summary>
        /// for email template ren2 
        /// </summary>
        /// <param name="email_arr">array of "to"</param>
        /// <param name="email_content">from EmailContent</param>
        /// <returns>email</returns>
        /// @ver 1.0 @author rherejias 2/9/2017
        public EmailServiceReference.Output SendEmail(string email_content, List<string> Email, string requestor, string distinguisher)
        {
            var service = new EmailServiceReference.ServiceClient();
            var emailObject = new EmailServiceReference.EmailObject();
            var output = new EmailServiceReference.Output();

            emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
            emailObject.Alias = "Online Gate Pass";

            List<string> recipient = new List<string>();
            if (distinguisher == "approve")
            {
                foreach (var item in Email)
                {
                    recipient.Add(item);
                }
                emailObject.Recipient = recipient.ToArray();
            }
            else
            {
                recipient.Add(requestor);
                emailObject.Recipient = recipient.ToArray();
            }



            emailObject.Subject = "Online Gate Pass: Override";
            emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp " + ((distinguisher == "approve") ? "OVERRIDE (Approve)" : "OVERRIDE (Reject)") + "</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + email_content + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";

            var carbonCopy = new List<string>();
            if (distinguisher == "approve")
            {
                carbonCopy.Add(requestor);
                emailObject.CarbonCopy = carbonCopy.ToArray();
            }
            else
            {
                foreach (var item in Email)
                {
                    carbonCopy.Add(item);
                }
                emailObject.CarbonCopy = carbonCopy.ToArray();
            }


            var blindcarbonCopy = new List<string>();
            string bcc_str = ConfigurationManager.AppSettings["dev_email"].ToString();
            string[] bcc_arr = bcc_str.Split(',');
            for (int i = 0; i < bcc_arr.Length; i++)
            {
                blindcarbonCopy.Add(bcc_arr[i]);
            }
            emailObject.BlindCarbonCopy = blindcarbonCopy.ToArray();

            emailObject.isHtml = true;

            output = service.SendEmailNotification(emailObject);
            return output;
        }




        /// <summary>
        /// for override reject 
        /// </summary>
        /// <returns>response if success or not</returns>
        /// @ver @author rherejias 2/10/2017 
        public JsonResult Reject()
        {
            try
            {
                approverLog.Id = Convert.ToInt32(Request["id"]);
                approverLog.TransHeaderCode = Request["headercode"].ToString();
                approverLog.IsApproved = false;
                approverLog.RejectedBy = Convert.ToInt32(Session["userId_local"]);
                approverLog.DateRejected = DateTime.Now;
                approverLog.IsOverride = true;
                approverLog.Remarks = Request["remarks"].ToString();
                approverLog.IP = CustomHelper.GetLocalIPAddress();
                approverLog.MAC = CustomHelper.GetMACAddress();

                overrideModels.Reject(approverLog);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "Record added successfully!");
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
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   constract the content and information of email notification
        */
        [HttpGet]
        public void ApprovalEmailContent(string headercode, string approverCode, string email, string emialLabelApprover)
        {
            string str = "<table style='font-family:Arial; font-size:12px;width:95%' align='center'>";
            DataTable dtTransheader = transModel.GetGatepassByCode(headercode);
            string requestor = overrideModels.GetRequestor(headercode);

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
            str += "<p style='font-size:12px;font-family:Arial'>&nbsp;&nbsp;&nbsp;&nbsp;<strong>Note :</strong> Use Google Chrome browser for better display.</p>";

            SendEmailSpecialCase(email, str, requestor, emialLabelApprover);

        } // End

        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   sending of email notification and contract the email template.
        */
        public EmailServiceReference.Output SendEmailSpecialCase(string email, string emailContent, string requestor, string emialLabelApprover)
        {
            var service = new EmailServiceReference.ServiceClient();
            var emailObject = new EmailServiceReference.EmailObject();
            var output = new EmailServiceReference.Output();

            emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
            emailObject.Alias = "Online Gate Pass";

            List<string> recipient = new List<string>();

            recipient.Add(email);

            emailObject.Recipient = recipient.ToArray();

            emailObject.Subject = "Online Gate Pass : For Approval" + " " + "(" + emialLabelApprover + ")" + "";
            emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp;FOR APPROVAL " + " " + "(" + emialLabelApprover + ")" + "</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailContent + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";
            var carbonCopy = new List<string>();
            carbonCopy.Add(requestor);
            emailObject.CarbonCopy = carbonCopy.ToArray();

            var blindCarbonCopy = new List<string>();
            blindCarbonCopy.Add("avillena@allegromicro.com");
            blindCarbonCopy.Add("rherejias@ALLEGROMICRO.com");
            blindCarbonCopy.Add("abasolo@ALLEGROMICRO.com");
            emailObject.BlindCarbonCopy = blindCarbonCopy.ToArray();

            emailObject.isHtml = true;

            output = service.SendEmailNotification(emailObject);

            return output;
        }//End
    }
}