using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace GatePass.Models
{
    public class GuardMaintenanceModels
    {
        //rherejias
        public Dictionary<string, string> GetGuardData_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("Username", "username_string");
            cols.Add("Password", "password_string");
            cols.Add("GivenName", "givenname_string");
            cols.Add("LastName", "lastname_string");
            return cols;
        }

        public DataTable GetGuardData(int offset, int next, string where, string sorting, string IsActive)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetGuardData_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "Username ASC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = '" + IsActive + "' AND DEPARTMENT = 'Guard'";
            }
            else
            {
                where += "AND IsActive = '" + IsActive + "' AND DEPARTMENT = 'Guard'";
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
            sql += " FROM tblUsers " + where + " ORDER BY " + sorting + " " + pagination + ";";

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
        public int GetGuardData_count(string where, string IsActive)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = '" + IsActive + "' AND DEPARTMENT = 'Guard'";
            }
            else
            {
                where += "AND IsActive = '" + IsActive + "' AND DEPARTMENT = 'Guard'";
            }



            //build the sql statement
            string sql = "SELECT COUNT(*) FROM tblUsers " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }


        public bool Add(GuardObject guard)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@username", guard.username),
                new SqlParameter("@password", guard.password),
                new SqlParameter("@givenname", guard.givenName),
                new SqlParameter("@lastname", guard.lastName),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateAdded", DateTime.Now),
                new SqlParameter("@AddedBy",Convert.ToInt32(HttpContext.Current.Session["userId_local"]))
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddGuardProfile", params_, CommandType.StoredProcedure);
        }

        public bool Edit(GuardObject guard)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", guard.Id),
                new SqlParameter("@code", guard.code),
                new SqlParameter("@username", guard.username),
                new SqlParameter("@password", guard.password),
                new SqlParameter("@givenname", guard.givenName),
                new SqlParameter("@lastname", guard.lastName),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateAdded", DateTime.Now),
                new SqlParameter("@AddedBy",Convert.ToInt32(HttpContext.Current.Session["userId_local"]))
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateGuardProfile", params_, CommandType.StoredProcedure);
        }
        public bool Deactivate(GuardObject guard)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", guard.Id),
                new SqlParameter("@Code", guard.code),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateAdded", DateTime.Now),
                new SqlParameter("@AddedBy",Convert.ToInt32(HttpContext.Current.Session["userId_local"])),
                new SqlParameter("@IsActive", false),
                new SqlParameter("@table", "tblUsers"),
                new SqlParameter("@Module", "GUARD")
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);
        }

        public bool Activate(GuardObject guard)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", guard.Id),
                new SqlParameter("@Code", guard.code),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateAdded", DateTime.Now),
                new SqlParameter("@AddedBy",Convert.ToInt32(HttpContext.Current.Session["userId_local"])),
                new SqlParameter("@table", "tblUsers"),
                new SqlParameter("@Module", "GUARD")
            };

            return Library.ConnectionString.returnCon.executeQuery("spActivateItem", params_, CommandType.StoredProcedure);
        }


        public string usernameIsExist(string code)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("SELECT count(*) FROM tblUsers where [Username] ='" + code + "'", CommandType.Text).ToString();
        }

        public string getGivenName(string code)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("SELECT [Username] FROM tblUsers where [Code] ='" + code + "'", CommandType.Text).ToString();
        }
    }
}