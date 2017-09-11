using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class ITPurchasingModel
    {
        //rherejias
        public Dictionary<string, string> GetITPurchasingApprovers_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("ApprovalTypeCode", "approval_type_code_string");
            cols.Add("ApprovalType", "approval_type_string");
            cols.Add("DepartmentCode", "department_code_string");
            cols.Add("Department", "department_string");
            cols.Add("UserCode", "user_code_string");
            cols.Add("[User]", "user_string");
            cols.Add("Email", "email_string");
            cols.Add("IsActive", "active_bool");

            return cols;
        }

        public DataTable GetITPurchasingApprovers(int offset, int next, string where, string sorting, string searchStr, string ApproverType)
        {
            string Primary = "";
            string alt = "";
            if (ApproverType == "IT")
            {
                Primary = ConfigurationManager.AppSettings["itApprover"];
                alt = ConfigurationManager.AppSettings["secondaryITApprover"];
            }
            else if (ApproverType == "Purchasing")
            {
                Primary = ConfigurationManager.AppSettings["purchasingApprover"];
                alt = ConfigurationManager.AppSettings["secondaryPurchasingApprover"];
            }

            //call for the method in getting the columns
            Dictionary<string, string> cols = GetITPurchasingApprovers_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "Department ASC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND (ApprovalTypeCode = '" + Primary + "' OR ApprovalTypeCode = '" + alt + "')";
            }
            else
            {
                where += " AND IsActive = 1 AND (ApprovalTypeCode = '" + Primary + "' OR ApprovalTypeCode = '" + alt + "')";
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
         * @date        :   2/7/2017
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetITPurchasingApprovers_count(string where, string searchStr, string ApproverType)
        {
            string Primary = "";
            string alt = "";
            if (ApproverType == "IT")
            {
                Primary = ConfigurationManager.AppSettings["itApprover"];
                alt = ConfigurationManager.AppSettings["secondaryITApprover"];
            }
            else if (ApproverType == "Purchasing")
            {
                Primary = ConfigurationManager.AppSettings["purchasingApprover"];
                alt = ConfigurationManager.AppSettings["secondaryPurchasingApprover"];
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND (ApprovalTypeCode = '" + Primary + "' OR ApprovalTypeCode = '" + alt + "')";
            }
            else
            {
                where += " AND IsActive = 1 AND (ApprovalTypeCode = '" + Primary + "' OR ApprovalTypeCode = '" + alt + "')";
            }



            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwApprovers " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        /// <summary>
        /// for assigning approver on "IT" and "Purchasing"
        /// </summary> 
        /// <param name="approver">object that hold data from controller</param>
        /// <returns>bollean true or false</returns>
        /// @ver 1.0 @author rherejias 2/7/2017
        public bool AssignApprover(AppoverObject approver)
        {
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

            return Library.ConnectionString.returnCon.executeQuery("spAssignTempApprover", params_, CommandType.StoredProcedure);
        }
    }
}