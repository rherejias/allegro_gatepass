using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
namespace GatePass.Models
{
    public class DepartmentMaintenanceModel
    {



        //desc : table column for department list | date : 06/06/2017 | author : avillena | version :1.0
        public Dictionary<string, string> GetGuardDataCols()
        {
            var cols = new Dictionary<string, string>();

            cols.Add(key: "Id", value: "id_number");
            cols.Add(key: "Code", value: "code_string");
            cols.Add(key: "Name", value: "department_name_string");
            cols.Add(key: "Description", value: "description_string");
            cols.Add(key: "IsActive", value: "isactive_bool");
            cols.Add(key: "Source", value: "source_string");
            cols.Add(key: "AddedBy", value: "addedby_string");
            cols.Add(key: "DateAdded", value: "dateadded_string");
            return cols;
        }

        //desc : get department record | date : 06/06/2017 | author : avillena | version :1.0
        public DataTable GetGuardData(int offset, int next, string where, string sorting, string isActive)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetGuardDataCols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "Name ASC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = '" + isActive + "'";
            }
            else
            {
                where += "AND IsActive = '" + isActive + "'";
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
            sql += " FROM tblDepartments " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   avillena@allegromicro.com
         * @date        :   06/06/2017
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetGuardDataCount(string where, string isActive)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = '" + isActive + "'";
            }
            else
            {
                where += "AND IsActive = '" + isActive + "'";
            }



            //build the sql statement
            string sql = "SELECT COUNT(*) FROM tblDepartments " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        //desc : adding of new department record | date : 06/06/2017 | author : avillena | version :1.0
        public bool Add(GuardObject department)
        {
            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@departmentname", value: department.deptname),
                new SqlParameter(parameterName: "@description", value: department.description),
                new SqlParameter(parameterName: "@IsActive", value: true),
                new SqlParameter(parameterName: "@Source", value: "Manual"),
                new SqlParameter(parameterName: "@IP", value: Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter(parameterName: "@MAC", value: Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter(parameterName: "@DateAdded", value: DateTime.Now),
                new SqlParameter(parameterName: "@AddedBy", value: Convert.ToInt32(HttpContext.Current.Session["userId_local"]))
            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spAddNewDepartment", params_: @params, type_: CommandType.StoredProcedure);
        }
        //desc : update department record | date : 06/06/2017 | author : avillena | version :1.0
        public bool Edit(GuardObject department)
        {
            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@Id", value: department.Id),
                new SqlParameter(parameterName: "@code", value: department.code),
                new SqlParameter(parameterName: "@departmentname", value: department.deptname),
                new SqlParameter(parameterName: "@description", value: department.description),
                new SqlParameter(parameterName: "@IsActive", value: true),
                new SqlParameter(parameterName: "@Source", value: "Manual"),
                new SqlParameter(parameterName: "@IP", value: Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter(parameterName: "@MAC", value: Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter(parameterName: "@DateAdded", value: DateTime.Now),
                new SqlParameter(parameterName: "@AddedBy", value: Convert.ToInt32(HttpContext.Current.Session["userId_local"]))
            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spUpdateDepartmentList", params_: @params, type_: CommandType.StoredProcedure);
        }
        //desc : deactive department record | date : 06/06/2017 | author : avillena | version :1.0
        public bool Deactivate(GuardObject department)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter(parameterName: "@id", value: department.Id),
                new SqlParameter(parameterName: "@Code", value: department.code),
                new SqlParameter(parameterName: "@IsActive", value: department.IsActive),
                new SqlParameter(parameterName: "@table", value: "tblDepartments"),
                new SqlParameter(parameterName: "@AddedBy", value: department.AddedBy),
                new SqlParameter(parameterName: "@DateAdded", value: department.DateAdded),
                new SqlParameter(parameterName: "@Module", value: "DEPARTMENT"),
                new SqlParameter(parameterName: "@IP", value: Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter(parameterName: "@MAC", value: Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spDeactivateDepartmentList", params_: params_, type_: CommandType.StoredProcedure);
        }
        //desc : inactive department record | date : 06/06/2017 | author : avillena | version :1.0
        public bool Activate(GuardObject department)
        {
            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@id", value: department.Id),
                new SqlParameter(parameterName: "@Code", value: department.code),
                new SqlParameter(parameterName: "@IP", value: Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter(parameterName: "@MAC", value: Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter(parameterName: "@DateAdded", value: DateTime.Now),
                new SqlParameter(parameterName: "@AddedBy", value: Convert.ToInt32(HttpContext.Current.Session["userId_local"])),
                new SqlParameter(parameterName: "@table", value: "tblDepartments"),
                new SqlParameter(parameterName: "@Module", value: "Department")
            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spActivateDepartmentList", params_: @params, type_: CommandType.StoredProcedure);
        }


        //desc : validation of department existing | date : 06/06/2017 | author : avillena | version :1.0
        public string DepartmentExist(string deptname)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("SELECT count(*) FROM [tblDepartments] where [Name] ='" + deptname + "'", CommandType.Text).ToString();
        }
        //desc : validation of department existing | date : 06/06/2017 | author : avillena | version :1.0
        public string GetDeptName(string code)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("SELECT [Name] FROM tblDepartments where [Code] ='" + code + "'", CommandType.Text).ToString();
        }

    }
}