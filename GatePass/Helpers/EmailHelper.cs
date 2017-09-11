using GatePass.Models;
using System.Collections.Generic;
using System.Data;

namespace GatePass.Helpers
{
    public class EmailHelper
    {
        public string EmailTemplate(string content)
        {
            string email_template = "<html>" +
                                            "<head></head>" +
                                            "<body>" +
                                                "<table style='width:80%; font-family:Arial, Helvetica, sans-serif;' align:'center'>" +
                                                    "<tr>" +
                                                        "<th style='height:40px; background-color:#34425A; color:white;text-align: left;'>" +
                                                            "&nbsp;&nbsp ITEMS NOT RETURNED" +
                                                        "</th>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style='border-color:white;text-align: left;background-color:#edeaea'>" + content + "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style='border-color:white;text-align: left; font-size:12'>This is a system-generated email. Please do not reply. Thank you.</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style='height:40px; background-color:#34425A; color:white;text-align: left; font-size:11px'>&nbsp;&nbsp 2016 © AMPI Software Development Group.</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</body>" +
                                        "</html>";
            return email_template;
        }



        public List<List<string>> GetEmailsITandPurchasing(string headerCode)
        {
            ForApprovalModel approvalModel = new ForApprovalModel();

            DataTable approverITandPurch = approvalModel.GetITandPurch(headerCode);
            List<string> emailarrITandPurch = new List<string>();
            List<string> emailApproverCode = new List<string>();
            List<string> emailApprovalType = new List<string>();


            foreach (DataRow row in approverITandPurch.Rows)
            {
                emailarrITandPurch.Add(row["Email"].ToString());
                emailApproverCode.Add(row["ToBeApprovedBy"].ToString());
                emailApprovalType.Add(row["Approvaltype"].ToString());

            }

            List<List<string>> response = new List<List<string>>();
            response.Add(emailarrITandPurch);
            response.Add(emailApproverCode);
            response.Add(emailApprovalType);
            return response;

        }



        public List<List<string>> GetEmailsAccounting(string headerCode)
        {
            ForApprovalModel approvalModel = new ForApprovalModel();

            DataTable approverAccounting = approvalModel.GettheAccountingApprover(headerCode);
            List<string> emailarrAccounting = new List<string>();
            List<string> emailApproverCode = new List<string>();
            List<string> emailApprovalType = new List<string>();

            foreach (DataRow row in approverAccounting.Rows)
            {
                emailarrAccounting.Add(row["Email"].ToString());
                emailApproverCode.Add(row["ToBeApprovedBy"].ToString());
                emailApprovalType.Add(row["Approvaltype"].ToString());

            }

            List<List<string>> response = new List<List<string>>();
            response.Add(emailarrAccounting);
            response.Add(emailApproverCode);
            response.Add(emailApprovalType);

            return response;

        }


        public List<string> GetEmailsOfGPWhoWasApproved(string headerCode)
        {
            var approvalModel = new ForApprovalModel();

            DataTable approverAccounting = approvalModel.GettheEmailWhoApprovedGP(headerCode);
            var emailarrApprover = new List<string>();

            foreach (DataRow row in approverAccounting.Rows)
            {
                emailarrApprover.Add(row["Email"].ToString());

            }

            return emailarrApprover;
        }


        //public List<string> GetEmailsAccounting(string headerCode)
        //{
        //    ForApprovalModel approvalModel = new ForApprovalModel();

        //    DataTable approverAccounting = approvalModel.GettheAccountingApprover(headerCode);
        //    List<string> emailarrAccounting = new List<string>();
        //    foreach (DataRow row in approverAccounting.Rows)
        //    {
        //        emailarrAccounting.Add(row["Email"].ToString());

        //    }

        //    return emailarrAccounting;

        //}


    }
}