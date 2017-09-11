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
    public class GuardController : Controller
    {




        private readonly CustomHelper customHelper = new CustomHelper();
        private readonly Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly SupplierModels supplierModel = new SupplierModels();
        private readonly TransactionModels transModel = new TransactionModels();
        private readonly ItemReturnLogsObject itemReturnLogsDetails = new ItemReturnLogsObject();
        private readonly TransactionHeaderObject transHeaderObj = new TransactionHeaderObject();
        private readonly Details transAddItems = new Details();
        private readonly ItemMasterObject itemMasterObj = new ItemMasterObject();
        private readonly UploaderHelper uploaderHelper = new UploaderHelper();
        private readonly GuardModel guardModel = new GuardModel();
        private readonly AuditTrailModels auditModels = new AuditTrailModels();
        private readonly AuditTrailObject auditObj = new AuditTrailObject();
        private readonly EmailHelper emailHelper = new EmailHelper();


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   notification for gate pass requestor that his/her gate pass was approved.
        * @version     :   1.0
        * /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   JUNE. 5, 2017
        * @description :   adding of header/gate pass id parameter to display on email notification
        * version      :   2.0
        */

        public bool SendEmailReturnSlipByGuard(string emailContentAlreadyApproved, string[] gatePassOwner, string[] accountingApprover, string headercode)
        {

            try

            {

                var service = new EmailServiceReference.ServiceClient();
                var emailObject = new EmailServiceReference.EmailObject();
                var output = new EmailServiceReference.Output();

                emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
                emailObject.Alias = "Online Gate Pass";

                var recipientAccounting = new List<string>();
                foreach (var item in accountingApprover)
                {
                    recipientAccounting.Add(item);
                }
                emailObject.Recipient = recipientAccounting.ToArray();

                emailObject.Subject = "Online Gate Pass : Return Slip Id : " + headercode + "";
                emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp;RETURN SLIP DETAILS</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailContentAlreadyApproved + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px;border-color:white'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";
                var carbonCopy = new List<string>();
                foreach (var item in gatePassOwner)
                {
                    carbonCopy.Add(item);
                }
                emailObject.CarbonCopy = carbonCopy.ToArray();

                var blindCarbonCopy = new List<string>();
                blindCarbonCopy.Add(item: ConfigurationManager.AppSettings["blind_carbon_copy_email"].ToString());
                emailObject.BlindCarbonCopy = blindCarbonCopy.ToArray();

                emailObject.isHtml = true;

                output = service.SendEmailNotification(emailObject);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }//End


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   constract the content and information of email notification
        */
        [HttpGet]
        public string EmailContentReturnSlip(string headercode, string remarks)
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
                str += "<tr><td style='width:15%'><strong>Remarks</strong></td><td>: &nbsp;&nbsp</td><td>" + remarks + "</td></tr>";
            }
            str += "</table>";

            str += "<br/>";

            DataTable dtTransDetails = transModel.GetGatepassDetailsByCode(headercode);
            str += "<style>table, th, td {border: 1px solid black;border-collapse:collapse;}</style>";
            str += "<table style='width:95%; font-family:Arial;font-size:12px' align='center'>";
            str += "<tr style='background-color:#12AFCB; color:white;font-weight:bold;height:25px'><td>To be Return</td><td>Returned Qty</td><td>Total Qty</td><td style='width:30%'>Item Name</td><td>Serial Number</td><td>Tag Number</td><td>PO Number</td><td>Unit of Measure</td><td style='width:15%'>Status</td></tr>";



            foreach (DataRow row in dtTransDetails.Rows)

            {
                double quantitywithoutzero = double.Parse(row["Quantity"].ToString().Split(separator: new char[] { ',' })[0]);
                double quantityReturnedwithoutzero = double.Parse(row["QtyReturned"].ToString().Split(separator: new char[] { ',' })[0]);
                double tobereturn = quantitywithoutzero - quantityReturnedwithoutzero;

                string itemStatus;

                if (quantitywithoutzero > quantityReturnedwithoutzero && quantityReturnedwithoutzero != 0)
                {
                    itemStatus = "Partially Returned";

                }
                else if (quantityReturnedwithoutzero == 0)
                {

                    itemStatus = "Not Return";
                }
                else { itemStatus = "Returned"; }


                str += "<tr><td>" + tobereturn + "</td><td>" + quantityReturnedwithoutzero + "</td><td>" + quantitywithoutzero + "</td><td>" + row["ItemName"] + "</td><td>" + row["SerialNbr"] + "</td><td>" + row["TagNbr"] + "</td><td>" + row["PONbr"] + "</td><td>" + row["UOMName"] + "</td><td>" + itemStatus + "</td></ tr >";

            }

            str += "</table>";
            str += "<br/>";
            str += "<br/>";
            str += "<p style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;</p>";

            return str;

        } // End




        /*
         * @author      :   AV <avillena@allegromicro.com>
         * @date        :   DEC. 15, 2016
         * @description :   constract the content and information of email notification
         */
        [HttpGet]
        public string EmailContent(string headercode)
        {
            string noteretunable = "<p style='font-size:12px; font-family:Arial'>&nbsp;&nbsp;&nbsp;&nbsp; <strong>Info : </strong>Your Return Slip was automatically generated with the same 'Id number' of this requested Gate Pass. </p>";

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
                if (row["ReturnDate"].ToString() == "" || row["ReturnDate"].ToString() == null)
                {
                    noteretunable = "<p style='font-size:12px; font-family:Arial'>&nbsp;&nbsp;&nbsp;&nbsp; <strong>Info : </strong>Return Slip is NOT Applicable, Item in this Gate Pass is/are Non-Returnable. </p>";

                }

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
            str += "<br/>";
            str += noteretunable;
            return str;

        } // End



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   notification for gate pass requestor that his/her gate pass was approved.
        */
        public bool SendEmailApprovedByGuard(string emailContentAlreadyApproved, string[] gatePassOwner, string[] accountingApprover, string headercode)
        {

            try

            {

                var service = new EmailServiceReference.ServiceClient();
                var emailObject = new EmailServiceReference.EmailObject();
                var output = new EmailServiceReference.Output();

                emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
                emailObject.Alias = "Online Gate Pass";

                var recipient = new List<string>();
                foreach (var item in gatePassOwner)
                {
                    recipient.Add(item);
                }
                emailObject.Recipient = recipient.ToArray();

                emailObject.Subject = "Gate Pass Id : " + headercode + " - Approved (Guard)";
                emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp;APPROVED (Guard)</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailContentAlreadyApproved + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px;border-color:white'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";
                var carbonCopy = new List<string>();
                foreach (var item in accountingApprover)
                {
                    carbonCopy.Add(item);
                }
                emailObject.CarbonCopy = carbonCopy.ToArray();

                var blindCarbonCopy = new List<string>();
                blindCarbonCopy.Add(item: ConfigurationManager.AppSettings["blind_carbon_copy_email"].ToString());
                emailObject.BlindCarbonCopy = blindCarbonCopy.ToArray();

                emailObject.isHtml = true;

                output = service.SendEmailNotification(emailObject);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }//End



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   notification for gate pass requestor that his/her gate pass was approved.
        */
        public bool SendEmailRejectedByGuard(string emailContentAlreadyApproved, string[] gatePassOwner, List<string> accountingApprover)
        {

            try

            {

                var service = new EmailServiceReference.ServiceClient();
                var emailObject = new EmailServiceReference.EmailObject();
                var output = new EmailServiceReference.Output();

                emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
                emailObject.Alias = "Online Gate Pass";

                var recipient = new List<string>();
                foreach (var item in gatePassOwner)
                {
                    recipient.Add(item);
                }
                emailObject.Recipient = recipient.ToArray();

                emailObject.Subject = "Online Gate Pass : Rejected (Guard)";
                emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp;REJECTED (Guard)</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailContentAlreadyApproved + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px;border-color:white'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";
                var carbonCopy = new List<string>();
                foreach (var item in accountingApprover)
                {
                    carbonCopy.Add(item);
                }
                emailObject.CarbonCopy = carbonCopy.ToArray();

                var blindCarbonCopy = new List<string>();
                blindCarbonCopy.Add(item: ConfigurationManager.AppSettings["blind_carbon_copy_email"].ToString());
                emailObject.BlindCarbonCopy = blindCarbonCopy.ToArray();

                emailObject.isHtml = true;

                output = service.SendEmailNotification(emailObject);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }//End



        /*
         * @author      :   AV <avillena@allegromicro.com>
         * @date        :   DEC. 15, 2016
         * @description :   constract the content and information of email notification
         */
        [HttpGet]
        public string EmailContentRejected(string headercode, string comment, string rejectedbyguardfullname)
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
                str += "<tr><td style='width:15%'><strong></strong></td><td>&nbsp;&nbsp</td><td></td></tr>";
                str += "<tr><td style='width:15%'><strong>Reviewed By</strong></td><td>: &nbsp;&nbsp</td><td>" + rejectedbyguardfullname + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Remarks</strong></td><td>: &nbsp;&nbsp</td><td>" + comment + "</td></tr>";
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
            str += "<br/>";
            str += "<p style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;</p>";

            return str;

        } // End




        /// <summary>
        /// description: display the transaction page view
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                ViewBag.Menu = customHelper.PrepareMenu(child: 11, parent: 0, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Transaction Records";
                ViewBag.PageHeader = "Transaction Records";
                ViewBag.Breadcrumbs = "Home / All Transactions";
                return View();

            }
            catch (Exception)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }
        }// End



        /// <summary>
        /// description: get and display all gatepass transaction for guard view
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllTransGuard()
        {
            try
            {
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = guardModel.GetAllTransGuardcols();

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
                    int totalRows = guardModel.GetAllTransGuardCount(where);

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
                        transactions = guardModel.GetAllTransGuard(offset: 0, next: 0, where: where, sorting: sorting);
                    }
                    else
                    {
                        transactions = guardModel.GetAllTransGuard(start, pagesize, where, sorting);
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
                    resultConfig.Add(key: "TotalRows", value: guardModel.GetAllTransGuardCount(where));
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
        }//End




        /// <summary>
        /// desc: update the item quantity by partial return
        /// by: avillena@allegromicro.com
        /// date: january 16, 2017
        /// </summary>
        /// <returns></returns>
        /// 
        /// <summary>
        /// desc: adding of headercode/gate pass number parameter to display on email notification
        /// by: avillena@allegromicro.com
        /// date: june 5, 2017
        /// version : 2.0
        /// </summary>
        /// <returns></returns>
        #region PartialReturnQuantity
        public JsonResult PartialReturnQuantity()
        {
            try
            {
                var res = new Dictionary<string, object>();
                string[] itemCodeArr = Request["ItemCode"].ToString().Split(separator: new char[] { ',' });
                string[] itemQuantityArr = Request["PartialQuantity"].ToString().Split(separator: new char[] { ',' });
                string[] itemGuidArr = Request["ItemGUID_Arr"].ToString().Split(separator: new char[] { ',' });


                for (int i = 0; i < itemCodeArr.Length; i++)
                {
                    string itemCode = itemCodeArr[i];
                    string itemQuantity = itemQuantityArr[i];
                    string itemGuid = itemGuidArr[i];

                    itemReturnLogsDetails.HeaderCode = Request["HeaderCode"].ToString();
                    itemReturnLogsDetails.ItemCode = itemCode.ToString();
                    itemReturnLogsDetails.Quantity = Convert.ToInt32(itemQuantity);
                    itemReturnLogsDetails.IsActive = true;
                    itemReturnLogsDetails.AddedBy = Convert.ToInt32(Session["userId_local"]);
                    itemReturnLogsDetails.DateAdded = DateTime.Now;
                    itemReturnLogsDetails.Remarks = Request["PartialReturnComment"].ToString();
                    itemReturnLogsDetails.GUID = itemGuid.ToString();
                    guardModel.PartialReturn_Qty(itemReturnLogsDetails);

                }
                ViewBag.headercodeOwnerId = guardModel.GetOwnerofHeader(itemReturnLogsDetails.HeaderCode);
                ViewBag.departmentcodeapprover = guardModel.GetDepartmentCodebaseOnHeader(itemReturnLogsDetails.HeaderCode);
                string[] headercodeOwnerEmail = { guardModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId), ConfigurationManager.AppSettings["ampiguard_email"] };
                string[] accountingEmail = { guardModel.GetAccountingEmailReturnSlip(itemReturnLogsDetails.HeaderCode), guardModel.GetDepartmentEmailReturnSlip(ViewBag.departmentcodeapprover) };
                int getHeaderCodeId = guardModel.getHeaderCodeId(Request["HeaderCode"].ToString());

                // used for audit trail purpose
                auditObj.ProjectCode = "GAT"; // jira story name
                auditObj.Module = "GUARD"; // module name transacted
                auditObj.Operation = "Return"; // action inside the module
                auditObj.Object = "tblTransactionHeaders"; // table name transacted
                auditObj.ObjectId = getHeaderCodeId;
                auditObj.ObjectCode = Request["HeaderCode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                var emailMod = new EmailModels();
                string emailContent = EmailContentReturnSlip(itemReturnLogsDetails.HeaderCode, itemReturnLogsDetails.Remarks);

                if (!SendEmailReturnSlipByGuard(emailContent, headercodeOwnerEmail, accountingEmail, itemReturnLogsDetails.HeaderCode))
                {
                    throw new Exception(message: "Email not sent.");
                }



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
        }// End
        #endregion




        /// <summary>
        /// desc: update the gatepass status to approved and return slip to not returned
        /// by: avillena@allegromicro.com
        /// date: january 23, 2017
        /// </summary>
        /// <returns></returns>

        #region GuardApproved

        public JsonResult GuardApproved()
        {
            try
            {
                string headercode = Request["Id"].ToString();
                string returndate = Request["returndate"].ToString();


                ViewBag.headercodeOwnerId = guardModel.GetOwnerofHeader(headercode);
                string[] headercodeOwnerEmail = { guardModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId) };
                string[] accountingEmail = { guardModel.GetAccountingEmail(headercode), ConfigurationManager.AppSettings["ampiguard_email"] };
                string comment = "";

                int guardIdLocal = Convert.ToInt32(Session["userId_local"]);
                string guardRights = Session["Department"].ToString();
                string guradCode = Session["user_code"].ToString();
                string guradname = Session["cn"].ToString();
                string guardDateAdded = DateTime.Now.ToString();
                bool guardIsApproved = true;
                guardModel.GatePassApproved(headercode, comment, guardIdLocal, guardRights, guradCode, guradname, guardDateAdded, guardIsApproved, returndate);

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT"; // jira story name
                auditObj.Module = "GUARD"; // module transacted
                auditObj.Operation = "Approve"; //action inside the module
                auditObj.Object = "tblTransactionHeaders"; // table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["IdNumber"]);
                auditObj.ObjectCode = Request["Id"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                var emailMod = new EmailModels();
                string emailContent = EmailContent(headercode);

                if (!SendEmailApprovedByGuard(emailContent, headercodeOwnerEmail, accountingEmail, headercode))
                {
                    throw new Exception(message: "Email not sent.");
                }


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
        }// End
        #endregion



        /// <summary>
        /// desc: update the gatepass status to rejected/send email notification
        /// by: avillena@allegromicro.com
        /// date: january 23, 2017
        /// </summary>
        /// <returns></returns>
        #region GuardRejected
        public JsonResult GuardRejected()
        {
            try
            {
                string headercode = Request["Id"].ToString();
                string comment = Request["Remarks"].ToString();
                string returndate = "";
                ViewBag.headercodeOwnerId = guardModel.GetOwnerofHeader(headercode);
                string[] headercodeOwnerEmail = { guardModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId) };

                string[] accountingEmail = { guardModel.GetAccountingEmail(headercode) };

                string rejectbyGuardFullname = Session["cn"].ToString();

                int guardIdLocal = Convert.ToInt32(Session["userId_local"]);
                string guardRights = Session["Department"].ToString();
                string guradCode = Session["user_code"].ToString();
                string guradname = Session["cn"].ToString();
                string guardDateAdded = DateTime.Now.ToString();
                bool guardIsApproved = false;

                guardModel.GatePassApproved(headercode, comment, guardIdLocal, guardRights, guradCode, guradname, guardDateAdded, guardIsApproved, returndate);

                List<string> emailApproverswhoApproved = emailHelper.GetEmailsOfGPWhoWasApproved(headercode);

                emailApproverswhoApproved.Add(ConfigurationManager.AppSettings["ampiguard_email"]);

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT";// jira story name
                auditObj.Module = "GUARD";// module transacted
                auditObj.Operation = "Reject";//action inside the module
                auditObj.Object = "tblTransactionHeaders";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["IdNumber"]);
                auditObj.ObjectCode = Request["Id"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                var emailMod = new EmailModels();
                string emailContent = EmailContentRejected(headercode, comment, rejectbyGuardFullname);

                if (!SendEmailRejectedByGuard(emailContent, headercodeOwnerEmail, emailApproverswhoApproved))
                {
                    throw new Exception(message: "Email not sent.");
                }


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
        }// End
        #endregion



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
            Dictionary<string, string> columns = guardModel.GetAllReturnSlipCols();

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
                int totalRows = guardModel.GetAllTransCount(where, isSearch);

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
                    transactions = guardModel.GetAllReturnSlip(offset: 0, next: 0, where: where, sorting: sorting, isSearch: isSearch);
                }
                else
                {
                    transactions = guardModel.GetAllReturnSlip(start, pagesize, where, sorting, isSearch);
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
                resultConfig.Add(key: "TotalRows", value: guardModel.GetAllTransCount(where, isSearch));
            }

            response.Add(key: "success", value: true);
            response.Add(key: "error", value: false);
            response.Add(key: "message", value: resultConfig);

            return Json(response, JsonRequestBehavior.AllowGet);
        }// End
        #endregion



    }
}