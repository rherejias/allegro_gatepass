using GatePass.Helpers;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class CreateNewModel
    {
        readonly UploaderHelper upload = new UploaderHelper();

        /// <summary>
        /// check if username already exist on tbldepartmentrelationship
        /// </summary>
        /// <param name="username">will be used to the where clause</param>
        /// <returns>integer</returns>
        /// @ver 1.0 @author rherejias 1/18/2017
        public int IsItemAndSerialisExist(string itemCode, string serialNbr)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionDetails_temp where [ItemCode] = '" + itemCode + "' AND [SerialNbr] = '" + serialNbr + "'", CommandType.Text));
        }

        public int IsItemAndSerialisExistToDrafted_GP(string id, string itemCode, string serialNbr)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionDetails where [HeaderCode] = '" + id + "' AND [ItemCode] = '" + itemCode + "' AND [SerialNbr] = '" + serialNbr + "'", CommandType.Text));
        }



        public int IsItemAndSerialisExistUpdate(string itemCode, string serialNbr, int idtrans)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionDetails_temp where [Id] != " + idtrans + " AND [ItemCode] = '" + itemCode + "' AND [SerialNbr] = '" + serialNbr + "'", CommandType.Text));
        }


        #region GetItemforEdit
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 12, 2017
         * @description     :   this model will get the supplier name + code
         */
        public DataTable GetItemforEdit()
        {
            string sql = "SELECT [Code],[Name] FROM [vwItemsMasterlist] WHERE [IsActive]!=0 ORDER BY [Name];";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion



        /*
         * @author      :   AV <avillena@allegromicro.com>
         * @date        :   DEC. 15, 2016
         * @description :   addition of item details
         */
        #region UpdateItems
        public bool UpdateItems(Details item)
        {

            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@SessionId", value: item.SessionId),
                new SqlParameter(parameterName: "@IdEdit", value: item.Id),
              //  new SqlParameter(parameterName: "@HeaderCode", value: item.HeaderCode),
                new SqlParameter(parameterName: "@ItemCode", value: item.ItemCode),  //must not be empty
                new SqlParameter(parameterName: "@Quantity", value: item.Quantity),
                new SqlParameter(parameterName: "@UnitOfMeasureCode", value: item.UnitOfMeasureCode),
                new SqlParameter(parameterName: "@CategoryCode", value: item.CategoryCode),
                new SqlParameter(parameterName: "@ItemTypeCode", value: item.ItemTypeCode),
                new SqlParameter(parameterName: "@SerialNbr", value: item.SerialNbr),
                new SqlParameter(parameterName: "@TagNbr", value: item.TagNbr),
                new SqlParameter(parameterName: "@PONbr", value: item.PONbr),
                new SqlParameter(parameterName: "@IsActive", value: item.IsActive),
                new SqlParameter(parameterName: "@AddedBy", value: item.AddedBy),
                new SqlParameter(parameterName: "@DateAdded", value: item.DateAdded),
                new SqlParameter(parameterName: "@Remarks", value: item.Remarks),
                new SqlParameter(parameterName: "@Image", value: (item.Image == "") ? "" : UploaderHelper.FileName)

            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spUpdateAddedItem", params_: @params, type_: CommandType.StoredProcedure);
        }// End
        #endregion

        /// <summary>
        /// desc: to get department approver
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDepartmentApproversCols()
        {
            var cols = new Dictionary<string, string>();

            cols.Add(key: "Id", value: "id_number");
            cols.Add(key: "Code", value: "code_string");
            cols.Add(key: "ApprovalTypeCode", value: "approval_type_code_string");
            cols.Add(key: "ApprovalType", value: "approval_type_string");
            cols.Add(key: "ApproverName", value: "approver_name_string");
            cols.Add(key: "Department", value: "department_string");
            cols.Add(key: "UserCode", value: "user_code_string");
            cols.Add(key: "[User]", value: "user_string");
            cols.Add(key: "Email", value: "email_string");
            cols.Add(key: "IsActive", value: "active_bool");
            cols.Add(key: "DepartmentCode", value: "department_code_string");


            return cols;
        }

        public int GetDepartmentApproversCount(string where, string searchStr, string userdept)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND Department = 'Accounting (080)' AND ApprovalTypeCode = '" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "'";
            }
            else
            {
                where += " AND IsActive = 1 AND Department = 'Accounting (080)' AND ApprovalTypeCode = '" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "'";
            }



            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwApprovers " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        public DataTable GetDepartmentApprovers(int offset, int next, string where, string sorting, string searchStr, string userdept)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetDepartmentApproversCols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "Department ASC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND Department = '" + userdept + "' AND ApprovalTypeCode = '" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "'";
            }
            else
            {
                where += " AND IsActive = 1 AND Department = '" + userdept + "' AND ApprovalTypeCode = '" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "'";
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
            sql = sql.Substring(startIndex: 0, length: sql.Length - 2);

            //set the table name and other params
            sql += " FROM vwApprovers " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }


        /// <summary>
        /// desc: to get the dept approver
        /// by: avillena@allegromicro.com
        /// date: january 11 ,2017
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public string GEtDeptApproverEmail(string deptCode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("select [Email] from vwApprovers where [UserCode] = '" + deptCode + "';", CommandType.Text).ToString();
        }



        /// <summary>
        /// desc: count of non returnable item into temp details table
        /// by: avillena@allegromicro.com
        /// version : 1.0
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public int Hasnonreturnableitem(string sessionid)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from tblTransactionDetails_temp where [ItemTypeCode] = '20170200000' AND [SessionId] = '" + sessionid + "'", type_: CommandType.Text));
        }


    }
}