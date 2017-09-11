using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GatePass.Models
{
    public class ApproverTypeModels
    {
        //rherejias
        public Dictionary<string, string> GetApproverType_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_string");
            cols.Add("Code", "code_string");
            cols.Add("Name", "name_string");
            cols.Add("Description", "description_string");
            return cols;
        }

        public DataTable GetApproverType(int offset, int next, string where, string sorting, string searchStr)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetApproverType_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "Code ASC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " AND IsActive = 1";
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
            sql += " FROM tblApprovalTypes " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   1/5/2017 9:38 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetApproverType_count(string where, string searchStr)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " AND IsActive = 1";
            }



            //build the sql statement
            string sql = "SELECT COUNT(*) FROM tblApprovalTypes " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        public bool addApproverType(ApproverTypeObject approverType)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("@Name", approverType.Name),
                new SqlParameter("@Description", approverType.Description),
                new SqlParameter("@IsActive", approverType.IsActive),
                new SqlParameter("@DateAdded", approverType.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddApproverType", params_, CommandType.StoredProcedure);
        }

        public bool EditApproverType(ApproverTypeObject approverType)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("@Code", approverType.Code),
                new SqlParameter("@Name", approverType.Name),
                new SqlParameter("@Description", approverType.Description)
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateApprovalType", params_, CommandType.StoredProcedure);
        }

        public bool InactiveApproverType(ApproverTypeObject approverType)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", approverType.Id),
                new SqlParameter("@Code", approverType.Code),
                new SqlParameter("@table", "tblApprovalTypes"),
                new SqlParameter("@IsActive", "false"),
                new SqlParameter("@DateAdded", DateTime.Now),
                new SqlParameter("@Module", "APPROVER_TYPE"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@AddedBy", Convert.ToInt32(HttpContext.Current.Session["userId_local"]))

        };

            return Library.ConnectionString.returnCon.executeQuery("[spDeactivateItem]", params_, CommandType.StoredProcedure);
        }
    }
}