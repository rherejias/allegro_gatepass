using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class ApproverModels
    {
        //rherejias
        public Dictionary<string, string> GetApprovers_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("ApprovalTypeCode", "approval_type_code_string");
            cols.Add("ApprovalType", "approval_type_string");
            cols.Add("DepartmentCode", "department_code_string");
            cols.Add("[User]", "user_string");
            cols.Add("UserCode", "user_code_string");
            cols.Add("Department", "department_string");
            cols.Add("Email", "email_string");
            cols.Add("IsActive", "active_bool");

            return cols;
        }

        public DataTable GetApprovers(int offset, int next, string where, string sorting, string searchStr, string IsActive, string type)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetApprovers_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "User ASC";
            }

            if (type == ConfigurationManager.AppSettings["departmentApprover"].ToString())
            {
                if (where == null || where == "" || where == string.Empty)
                    where = "WHERE IsActive =" + IsActive + " AND ApprovalTypeCode = " + type;
                else
                    where += " AND IsActive =" + IsActive + " AND ApprovalTypeCode = " + type;
            }
            else if (type == ConfigurationManager.AppSettings["itApprover"])
            {
                if (where == null || where == "" || where == string.Empty)
                    where = "WHERE IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryITApprover"] + "')";
                else
                    where += " AND IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryITApprover"] + "')";
            }
            else if (type == ConfigurationManager.AppSettings["purchasingApprover"])
            {
                if (where == null || where == "" || where == string.Empty)
                    where = "WHERE IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryPurchasingApprover"] + "')";
                else
                    where += " AND IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryPurchasingApprover"] + "')";
            }
            else if (type == ConfigurationManager.AppSettings["accountingApprover"])
            {
                if (where == null || where == "" || where == string.Empty)
                    where = "WHERE IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryAccountingApprover"] + "')";
                else
                    where += " AND IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryAccountingApprover"] + "')";
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
            sql += " FROM vwApprovers " + where + " ORDER BY " + sorting + " " + pagination + ";";

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
        public int GetApprovers_count(string where, string searchStr, string IsActive, string type)
        {
            if (type == ConfigurationManager.AppSettings["departmentApprover"].ToString())
            {
                if (where == null || where == "" || where == string.Empty)
                    where = "WHERE IsActive =" + IsActive + " AND ApprovalTypeCode = " + type;
                else
                    where += " AND IsActive =" + IsActive + " AND ApprovalTypeCode = " + type;
            }
            else if (type == ConfigurationManager.AppSettings["itApprover"])
            {
                if (where == null || where == "" || where == string.Empty)
                    where = "WHERE IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryITApprover"] + "')";
                else
                    where += " AND IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryITApprover"] + "')";
            }
            else if (type == ConfigurationManager.AppSettings["purchasingApprover"])
            {
                if (where == null || where == "" || where == string.Empty)
                    where = "WHERE IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryPurchasingApprover"] + "')";
                else
                    where += " AND IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryPurchasingApprover"] + "')";
            }
            else if (type == ConfigurationManager.AppSettings["accountingApprover"])
            {
                if (where == null || where == "" || where == string.Empty)
                    where = "WHERE IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryAccountingApprover"] + "')";
                else
                    where += " AND IsActive =" + IsActive + " AND (ApprovalTypeCode = " + type + " OR ApprovalTypeCode =  '" + ConfigurationManager.AppSettings["secondaryAccountingApprover"] + "')";
            }

            //build the sql statement
            //ver 1.0 rherejias the short hand if code is open for modification if you have better code please modify.
            string sql = "SELECT COUNT(*) FROM vwApprovers " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        /// <summary>
        /// for adding approver details
        /// </summary>
        /// <param name="approver">object that hold data from controller</param>
        /// <returns>boolean true or false</returns>
        /// @ver 1.0 @author rherejias 1/18/2017 
        public bool addApprover(AppoverObject approver)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("@ApprovalType", approver.ApprovalTypeCode),
                new SqlParameter("@Department", approver.DepartmentCode),
                new SqlParameter("@User", approver.UserCode),
                new SqlParameter("@DateAdded", approver.DateAdded),
                new SqlParameter("@AddedBy", approver.AddedBy),
                new SqlParameter("@IsActive", approver.IsActive),
                new SqlParameter("@IP", approver.IP),
                new SqlParameter("@MAC", approver.MAC)
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddApprover", params_, CommandType.StoredProcedure);

        }

        /// <summary>
        /// for updating approver details
        /// </summary>
        /// <param name="approver">object that hold data from controller</param>
        /// <returns>boolean true or false</returns>
        /// @ver 1.0 @author rherejias 1/18/2017 
        public bool editApprover(AppoverObject approver)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("@id", approver.Id),
                new SqlParameter("@Code", approver.Code),
                new SqlParameter("@DepartmentInput", approver.DepartmentCode),
                new SqlParameter("@UserInput", approver.UserCode),
                new SqlParameter("@AddedBy", approver.AddedBy),
                new SqlParameter("@IP", approver.IP),
                new SqlParameter("@MAC", approver.MAC),
                new SqlParameter("@DateAdded", approver.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateApprover", params_, CommandType.StoredProcedure);

        }

        /// <summary>
        /// for de-activating approver
        /// </summary>
        /// <param name="approver">object that hold data from controller</param>
        /// <returns>boolean true or false</returns>
        /// @ver 1.0 @author rherejias 1/18/2017 
        public bool inactiveApprover(AppoverObject approver)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("@id", approver.Id),
                new SqlParameter("@Code", approver.Code),
                new SqlParameter("@table", "tblApprovalDepartmentRelationship"),
                new SqlParameter("@IsActive", "false"),
                new SqlParameter("@DateAdded", approver.DateAdded),
                new SqlParameter("@Module", "APPROVER"),
                new SqlParameter("@IP", approver.IP),
                new SqlParameter("@MAC", approver.MAC),
                new SqlParameter("@AddedBy", approver.AddedBy)
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);

        }

        /// <summary>
        /// for re-activating approver
        /// </summary>
        /// <param name="approver">object that hold data from controller</param>
        /// <returns>boolean true or false</returns>
        /// @ver 1.0 @author rherejias 1/18/2017 
        public bool ActiveApprover(AppoverObject approver)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("@id", approver.Id),
                new SqlParameter("@Code", approver.Code),
                new SqlParameter("@table", "tblApprovalDepartmentRelationship"),
                new SqlParameter("@DateAdded", approver.DateAdded),
                new SqlParameter("@Module", "APPROVER"),
                new SqlParameter("@IP", approver.IP),
                new SqlParameter("@MAC", approver.MAC),
                new SqlParameter("@AddedBy", approver.AddedBy)
            };

            return Library.ConnectionString.returnCon.executeQuery("spActivateItem", params_, CommandType.StoredProcedure);

        }

        /// <summary>
        /// for assigning approver
        /// </summary>
        /// <param name="approver">object that hold data from controller</param>
        /// @ver 1.0 @author rherejias 2/6/2017 
        public bool AssignApprover(AppoverObject approver)
        {
            string sp = "";
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("@Id", approver.Id),
                new SqlParameter("@Code", approver.Code),
                new SqlParameter("@ApprovalType",approver.ApprovalTypeCode),
                new SqlParameter("@Department", approver.DepartmentCode),
                new SqlParameter("@DateAdded", approver.DateAdded),
                new SqlParameter("@AddedBy", approver.AddedBy),
                new SqlParameter("@IP", approver.IP),
                new SqlParameter("@MAC", approver.MAC),
                new SqlParameter("@Module", approver.Module)
            };
            if (approver.ApprovalTypeCode == ConfigurationManager.AppSettings["secondaryITApprover"])
                sp = "spAssignIT";
            else if (approver.ApprovalTypeCode == ConfigurationManager.AppSettings["secondaryPurchasingApprover"])
                sp = "spAssignPurchasing";
            else if (approver.ApprovalTypeCode == ConfigurationManager.AppSettings["secondaryAccountingApprover"])
                sp = "spAssignAccounting";

            return Library.ConnectionString.returnCon.executeQuery(sp, params_, CommandType.StoredProcedure);

        }

        /// <summary>
        /// check if primary approver already exist on tbldepartmentrelationship
        /// </summary>
        /// <param name="deptCode">will be used to the where clause</param>
        /// <returns>integer long bec. approvaltype code requires long integer</returns>
        /// @ver 1.0 @author rherejias 1/18/2017
        public long isPrimaryExist(string deptCode)
        {
            return Convert.ToInt64(Library.ConnectionString.returnCon.executeScalarQuery
                ("select [ApprovalTypeCode] from tblApprovalDepartmentRelationship where [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "' AND [DepartmentCode] = " + deptCode + "", CommandType.Text));
        }

        /// <summary>
        /// check if username already exist on tbldepartmentrelationship
        /// </summary>
        /// <param name="username">will be used to the where clause</param>
        /// <returns>integer</returns>
        /// @ver 1.0 @author rherejias 1/18/2017
        public int isUsernameExist(string username, string code)
        {
            string select = "";
            if (code == ConfigurationManager.AppSettings["secondaryITApprover"])
                select = "select count(*) from tblApprovalDepartmentRelationship where [UserCode] = " + username + " AND ([ApprovalTypeCode] = " + code + " OR [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["itApprover"] + "')";
            else if (code == ConfigurationManager.AppSettings["secondaryPurchasingApprover"])
                select = "select count(*) from tblApprovalDepartmentRelationship where [UserCode] = " + username + " AND ([ApprovalTypeCode] = " + code + " OR [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["purchasingApprover"] + "')";
            else if (code == ConfigurationManager.AppSettings["secondaryAccountingApprover"])
                select = "select count(*) from tblApprovalDepartmentRelationship where [UserCode] = " + username + " AND ([ApprovalTypeCode] = " + code + " OR [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["accountingApprover"] + "')";
            else if (code == ConfigurationManager.AppSettings["itApprover"])
                select = "select count(*) from tblApprovalDepartmentRelationship where [UserCode] = " + username + " AND [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["secondaryITApprover"] + "'";
            else if (code == ConfigurationManager.AppSettings["purchasingApprover"])
                select = "select count(*) from tblApprovalDepartmentRelationship where [UserCode] = " + username + " AND [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["secondaryPurchasingApprover"] + "'";
            else if (code == ConfigurationManager.AppSettings["accountingApprover"])
                select = "select count(*) from tblApprovalDepartmentRelationship where [UserCode] = " + username + " AND [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["secondaryAccountingApprover"] + "'";
            else
                select = "select count(*) from tblApprovalDepartmentRelationship where [UserCode] = " + username + " AND [ApprovalTypeCode] = " + code + "";

            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(select, CommandType.Text));
        }

        /// <summary>
        /// check if approver with the same department already exist
        /// </summary>
        /// <param name="username"> will be used in the where clause</param>
        /// <param name="dept"> will be used in the where clause</param>
        /// <returns>true or false, true if count is  = 1 false if 0</returns>
        /// @ver 1.0 @author rherejias 3/28/2017
        public bool IsApproverExistAndDept(string username, string dept, string approvalType)
        {
            bool value = false;
            string select = "";
            if (approvalType == ConfigurationManager.AppSettings["secondaryITApprover"])
                select = "select count(*) from vwApprovers where [UserCode] = '" + username + "' AND [DepartmentCode] ='" + dept + "' AND ([ApprovalTypeCode] = '" + approvalType + "' OR [ApprovalTypeCode] = " + ConfigurationManager.AppSettings["itApprover"] + ")";
            else if (approvalType == ConfigurationManager.AppSettings["secondaryPurchasingApprover"])
                select = "select count(*) from vwApprovers where [UserCode] = '" + username + "' AND [DepartmentCode] ='" + dept + "' AND ([ApprovalTypeCode] = '" + approvalType + "' OR [ApprovalTypeCode] = " + ConfigurationManager.AppSettings["purchasingApprover"] + ")";
            else if (approvalType == ConfigurationManager.AppSettings["secondaryAccountingApprover"])
                select = "select count(*) from vwApprovers where [UserCode] = '" + username + "' AND [DepartmentCode] ='" + dept + "' AND ([ApprovalTypeCode] = '" + approvalType + "' OR [ApprovalTypeCode] = " + ConfigurationManager.AppSettings["accountingApprover"] + ")";
            else if (approvalType == ConfigurationManager.AppSettings["itApprover"])
                select = "select count(*) from vwApprovers where [UserCode] = '" + username + "' AND [DepartmentCode] ='" + dept + "' AND ([ApprovalTypeCode] = '" + approvalType + "' OR [ApprovalTypeCode] = " + ConfigurationManager.AppSettings["secondaryITApprover"] + ")";
            else if (approvalType == ConfigurationManager.AppSettings["purchasingApprover"])
                select = "select count(*) from vwApprovers where [UserCode] = '" + username + "' AND [DepartmentCode] ='" + dept + "' AND ([ApprovalTypeCode] = '" + approvalType + "' OR [ApprovalTypeCode] = " + ConfigurationManager.AppSettings["secondaryPurchasingApprover"] + ")";
            else if (approvalType == ConfigurationManager.AppSettings["accountingApprover"])
                select = "select count(*) from vwApprovers where [UserCode] = '" + username + "' AND [DepartmentCode] ='" + dept + "' AND ([ApprovalTypeCode] = '" + approvalType + "' OR [ApprovalTypeCode] = " + ConfigurationManager.AppSettings["secondaryAccountingApprover"] + ")";
            else
                select = "select count(*) from vwApprovers where [UserCode] = '" + username + "' AND [DepartmentCode] ='" + dept + "' AND [ApprovalTypeCode] = '" + approvalType + "'";

            int count = Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(select, CommandType.Text));
            if (count == 0)
            {
                value = true;
            }
            return value;
        }

        /// <summary>
        /// check if username already exist on tbldepartmentrelationship for edit functio
        /// </summary>
        /// <param name="userCode">will be used to the where clause</param>
        /// <returns>integer long bec. Code requires long integer</returns>
        /// @ver 1.0 @author rherejias 1/18/2017
        public long isUsernameExistEdit(string userCode, string approverCode)
        {
            string select = "";
            if (approverCode == ConfigurationManager.AppSettings["secondaryITApprover"])
                select = "select [Code] from tblApprovalDepartmentRelationship where [UserCode] = " + userCode + " AND ([ApprovalTypeCode] = " + approverCode + " OR [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["itApprover"] + "')";
            else if (approverCode == ConfigurationManager.AppSettings["secondaryPurchasingApprover"])
                select = "select [Code] from tblApprovalDepartmentRelationship where [UserCode] = " + userCode + " AND ([ApprovalTypeCode] = " + approverCode + " OR [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["purchasingApprover"] + "')";
            else if (approverCode == ConfigurationManager.AppSettings["secondaryAccountingApprover"])
                select = "select [Code] from tblApprovalDepartmentRelationship where [UserCode] = " + userCode + " AND ([ApprovalTypeCode] = " + approverCode + " OR [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["accountingApprover"] + "')";
            else
                select = "select [Code] from tblApprovalDepartmentRelationship where [UserCode] = " + userCode + " AND [ApprovalTypeCode] = " + approverCode + "";

            return Convert.ToInt64(Library.ConnectionString.returnCon.executeScalarQuery(select, CommandType.Text));
        }

        /// <summary>
        /// department name getter
        /// </summary>
        /// <param name="deptCode">will be used to the where clause</param>
        /// <returns>string</returns>
        /// @ver 1.0 @author rherejias 1/18/2017
        public string getDepartmentName(string deptCode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("select [Name] from tblDepartments where [Code] = " + deptCode + "", CommandType.Text).ToString();
        }

        /// <summary>
        /// user department name getter
        /// </summary>
        /// <param name="deptCode">will be used to the where clause</param>
        /// <returns>string</returns>
        /// @ver 1.0 @author rherejias 2/9/2017
        public string getUserDepartmentName(string deptName)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("select [Department] from tblUsers where [Code] = " + deptName + "", CommandType.Text).ToString();
        }

        /// <summary>
        /// department code getter
        /// </summary>
        /// <param name="deptCode">will be used to the where clause</param>
        /// <returns>string</returns>
        /// @ver 1.0 @author rherejias 2/9/2017
        public string getDepartmentCode(string deptName)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("select [Code] from tblDepartments where [Name] = '" + deptName + "'", CommandType.Text).ToString();
        }


        /// <summary>
        /// guard department code getter
        /// </summary>
        /// <param name="deptCode">will be used to the where clause</param>
        /// <returns>string</returns>
        /// @ver 1.0 @author rherejias 2/9/2017
        public string getGuardDept(string userCode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("select [Department] from tblUsers where [Code] = '" + userCode + "'", CommandType.Text).ToString();
        }
    }
}