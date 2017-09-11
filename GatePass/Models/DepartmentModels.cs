using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GatePass.ViewModels;
using System.Data.SqlClient;
using System.Data;

namespace GatePass.Models
{
    public class DepartmentModels
    {

        #region UpSertDepartment
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 5, 2016 11:10AM
         * @description     :   update and/or insert to tbldepartments
         */
        public bool UpSertDepartment(DepartmentObject DeptObj)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Code", DeptObj.Code),
                new SqlParameter("@Name", DeptObj.Name),
                new SqlParameter("@Description", DeptObj.Description),
                new SqlParameter("@Source", DeptObj.Source),
                new SqlParameter("@IsActive", DeptObj.IsActive),
                new SqlParameter("@AddedBy", DeptObj.AddedBy),
                new SqlParameter("@DateAdded", DeptObj.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpSertDepartments", params_, CommandType.StoredProcedure);
        }
        #endregion

        //rherejias
        public Dictionary<string, string> GetDepartment_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("Name", "name_string");
            cols.Add("Description", "department_string");
            cols.Add("IsActive", "active_bool");
            cols.Add("Source", "source_string");
            cols.Add("AddedBy", "added_by_string");
            cols.Add("DateAdded", "date_added_datetime");

            return cols;
        }

        public DataTable GetDepartment(int offset, int next, string where, string sorting, string searchStr)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetDepartment_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "Name ASC";
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
            sql += " FROM tblDepartments " + where + " ORDER BY " + sorting + " " + pagination + ";";

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
        public int GetDepartment_count(string where, string searchStr)
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
            string sql = "SELECT COUNT(*) FROM tblDepartments " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
    }
}