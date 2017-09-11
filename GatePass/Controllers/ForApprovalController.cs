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
    public class ForApprovalController : Controller
    {
        private readonly Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly CustomHelper customHelper = new CustomHelper();
        private readonly SupplierModels supplierModel = new SupplierModels();
        private readonly TransactionModels transModel = new TransactionModels();
        private readonly ForApprovalModel forArpprovalModel = new ForApprovalModel();
        private readonly ItemReturnLogsObject itemReturnLogsDetails = new ItemReturnLogsObject();
        private readonly TransactionHeaderObject transHeaderObj = new TransactionHeaderObject();
        private readonly Details transAddItems = new Details();
        private readonly ItemMasterObject itemMasterObj = new ItemMasterObject();
        private readonly UploaderHelper uploaderHelper = new UploaderHelper();
        private readonly GuardModel guardModel = new GuardModel();
        private readonly ApprovalLogsObject approvalLogsObj = new ApprovalLogsObject();
        private readonly EmailHelper emailHelper = new EmailHelper();
        private readonly AuditTrailModels auditModels = new AuditTrailModels();
        private readonly AuditTrailObject auditObj = new AuditTrailObject();
        readonly Library.EncryptDecrypt.EncryptDecryptPassword encrypting = new Library.EncryptDecrypt.EncryptDecryptPassword();

        /*
       * @author      :   AV <avillena@allegromicro.com>
       * @date        :   DEC. 15, 2016
       * @description :   constract the content and information of email notification
       * @version     :   1.0
       */
        [HttpGet]
        public string EmailContent(string headercode, string accountingCode)
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

            str += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='" + ConfigurationManager.AppSettings["link_into_email"].ToString() + headercode + "&approverCode=" + accountingCode + "'>Click here to approve/reject the request</a>";
            str += "<br/>";
            str += "<p style='font-size:12px;font-family:Arial'>&nbsp;&nbsp;&nbsp;&nbsp;<strong>Note :</strong> Best viewed on Google Chrome </p>";

            return str;

        } // End


        /*
     * @author      :   AV <avillena@allegromicro.com>
     * @date        :   DEC. 15, 2016
     * @description :   email content for approved gate pass all approvers
     * @version     :   1.0
     */
        [HttpGet]
        public string EmailContentGatePassAlreadyApproved(string headercode)

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
            str += "<p style='font-size:12px;font-family:Arial'>&nbsp;&nbsp;&nbsp;&nbsp;<strong>Direction :</strong>  Submit your Gate Pass Id on Guard Duty to verify your Gate Pass request.</p>";

            return str;

        } // End



        /*
       * @author      :   AV <avillena@allegromicro.com>
       * @date        :   DEC. 15, 2016
       * @description :   constract the content and information of email notification
       * @version     :   1.0
       */
        [HttpGet]
        public string ApprovalEmailContent(string headercode, string approverCode)
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
            str += "<p style='font-size:12px;font-family:Arial'>&nbsp;&nbsp;&nbsp;&nbsp;<strong>Note :</strong> Best viewed on Google Chrome </p>";

            return str;

        } // End



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   sending of email notification and contract the email template.
        * @version     :   1.0
        */
        public bool SendEmail(List<string> ITandPurch, string emailContent, string gatePassOwner, string approvertype, string headercode)
        {

            try

            {
                var service = new EmailServiceReference.ServiceClient();
                var emailObject = new EmailServiceReference.EmailObject();
                var output = new EmailServiceReference.Output();

                emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
                emailObject.Alias = "Online Gate Pass";

                List<string> recipient = new List<string>();
                foreach (var item in ITandPurch)
                {
                    recipient.Add(item);
                }
                emailObject.Recipient = recipient.ToArray();

                emailObject.Subject = "Gate Pass Id : " + headercode + " - For Approval" + " " + "(" + approvertype + ")" + "";
                emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp;FOR APPROVAL" + " " + "(" + approvertype + ")" + "</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailContent + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";
                var carbonCopy = new List<string>();
                carbonCopy.Add(item: gatePassOwner);
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


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   notification for gate pass requestor that his/her gate pass was approved.
        * @version     :   1.0
        */
        public bool SendEmailForApprovedGatePass(string emailContentAlreadyApproved, string[] gatePassOwner, string headercode)
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

                emailObject.Subject = "Gate Pass Id : " + headercode + " - For Approval (Guard)";
                emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp;FOR APPROVAL (Guard)</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailContentAlreadyApproved + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px;border-color:white'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";
                var carbonCopy = new List<string>();
                carbonCopy.Add(item: ConfigurationManager.AppSettings["ampiguard_email"]);
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
        * @description :   email content for rejected gatepass
        * @version     :   1.0
        */
        [HttpGet]
        public string EmailContentGatePassRejected(string headercode, string approvaltype, string rejectapproverfullname)
        {
            string str = "<table style='font-family:Arial; font-size:12px;width:95%'align='center'>";
            DataTable dtTransheader = transModel.GetGatepassByCode(headercode);
            string remarks = transModel.GetRemarks(headercode, approvaltype);

            foreach (DataRow row in dtTransheader.Rows)
            {

                str += "<tr><td style='width:15%'><strong>Gate Pass ID</strong></td><td>: &nbsp;&nbsp</td><td>" + row["code"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Gate Pass Owner</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Requestor"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Department</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Department"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Return Date</strong></td><td>: &nbsp;&nbsp</td><td>" + row["ReturnDate"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Purpose</strong></td><td>: &nbsp;&nbsp</td><td>" + row["Purpose"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Supplier Name</strong></td><td>: &nbsp;&nbsp</td><td>" + row["SupplierName"] + "</td></tr>";
                str += "<tr><td style='width:15%'><strong></strong></td><td>&nbsp;&nbsp</td><td></td></tr>";
                str += "<tr><td style='width:15%'><strong>Reviewed By</strong></td><td>: &nbsp;&nbsp</td><td>" + rejectapproverfullname + "</td></tr>";
                str += "<tr><td style='width:15%'><strong>Remarks</strong></td><td>: &nbsp;&nbsp</td><td>" + remarks + "</td></tr>";


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

            str += "<p style='font-size:12px'>&nbsp;&nbsp;&nbsp;&nbsp;</p>";

            return str;

        } // End




        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   notification for gate pass requestor that his/her gate pass was approved.
        * @version     :   1.0
        */
        public bool SendEmailForRejectedGatePass(string[] departmentApprover, string emailcontent, string[] headerOwnerEmail, string headercode)
        {
            try

            {
                var service = new EmailServiceReference.ServiceClient();
                var emailObject = new EmailServiceReference.EmailObject();
                var output = new EmailServiceReference.Output();

                emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
                emailObject.Alias = "Online Gate Pass";
                emailObject.Recipient = headerOwnerEmail;

                emailObject.Subject = "Gate Pass Id : " + headercode + " - Rejected (Department Head)";
                emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp;REJECTED (Department)</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailcontent + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px;border-color:white'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";
                emailObject.CarbonCopy = departmentApprover;
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
       * @description :   notification for gate pass rejected
       * @version     :   1.0
       */
        public bool SendEmailForRejectedGatePassIT(string departmentApprover, string emailcontent, string[] headerOwnerEmail, List<string> emailApproverswhoApproved, string rejectedapprovaltype, string headercode)
        {

            try

            {

                var service = new EmailServiceReference.ServiceClient();
                var emailObject = new EmailServiceReference.EmailObject();
                var output = new EmailServiceReference.Output();

                emailObject.Sender = ConfigurationManager.AppSettings["gatepass_email_noreply"].ToString();
                emailObject.Alias = "Online Gate Pass";
                emailObject.Recipient = headerOwnerEmail;

                emailObject.Subject = "Gate Pass Id : " + headercode + " - Rejected " + " " + "(" + rejectedapprovaltype + ")" + "";
                emailObject.Body = "<html><head></head><body><table style='width:100%'><tr><th style='width:100px; border-color:white; text-align: left'></th><th style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma'>&nbsp;&nbsp;REJECTED " + " " + "(" + rejectedapprovaltype + ")" + "</th><th style='width:100px;border-color:white;text-align: left'></th></tr><tr><td style='border-color:white;text-align: left'></td><td style='border-color:white;text-align: left;background-color:#edeaea; border-spacing: 20px 5px;'>" + emailcontent + "</td><td style='border-color:white;text-align: left'></td></tr><tr><td style='border-color:white;text-align: left'></td><td style='height:50px; background-color:#34425A; color:white;text-align: left; font-family:Tahoma; font-size:12px;border-color:white'>&nbsp;&nbsp This is a system-generated email. Please do not reply. Thank you!</td><td style='border-color:white;text-align: left'></td></tr></table></body></html>";
                var carbonCopy = new List<string>();
                foreach (var item in emailApproverswhoApproved)
                    carbonCopy.Add(item);
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
       * @description :   views of department approval
       * @version     :   1.0
       */
        public ActionResult DepartmentApprovalView(string headercode, string approverCode)
        {

            try

            {

                {

                    ViewBag.Menu = customHelper.PrepareMenu(child: 0, parent: 28, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                    ViewBag.Title = "For Approval";
                    ViewBag.PageHeader = "For Approval";
                    ViewBag.Breadcrumbs = "For Approval";

                }

                return View();
            }


            catch (Exception)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }

        }


        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   views of IT related approval
      * @version     :   1.0
      */
        public ActionResult ITRelatedView()
        {

            try

            {
                {

                    ViewBag.Menu = customHelper.PrepareMenu(child: 28, parent: 33, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                    ViewBag.Title = "IT Approval";
                    ViewBag.PageHeader = "IT Approval";
                    ViewBag.Breadcrumbs = "IT Approval";

                }

                return View();
            }
            catch (Exception)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }
        }



        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   views of Purchasing approval
      * @version     :   1.0
      */
        public ActionResult PurchasingRelatedView()
        {
            try

            {
                {

                    ViewBag.Menu = customHelper.PrepareMenu(child: 28, parent: 34, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                    ViewBag.Title = "Purchasing Approval";
                    ViewBag.PageHeader = "Purchasing Approval";
                    ViewBag.Breadcrumbs = "Purchasing Approval";

                }

                return View();
            }
            catch (Exception)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }
        }




        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   views of accounting approval
      * @version     :   1.0
      */
        public ActionResult AccountingApprovalView()
        {
            try

            {
                {

                    ViewBag.Menu = customHelper.PrepareMenu(child: 28, parent: 35, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                    ViewBag.Title = "Accounting Approval";
                    ViewBag.PageHeader = "Accounting Approval";
                    ViewBag.Breadcrumbs = "Accounting Approval";

                }

                return View();
            }
            catch (Exception)
            {
                return View(viewName: "~/Views/Account/login.cshtml");
            }
        }





        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   views of for approval from email link
      * @version     :   1.0
      */
        [HttpGet]
        public ActionResult ForApprovalView(string headercode, string approverCode)
        {
            try

            {
                ViewBag.usercode = Session["user_code"].ToString();
                ViewBag.userdepartment = Session["department"].ToString();
                int ctr = forArpprovalModel.IsHeaderCodeIsExisting(headercode);
                if (ctr == 0 && headercode == null)
                {

                    ViewBag.Menu = customHelper.PrepareMenu(child: 0, parent: 28, usertype: Session["user_type"].ToString(), userId: Int32.Parse(Session["userId_local"].ToString()));
                    ViewBag.Title = "For Approval";
                    ViewBag.PageHeader = "For Approval";
                    ViewBag.Breadcrumbs = "For Approval";

                }
                else if (ctr == 0 && headercode != null)
                {
                    return View(viewName: "~/Views/ForApproval/AccessDenied.cshtml");

                }
                else if (ctr > 0 && approverCode == ViewBag.usercode)
                {

                    int nextapproverId = forArpprovalModel.GatePassIdNextApprover(headercode, approverCode);
                    string gatepassStatus = forArpprovalModel.GetGPStatus(headercode);

                    if (nextapproverId != 0 && gatepassStatus != "Rejected")
                    {
                        ViewBag.ApprovalType = forArpprovalModel.GetTobeApprovedBy(nextapproverId, headercode);
                        ViewBag.impexNumber = forArpprovalModel.GetGatePassFromLink(headercode);
                        ViewBag.TransType = forArpprovalModel.GetGatePassFromLinkTransType(headercode);
                        ViewBag.ReturnDate = forArpprovalModel.GetGatePassReturnDate(headercode);
                        if (ViewBag.ReturnDate == "1/1/1900 12:00:00 AM")
                        {
                            ViewBag.ReturnDate = "";
                        }

                        // get the user name of the requested gate pass
                        string gatepassownerCode = forArpprovalModel.GetgatepassownerCode(headercode);
                        ViewBag.GatePassOwner = forArpprovalModel.GetgatepassownerName(gatepassownerCode);
                        //End
                        ViewBag.Supplier = forArpprovalModel.GetGatePassSupplier(headercode);
                        ViewBag.Attachement = forArpprovalModel.GetGatePassAttachment(headercode);
                        ViewBag.Purpose = forArpprovalModel.GetGatePassPurpose(headercode);
                        ViewBag.ContactName = forArpprovalModel.GetGatePassContactName(headercode);

                        ViewBag.Fromlink = "hidedatagrid";
                        ViewBag.HeaderCode = headercode;

                    }
                    else
                    {
                        ViewBag.GatePassHeaderCode = headercode;
                        ViewBag.HasTransaction = "GPTransacted";
                        return View(viewName: "~/Views/ForApproval/AccessDenied.cshtml");
                    }
                }
                else if (ctr > 0 && approverCode != ViewBag.usercode)
                {
                    return View(viewName: "~/Views/ForApproval/AccessDenied.cshtml");
                }

                return View();
            }
            catch (Exception e)
            {
                ViewBag.ReturnUrl = "/ForApproval/ForApprovalView?headercode=" + headercode + "&approverCode=" + approverCode;
                return View(viewName: "~/Views/Account/login.cshtml");
            }
        }



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   update the approval logs approved by department
        * @version     :   1.0
        */
        #region ApprovedByDepartment
        public JsonResult ApprovedByDepartment()
        {
            try
            {

                string approvaltype = ConfigurationManager.AppSettings["departmentApprover"].ToString();
                approvalLogsObj.Code = Request["headercode"].ToString();
                approvalLogsObj.DateModified = DateTime.Now;

                ViewBag.headercodeOwnerId = forArpprovalModel.GetOwnerofHeader(approvalLogsObj.Code);
                ViewBag.headercodeOwnerEmail = forArpprovalModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId);

                forArpprovalModel.UpdateUpprovalLogsByDept(approvalLogsObj, approvaltype);

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT";// jira story name
                auditObj.Module = "DEPARTMENT";// module transacted
                auditObj.Operation = "Approve";//action inside the module
                auditObj.Object = "tblApprovalLogs";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["id"]);
                auditObj.ObjectCode = Request["headercode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                List<List<string>> approvers = emailHelper.GetEmailsITandPurchasing(approvalLogsObj.Code);
                List<string> approverEmails = approvers[0];
                List<string> approverCode = approvers[1];
                List<string> approverType = approvers[2];

                //get accounting email
                List<List<string>> emailarrAccounting = emailHelper.GetEmailsAccounting(approvalLogsObj.Code);
                List<string> approverEmailsAccounting = emailarrAccounting[0];
                List<string> approverCodeAccounting = emailarrAccounting[1];
                List<string> approverTypeAccounting = emailarrAccounting[2];



                if (approverEmails.Count == 0)
                {
                    for (int i = 0; i < approverEmailsAccounting.Count; i++)
                    {

                        var to = new List<string>();
                        to.Add(approverEmailsAccounting[i]);
                        string emailContent = EmailContent(approvalLogsObj.Code, approverCodeAccounting[i]);

                        if (!SendEmail(to, emailContent, ViewBag.headercodeOwnerEmail, approverTypeAccounting[i], approvalLogsObj.Code))
                        {
                            throw new Exception(message: "Email not sent.");
                        }

                    }

                }
                else
                {
                    var emailMod = new EmailModels();

                    for (int i = 0; i < approverEmails.Count; i++)
                    {

                        List<string> to = new List<string>();
                        to.Add(approverEmails[i]);

                        string emailContent = ApprovalEmailContent(approvalLogsObj.Code, approverCode[i]);

                        if (!SendEmail(to, emailContent, ViewBag.headercodeOwnerEmail, approverType[i], approvalLogsObj.Code))
                        {
                            throw new Exception(message: "Email not sent.");
                        }

                    }
                }





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
        }// End
        #endregion







        /// <summary>
        /// author : avillena
        /// description : approved and update the approval logs approved by IT related
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        #region ApprovedByITRelatedApprover
        public JsonResult ApprovedByITRelatedApprover(string identifier)
        {
            try
            {
                string approvaltype = ConfigurationManager.AppSettings["itApprover"];
                approvalLogsObj.Code = Request["headercode"].ToString();
                approvalLogsObj.DateModified = DateTime.Now;
                ViewBag.headercodeOwnerId = forArpprovalModel.GetOwnerofHeader(approvalLogsObj.Code);
                ViewBag.headercodeOwnerEmail = forArpprovalModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId);

                if (identifier != "override")
                {
                    forArpprovalModel.UpdateUpprovalLogsByDept(approvalLogsObj, approvaltype);
                }

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT";// jira story name
                auditObj.Module = "IT";// module transacted
                auditObj.Operation = "Approve";//action inside the module
                auditObj.Object = "tblApprovalLogs";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["id"]);
                auditObj.ObjectCode = Request["headercode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                int isHaveAPurchasingApprover = forArpprovalModel.IsHaveAPurchasingApprover(approvalLogsObj.Code);
                List<List<string>> emailarrAccounting = emailHelper.GetEmailsAccounting(approvalLogsObj.Code);
                List<string> approverEmailsAccounting = emailarrAccounting[0];
                List<string> approverCode = emailarrAccounting[1];
                List<string> approverTypeAccounting = emailarrAccounting[2];



                if (isHaveAPurchasingApprover == 0)
                {

                    for (int i = 0; i < approverEmailsAccounting.Count; i++)
                    {

                        var to = new List<string>();
                        to.Add(approverEmailsAccounting[i]);
                        string emailContent = EmailContent(approvalLogsObj.Code, approverCode[i]);

                        if (!SendEmail(to, emailContent, ViewBag.headercodeOwnerEmail, approverTypeAccounting[i], approvalLogsObj.Code))
                        {
                            throw new Exception(message: "Email not sent.");
                        }

                    }
                }


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
        }// End
        #endregion



        /// <summary>
        /// author : avillena
        /// description : approved and update the approval logs approved by Purchasing related
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        #region ApprovedByPurchasingRelatedApprover
        public JsonResult ApprovedByPurchasingRelatedApprover(string identifier)
        {
            try
            {
                string approvaltype = ConfigurationManager.AppSettings["purchasingApprover"];
                approvalLogsObj.Code = Request["headercode"].ToString();
                approvalLogsObj.DateModified = DateTime.Now;
                ViewBag.headercodeOwnerId = forArpprovalModel.GetOwnerofHeader(approvalLogsObj.Code);
                ViewBag.headercodeOwnerEmail = forArpprovalModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId);

                if (identifier != "override")
                {
                    forArpprovalModel.UpdateUpprovalLogsByDept(approvalLogsObj, approvaltype);
                }

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT";// jira story name
                auditObj.Module = "PURCHASING";// module transacted
                auditObj.Operation = "Approve";//action inside the module
                auditObj.Object = "tblApprovalLogs";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["id"]);
                auditObj.ObjectCode = Request["headercode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                int isHaveAitApproverandItsApproved = forArpprovalModel.IsHaveA_ITApproverandItsApproved(approvalLogsObj.Code);
                List<List<string>> emailarrAccounting = emailHelper.GetEmailsAccounting(approvalLogsObj.Code);
                List<string> approverEmailsAccounting = emailarrAccounting[0];
                List<string> approverCode = emailarrAccounting[1];
                List<string> approverTypeAccounting = emailarrAccounting[2];


                if (isHaveAitApproverandItsApproved == 0)
                {

                    for (int i = 0; i < approverEmailsAccounting.Count; i++)
                    {

                        var to = new List<string>();
                        to.Add(approverEmailsAccounting[i]);
                        string emailContent = EmailContent(approvalLogsObj.Code, approverCode[i]);

                        if (!SendEmail(to, emailContent, ViewBag.headercodeOwnerEmail, approverTypeAccounting[i], approvalLogsObj.Code))
                        {
                            throw new Exception(message: "Email not sent.");
                        }

                    }
                }

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
        }// End
        #endregion



        /// <summary>
        /// author : avillena
        /// description : approved and update the approval logs approved by Accounting related
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        #region ApprovedByAccountingApprover
        public JsonResult ApprovedByAccountingApprover(string identifier)
        {
            try
            {
                string approvaltype = ConfigurationManager.AppSettings["accountingApprover"];
                approvalLogsObj.Code = Request["headercode"].ToString();
                approvalLogsObj.DateModified = DateTime.Now;
                ViewBag.headercodeOwnerId = forArpprovalModel.GetOwnerofHeader(approvalLogsObj.Code);
                string[] headercodeOwnerEmail = { forArpprovalModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId) };

                if (identifier != "override")
                {
                    forArpprovalModel.UpdateUpprovalLogsByDept(approvalLogsObj, approvaltype);
                }

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT"; // Jira story name
                auditObj.Module = "ACCOUNTING";// module transacted
                auditObj.Operation = "Approve";//action inside the module
                auditObj.Object = "tblApprovalLogs";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["id"]);
                auditObj.ObjectCode = Request["headercode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                var emailMod = new EmailModels();
                string emailContentAlreadyApproved = EmailContentGatePassAlreadyApproved(approvalLogsObj.Code);

                if (!SendEmailForApprovedGatePass(emailContentAlreadyApproved, headercodeOwnerEmail, approvalLogsObj.Code))
                {
                    throw new Exception(message: "Email not sent.");
                }



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
        }// End
        #endregion



        /// <summary>
        /// description: get and display all gatepass transaction for department approval
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllTransForApproval()
        {
            try
            {
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = forArpprovalModel.GetAllTransForApprovalcols();

                //check for filters
                string where = "";
                string userdept = Session["department"].ToString();
                string userlogincode = Session["user_code"].ToString();

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
                    int totalRows = forArpprovalModel.GetAllTransForApprovalCount(where, userdept, userlogincode);

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
                        transactions = forArpprovalModel.GetAllTransForApproval(offset: 0, next: 0, where: where, sorting: sorting, userdept: userdept, userlogincode: userlogincode);
                    }
                    else
                    {
                        transactions = forArpprovalModel.GetAllTransForApproval(start, pagesize, where, sorting, userdept, userlogincode);
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
                    resultConfig.Add(key: "TotalRows", value: forArpprovalModel.GetAllTransForApprovalCount(where, userdept, userlogincode));
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
        /// description: get and display all gatepass transaction for IT related approval
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllForITRelatedApproval()
        {
            try
            {
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = forArpprovalModel.GetAllTransForApprovalcols();

                //check for filters
                string where = "";
                string userdept = Session["department"].ToString();
                string userlogincode = Session["user_code"].ToString();

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
                    int totalRows = forArpprovalModel.GetAllTransForITApprovalCount(where, userlogincode);

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
                        transactions = forArpprovalModel.GetAllTransForITRelatedApproval(offset: 0, next: 0, where: where, sorting: sorting, userlogincode: userlogincode);
                    }
                    else
                    {
                        transactions = forArpprovalModel.GetAllTransForITRelatedApproval(start, pagesize, where, sorting, userlogincode);
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
                    resultConfig.Add(key: "TotalRows", value: forArpprovalModel.GetAllTransForITApprovalCount(where, userlogincode));
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
        /// description: get and display all gatepass transaction for purchasing approval
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllForPurchasingRelatedApproval()
        {
            try
            {
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = forArpprovalModel.GetAllTransForApprovalcols();

                //check for filters
                string where = "";
                string userdept = Session["department"].ToString();
                string userlogincode = Session["user_code"].ToString();

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
                    int totalRows = forArpprovalModel.GetAllTransForPurchasingApprovalCount(where, userlogincode);

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
                        transactions = forArpprovalModel.GetAllTransForPurchasingRelatedApproval(offset: 0, next: 0, where: where, sorting: sorting, userlogincode: userlogincode);
                    }
                    else
                    {
                        transactions = forArpprovalModel.GetAllTransForPurchasingRelatedApproval(start, pagesize, where, sorting, userlogincode);
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
                    resultConfig.Add(key: "TotalRows", value: forArpprovalModel.GetAllTransForPurchasingApprovalCount(where, userlogincode));
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
        /// description: get and display all gatepass transaction for accounting approval
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllForAccountingApproval()
        {
            try
            {
                //create a dictionary that will contain the column + datafied config for the grid
                var resultConfig = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = forArpprovalModel.GetAllTransForApprovalcols();

                //check for filters
                string where = "";
                string userdept = Session["department"].ToString();
                string userlogincode = Session["user_code"].ToString();

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
                    int totalRows = forArpprovalModel.GetAllTransForAccountingApprovalCount(where, userlogincode);

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
                        transactions = forArpprovalModel.GetAllTransForAccountingRelatedApproval(offset: 0, next: 0, where: where, sorting: sorting, userlogincode: userlogincode);
                    }
                    else
                    {
                        transactions = forArpprovalModel.GetAllTransForAccountingRelatedApproval(start, pagesize, where, sorting, userlogincode);
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
                    resultConfig.Add(key: "TotalRows", value: forArpprovalModel.GetAllTransForAccountingApprovalCount(where, userlogincode));
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
        /// description: reject by department approver/send email notification
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        #region RejectedByDepartment
        public JsonResult RejectedByDepartment()
        {
            try
            {
                string approvaltype = ConfigurationManager.AppSettings["departmentApprover"].ToString();
                approvalLogsObj.RejectedBy = Convert.ToInt32(Session["userId_local"]);
                approvalLogsObj.Code = Request["headercode"].ToString();
                approvalLogsObj.Remarks = Request["remarks"].ToString();
                approvalLogsObj.DateRejected = DateTime.Now;

                string[] deparmentApproverEmail = { forArpprovalModel.GetDeptApproverEmail(approvalLogsObj.RejectedBy) };
                string rejectApproverFullName = forArpprovalModel.GetApproverFullname(deparmentApproverEmail[0]);

                ViewBag.headercodeOwnerId = forArpprovalModel.GetOwnerofHeader(approvalLogsObj.Code);
                string[] headercodeOwnerEmail = { forArpprovalModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId) };
                forArpprovalModel.UpdateUpprovalLogsByDept_Rejected(approvalLogsObj, approvaltype);

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT";// jira story name
                auditObj.Module = "DEPARTMENT";// module transacted
                auditObj.Operation = "Reject";//action inside the module
                auditObj.Object = "tblApprovalLogs";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["id"]);
                auditObj.ObjectCode = Request["headercode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                var emailMod = new EmailModels();
                string emailContent = EmailContentGatePassRejected(approvalLogsObj.Code, approvaltype, rejectApproverFullName);

                if (!SendEmailForRejectedGatePass(deparmentApproverEmail, emailContent, headercodeOwnerEmail, approvalLogsObj.Code))
                {
                    throw new Exception(message: "Email not sent.");
                }

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
        }// End
        #endregion


        /// <summary>
        /// description: reject by IT approver/send email notification
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        #region RejectedByITRelated
        public JsonResult RejectedByITRelated()
        {
            try
            {
                string approvaltype = ConfigurationManager.AppSettings["itApprover"];
                approvalLogsObj.RejectedBy = Convert.ToInt32(Session["userId_local"]);
                approvalLogsObj.Code = Request["headercode"].ToString();
                approvalLogsObj.Remarks = Request["remarks"].ToString();
                approvalLogsObj.DateRejected = DateTime.Now;

                string deparmentApproverEmail = forArpprovalModel.GetDeptApproverEmail(approvalLogsObj.RejectedBy);

                string rejectApproverFullName = forArpprovalModel.GetApproverFullname(deparmentApproverEmail);

                string rejectedapprovaltype = "IT";

                ViewBag.headercodeOwnerId = forArpprovalModel.GetOwnerofHeader(approvalLogsObj.Code);
                string[] headercodeOwnerEmail = { forArpprovalModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId) };

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT";// jira story name
                auditObj.Module = "IT";// module transacted
                auditObj.Operation = "Reject";//action inside the module
                auditObj.Object = "tblApprovalLogs";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["id"]);
                auditObj.ObjectCode = Request["headercode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                forArpprovalModel.UpdateUpprovalLogsByDept_Rejected(approvalLogsObj, approvaltype);

                List<string> emailApproverswhoApproved = emailHelper.GetEmailsOfGPWhoWasApproved(approvalLogsObj.Code);

                emailApproverswhoApproved.Add(item: deparmentApproverEmail);
                var emailMod = new EmailModels();
                string emailContent = EmailContentGatePassRejected(approvalLogsObj.Code, approvaltype, rejectApproverFullName);

                if (!SendEmailForRejectedGatePassIT(deparmentApproverEmail, emailContent, headercodeOwnerEmail, emailApproverswhoApproved, rejectedapprovaltype, approvalLogsObj.Code))
                {
                    throw new Exception(message: "Email not sent.");
                }

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
        }// End
        #endregion


        /// <summary>
        /// description: reject by purchsing approver/send email notification
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        #region RejectedByPurchasingRelated
        public JsonResult RejectedByPurchasingRelated()
        {
            try
            {
                string approvaltype = ConfigurationManager.AppSettings["purchasingApprover"];
                approvalLogsObj.RejectedBy = Convert.ToInt32(Session["userId_local"]);
                approvalLogsObj.Code = Request["headercode"].ToString();
                approvalLogsObj.Remarks = Request["remarks"].ToString();
                approvalLogsObj.DateRejected = DateTime.Now;

                string approverEmailTo = forArpprovalModel.GetDeptApproverEmail(approvalLogsObj.RejectedBy);
                string rejectApproverFullName = forArpprovalModel.GetApproverFullname(approverEmailTo);

                ViewBag.headercodeOwnerId = forArpprovalModel.GetOwnerofHeader(approvalLogsObj.Code);
                string[] headercodeOwnerEmail = { forArpprovalModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId) };


                // used for audit trail purposes
                auditObj.ProjectCode = "GAT";// jira story name
                auditObj.Module = "PURCHASING";// module transacted
                auditObj.Operation = "Reject";//action inside the module
                auditObj.Object = "tblApprovalLogs";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["id"]);
                auditObj.ObjectCode = Request["headercode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                forArpprovalModel.UpdateUpprovalLogsByDept_Rejected(approvalLogsObj, approvaltype);



                List<string> emailApproverswhoApproved = emailHelper.GetEmailsOfGPWhoWasApproved(approvalLogsObj.Code);

                emailApproverswhoApproved.Add(item: approverEmailTo);
                string rejectedapprovaltype = "Purchasing";

                var emailMod = new EmailModels();
                string emailContent = EmailContentGatePassRejected(approvalLogsObj.Code, approvaltype, rejectApproverFullName);

                if (!SendEmailForRejectedGatePassIT(approverEmailTo, emailContent, headercodeOwnerEmail, emailApproverswhoApproved, rejectedapprovaltype, approvalLogsObj.Code))
                {
                    throw new Exception(message: "Email not sent.");
                }

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
        }// End
        #endregion



        /// <summary>
        /// description: reject by accounting approver/send email notification
        /// by: avillena@allegromicro.com
        /// date: january 10 , 2017
        /// </summary>
        /// <returns></returns>
        #region RejectedByAccountingApprover
        public JsonResult RejectedByAccountingApprover()
        {
            try
            {
                string approvaltype = ConfigurationManager.AppSettings["accountingApprover"];
                approvalLogsObj.RejectedBy = Convert.ToInt32(Session["userId_local"]);
                approvalLogsObj.Code = Request["headercode"].ToString();
                approvalLogsObj.Remarks = Request["remarks"].ToString();
                approvalLogsObj.DateRejected = DateTime.Now;

                string approverEmailTo = forArpprovalModel.GetDeptApproverEmail(approvalLogsObj.RejectedBy);
                string rejectApproverFullName = forArpprovalModel.GetApproverFullname(approverEmailTo);


                ViewBag.headercodeOwnerId = forArpprovalModel.GetOwnerofHeader(approvalLogsObj.Code);
                string[] headercodeOwnerEmail = { forArpprovalModel.GetOwnerofHeaderEmail(ViewBag.headercodeOwnerId) };
                string rejectedapprovaltype = "Accounting";
                forArpprovalModel.UpdateUpprovalLogsByDept_Rejected(approvalLogsObj, approvaltype);

                // used for audit trail purposes
                auditObj.ProjectCode = "GAT";// jira story name
                auditObj.Module = "ACCOUNTING";// module transacted
                auditObj.Operation = "Reject";//action inside the module
                auditObj.Object = "tblApprovalLogs";// table name transacted
                auditObj.ObjectId = Convert.ToInt32(Request["id"]);
                auditObj.ObjectCode = Request["headercode"].ToString();
                auditObj.UserCode = Convert.ToInt32(Session["userId_local"]);
                auditObj.IP = CustomHelper.GetLocalIPAddress();
                auditObj.MAC = CustomHelper.GetMACAddress();
                auditObj.DateAdded = DateTime.Now;

                auditModels.AddAuditTrail(auditObj);

                List<string> emailApproverswhoApproved = emailHelper.GetEmailsOfGPWhoWasApproved(approvalLogsObj.Code);
                emailApproverswhoApproved.Add(item: approverEmailTo);

                var emailMod = new EmailModels();
                string emailContent = EmailContentGatePassRejected(approvalLogsObj.Code, approvaltype, rejectApproverFullName);

                if (!SendEmailForRejectedGatePassIT(approverEmailTo, emailContent, headercodeOwnerEmail, emailApproverswhoApproved, rejectedapprovaltype, approvalLogsObj.Code))
                {
                    throw new Exception(message: "Email not sent.");
                }

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
        }// End
        #endregion



        /// <summary>
        /// @author avillena
        /// @description :  to get the gate pass owner display to the for approval
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        public ActionResult GetGatePassOwner()
        {

            try
            {
                string headercode = Request["headercode"].ToString();
                string gatepassownerCode = forArpprovalModel.GetgatepassownerCode(headercode);
                string gatePassOwnerName = forArpprovalModel.GetgatepassownerName(gatepassownerCode);
                //End

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", gatePassOwnerName);
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