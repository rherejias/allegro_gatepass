using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class OverrideModels
    {
        //rherejias
        public Dictionary<string, string> GetOverrideData_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("TransHeaderCode", "trans_header_code_string");
            cols.Add("ApprovalTypeCode", "approval_type_code_string");
            cols.Add("ApprovalType", "approval_type_string");
            cols.Add("IsApproved", "isapproved_string");
            cols.Add("DateApproved", "date_approved_datetime");
            cols.Add("ToBeApprovedBy", "to_be_approved_by_string");
            cols.Add("ApproverName", "approver_name_string");
            cols.Add("ApprovedBy", "approved_by_string");
            cols.Add("DateOverriden", "date_overriden_datetime");
            cols.Add("AddedBy", "added_by_string");
            cols.Add("DateAdded", "date_added_datetime");
            cols.Add("Remarks", "remarks_string");
            cols.Add("IsOverride", "isoverride_string");
            cols.Add("Email", "email_string");

            return cols;
        }

        public DataTable GetOverrideData(int offset, int next, string where, string sorting, string searchStr)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetOverrideData_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "TransHeaderCode ASC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = " WHERE id = ''";
            }
            else
            {
                where += " AND IsApproved is null";
            }



            //set pagination
            string pagination = "";
            if (offset != 0 && next != 0)
            {
                if (next > 0)
                {
                    pagination = "OFFSET " + offset + " ROWS FETCH NEXT " + next + " ROWS ONLY";
                }
            }


            //start building the sql statement
            string sql = "SELECT ";

            //place the columns in the sql string
            foreach (var item in cols)
            {
                sql += item.Key + " AS " + item.Value + ", ";
            }

            //remove the trailing " ," from the right
            sql = sql.Substring(0, sql.Length - 2);

            //set the table name and other params
            sql += " FROM vwOverride " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   12/27/2016 8:00 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetOverrideData_count(string where, string searchStr)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = " WHERE id = ''";
            }
            else
            {
                where += " AND IsApproved is null";
            }



            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwOverride " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        /// <summary>
        /// for override
        /// </summary>
        /// <param name="approvalLog">from controller</param>
        /// <returns>boolean true or false</returns>
        /// @ver @author rherejias 2/10/2017 
        public bool Override(ApprovalLogsObject approvalLog)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("id", approvalLog.Id),
                new SqlParameter("code", approvalLog.Code),
                new SqlParameter("isapproved", approvalLog.IsApproved),
                new SqlParameter("approvedby", approvalLog.ApprovedBy),
                new SqlParameter("dateapproved", approvalLog.DateApproved),
                new SqlParameter("remarks", approvalLog.Remarks),
                new SqlParameter("isoverride", approvalLog.IsOverride),
                new SqlParameter("IP", approvalLog.IP),
                new SqlParameter("MAC", approvalLog.MAC)


            };
            return Library.ConnectionString.returnCon.executeQuery("spOverride", params_, CommandType.StoredProcedure);
        }

        /// <summary>
        /// for reject
        /// </summary>
        /// <param name="approvalLog">from controller</param>
        /// <returns>boolean true or false</returns>
        /// @ver @author rherejias 2/10/2017 
        public bool Reject(ApprovalLogsObject approvalLog)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("id", approvalLog.Id),
                new SqlParameter("headercode", approvalLog.TransHeaderCode),
                new SqlParameter("isapproved", approvalLog.IsApproved),
                new SqlParameter("rejectedby", approvalLog.RejectedBy),
                new SqlParameter("daterejected", approvalLog.DateRejected),
                new SqlParameter("remarks", approvalLog.Remarks),
                new SqlParameter("isoverride", approvalLog.IsOverride),
                new SqlParameter("IP", approvalLog.IP),
                new SqlParameter("MAC", approvalLog.MAC)
            };
            return Library.ConnectionString.returnCon.executeQuery("spOverrideReject", params_, CommandType.StoredProcedure);
        }

        /// <summary>
        /// query to get remarks from tblApprovalLogs
        /// </summary>
        /// <param name="headerCode">will be used in where clause</param>
        /// <param name="approvalType">will be used in where clasue</param>
        /// <returns>string remarks</returns>
        /// @ver @author rherejias 2/10/2017 
        public string GetRemarks(string headerCode, string approvalType)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select Remarks from tblApprovalLogs Where TransHeaderCode = '" + headerCode + "' AND ApprovalTypeCode = '" + approvalType + "';", CommandType.Text).ToString();
        }

        /// <summary>
        /// query to get requestor
        /// </summary>
        /// <param name="headerCode">will be used in where clause</param>
        /// <param name="approvalType">will be used in where clause</param>
        /// <returns>string requestor</returns>
        /// @ver @author rherejias 2/10/2017 
        public string GetRequestor(string headerCode, string approvalType)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select Remarks from tblApprovalLogs Where TransHeaderCode = '" + headerCode + "' AND ApprovalTypeCode = '" + approvalType + "';", CommandType.Text).ToString();
        }

        /// <summary>
        /// query to get full name of user
        /// </summary>
        /// <param name="userID">will be used in where clause</param>
        /// <returns>string fullname</returns>
        /// @ver @author rherejias 2/10/2017 
        public string GetName(string userID)
        {
            string lastname = Library.ConnectionString.returnCon.executeScalarQuery("Select LastName from tblUsers Where Id = '" + userID + "';", CommandType.Text).ToString();
            string firstname = Library.ConnectionString.returnCon.executeScalarQuery("Select GivenName from tblUsers Where Id = '" + userID + "';", CommandType.Text).ToString();
            string fullname = lastname + ", " + firstname;

            return fullname;
        }

        /// <summary>
        /// query to get approver emails
        /// </summary>
        /// <param name="headerCode">will be used in where clause</param>
        /// <param name="approvalType">will be used in where clause</param>
        /// <returns>datatable rows</returns>
        /// @ver @author rherejias 2/10/2017 
        public DataTable GetApproverEmails(string headerCode, string approvalType)
        {
            string query = "";
            long approvalTypeDept = 0;
            if (approvalType == ConfigurationManager.AppSettings["departmentApprover"].ToString())
            {
                query = "select Email from vwOverride where TransHeaderCode = '" + headerCode + "' AND ApprovalTypeCode = '" + approvalType + "'";
            }
            else if (approvalType == ConfigurationManager.AppSettings["itApprover"])
            {
                approvalTypeDept = Convert.ToInt64(approvalType) - 1;
                query = "select Email from vwOverride where TransHeaderCode = '" + headerCode + "' AND (ApprovalTypeCode = '" + approvalType + "' OR ApprovalTypeCode = '" + approvalTypeDept.ToString() + "')";
            }
            else if (approvalType == ConfigurationManager.AppSettings["purchasingApprover"])
            {
                approvalTypeDept = Convert.ToInt64(approvalType) - 2;
                query = "select Email from vwOverride where TransHeaderCode = '" + headerCode + "' AND (ApprovalTypeCode = '" + approvalType + "' OR ApprovalTypeCode = '" + approvalTypeDept.ToString() + "')";
            }
            else
            {
                query = "Select Email from vwOverride Where TransHeaderCode = '" + headerCode + "';";
            }

            return Library.ConnectionString.returnCon.executeSelectQuery(query, CommandType.Text);

        }

        /// <summary>
        /// query to get approver emails when rejecting
        /// </summary>
        /// <param name="headerCode">will be used in where clause</param>
        /// <param name="approvalType">will be used in where clause</param>
        /// <returns>datatable rows</returns>
        /// 
        public DataTable GetApproverEmailsReject(string headerCode, string approvalType)
        {
            return Library.ConnectionString.returnCon.executeSelectQuery("Select Email from vwOverride Where TransHeaderCode = '" + headerCode + "';", CommandType.Text);
        }

        /// <summary>
        /// query to get requestor
        /// </summary>
        /// <param name="headerCode">will be used in where clause</param>
        /// <returns>string name of requestor</returns>
        public string GetRequestor(string headerCode)
        {
            string userID = Library.ConnectionString.returnCon.executeScalarQuery("Select AddedBy from tblTransactionDetails Where HeaderCode = '" + headerCode + "';", CommandType.Text).ToString();
            string email = Library.ConnectionString.returnCon.executeScalarQuery("Select Email from tblUsers Where Id = '" + userID + "';", CommandType.Text).ToString();

            return email;
        }

        /// <summary>
        /// query to get approver details
        /// </summary>
        /// <param name="headercode">will be used in where clause</param>
        /// <returns>table rows</returns>
        /// @ver @author rherejias 2/10/2017 
        public DataTable GetAprrover(string headercode)
        {
            return Library.ConnectionString.returnCon.executeSelectQuery("Select [Id],[TransHeaderCode],[ApprovalTypeCode],COALESCE([IsApproved],0) as IsApproved from tblApprovalLogs Where TransHeaderCode = '" + headercode + "';", CommandType.Text);
        }

        /// <summary>
        /// for approvallogs count
        /// </summary>
        /// <param name="headercode">will be used in where clause</param>
        /// <returns>string count</returns>
        /// @ver 1.0 @author rherejias 2/14/2017
        public int GetApprovalLogsCount(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery("Select COUNT(*) from tblApprovalLogs WHERE TransheaderCode = " + headercode + "", CommandType.Text));
        }

        /// <summary>
        /// for puchasing email
        /// </summary>
        /// <param name="headercode">will be used in where clause</param>
        /// <returns>string email</returns>
        /// @ver 1.0 @author rherejias 2/14/2017
        public string GetPurchasingEmail(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select Email from vwOverride WHERE ApprovalTypeCode = '" + ConfigurationManager.AppSettings["purchasingApprover"] + "' AND TransheaderCode = '" + headercode + "'", CommandType.Text).ToString();
        }

        /// <summary>
        /// for IT email
        /// </summary>
        /// <param name="headercode">will be used in where clause</param>
        /// <returns>string email</returns>
        /// @ver 1.0 @author rherejias 2/14/2017
        public string GetITEmail(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select Email from vwOverride WHERE ApprovalTypeCode = '" + ConfigurationManager.AppSettings["itApprover"] + "' AND TransheaderCode = '" + headercode + "'", CommandType.Text).ToString();
        }

        /// <summary>
        /// for Acct email
        /// </summary>
        /// <param name="headercode">will be used in where clause</param>
        /// <returns>string email</returns>
        /// @ver 1.0 @author rherejias 2/14/2017
        public string GetAcctEmail(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select Email from vwOverride WHERE ApprovalTypeCode = '" + ConfigurationManager.AppSettings["accountingApprover"] + "' AND TransheaderCode = '" + headercode + "'", CommandType.Text).ToString();
        }

        /// <summary>
        /// for purchasing is Approved
        /// </summary>
        /// <param name="headercode">will be used in where clause</param>
        /// <returns>string email</returns>
        /// @ver 1.0 @author rherejias 2/14/2017
        public string GetIsApprovedPurch(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select COALESCE(IsApproved,0) from vwOverride WHERE ApprovalTypeCode = '" + ConfigurationManager.AppSettings["purchasingApprover"] + "' AND TransheaderCode = '" + headercode + "'", CommandType.Text).ToString();
        }

        /// <summary>
        /// for purchasing is Approved
        /// </summary>
        /// <param name="headercode">will be used in where clause</param>
        /// <returns>string email</returns>
        /// @ver 1.0 @author rherejias 2/14/2017
        public string GetIsApprovedIT(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select COALESCE(IsApproved,0) from vwOverride WHERE ApprovalTypeCode = '" + ConfigurationManager.AppSettings["itApprover"] + "' AND TransheaderCode = '" + headercode + "'", CommandType.Text).ToString();
        }

        /// <summary>
        /// for purchasing is Approved
        /// </summary>
        /// <param name="headercode">will be used in where clause</param>
        /// <returns>string email</returns>
        /// @ver 1.0 @author rherejias 2/14/2017
        public string GetUserCode(string email)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select Code from tblUsers WHERE Email = '" + email + "'", CommandType.Text).ToString();
        }
    }
}