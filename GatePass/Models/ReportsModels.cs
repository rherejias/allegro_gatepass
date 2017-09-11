using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace GatePass.Models
{
    public class ReportsModels
    {
        #region all reports
        //modification: author rherejias 6/5/2017 *added additional column "DateOut"
        public Dictionary<string, string> GetReports_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Code", "Gatepass_ID_string");
            cols.Add("ImpexRefNbr", "impex_string");
            cols.Add("ReturnDate", "returndate_date");
            cols.Add("Purpose", "purpose_string");
            cols.Add("Status", "status_string");
            cols.Add("Requestor", "requestor_string");
            cols.Add("Department", "department_string");
            cols.Add("DateAdded", "dateadded_date");
            cols.Add("DateOut", "dateout_string");

            return cols;
        }

        public DataTable GetReports(int offset, int next, string where, string sorting)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetReports_cols();
            string draft = "";

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
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

            if (where == "")
            {
                draft = " WHERE [Status] != 'Drafted'";
            }
            else
            {
                draft = " AND [Status] != 'Drafted'";
            }

            //remove the trailing " ," from the right
            sql = sql.Substring(0, sql.Length - 2);

            //set the table name and other params
            sql += " FROM vwTransactionHeader " + where + draft + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   2/28/2016 9:36 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetReports_count(string where)
        {
            string draft = "";
            if (where == "")
            {
                draft = " WHERE [Status] != 'Drafted'";
            }
            else
            {
                draft = " AND [Status] != 'Drafted'";
            }

            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwTransactionHeader " + where + draft;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion 

        #region department reports
        public Dictionary<string, string> GetDeptReports_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Code", "Gatepass_ID_string");
            cols.Add("ImpexRefNbr", "impex_string");
            cols.Add("ReturnDate", "returndate_date");
            cols.Add("Purpose", "purpose_string");
            cols.Add("Status", "status_string");
            cols.Add("Requestor", "requestor_string");
            cols.Add("Department", "department_string");
            cols.Add("DateAdded", "dateadded_date");
            cols.Add("Approver", "approver_string");

            return cols;
        }

        public DataTable GetDeptReports(int offset, int next, string where, string sorting, string view)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetDeptReports_cols();
            string draft = "";

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
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
            sql += " FROM " + view + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   3/16/2017 9:36 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetDeptReports_count(string where, string view)
        {
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM " + view + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion 

        #region IT reports
        public Dictionary<string, string> GetITReports_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Code", "Gatepass_ID_string");
            cols.Add("ImpexRefNbr", "impex_string");
            cols.Add("ReturnDate", "returndate_date");
            cols.Add("Purpose", "purpose_string");
            cols.Add("Status", "status_string");
            cols.Add("Requestor", "requestor_string");
            cols.Add("Department", "department_string");
            cols.Add("DateAdded", "dateadded_date");
            cols.Add("Approver", "approver_string");

            return cols;
        }

        public DataTable GetITReports(int offset, int next, string where, string sorting, string isApproved, string view)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetITReports_cols();
            string draft = "";

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
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

            if (where == "")
            {
                draft = " WHERE [IsApproved] =" + isApproved;
            }
            else
            {
                draft = " AND [IsApproved] =" + isApproved;
            }

            //remove the trailing " ," from the right
            sql = sql.Substring(0, sql.Length - 2);

            //set the table name and other params
            sql += " FROM " + view + where + draft + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   3/16/2017 9:36 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetITReports_count(string where, string isApproved, string view)
        {
            string draft = "";
            if (where == "")
            {
                draft = " WHERE [IsApproved] =" + isApproved;
            }
            else
            {
                draft = " AND [IsApproved] =" + isApproved;
            }
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM " + view + where + draft;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion 

        #region Purch reports
        public Dictionary<string, string> GetPurchReports_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Code", "Gatepass_ID_string");
            cols.Add("ImpexRefNbr", "impex_string");
            cols.Add("ReturnDate", "returndate_date");
            cols.Add("Purpose", "purpose_string");
            cols.Add("Status", "status_string");
            cols.Add("Requestor", "requestor_string");
            cols.Add("Department", "department_string");
            cols.Add("DateAdded", "dateadded_date");
            cols.Add("Approver", "approver_string");

            return cols;
        }

        public DataTable GetPurchReports(int offset, int next, string where, string sorting, string IsApproved, string view)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetPurchReports_cols();
            string draft = "";

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
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

            if (where == "")
            {
                draft = " WHERE [IsApproved] =" + IsApproved;
            }
            else
            {
                draft = " AND [IsApproved] =" + IsApproved;
            }


            //remove the trailing " ," from the right
            sql = sql.Substring(0, sql.Length - 2);

            //set the table name and other params
            sql += " FROM " + view + where + draft + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   3/16/2017 9:36 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetPurchReports_count(string where, string IsApproved, string view)
        {
            string draft = "";
            if (where == "")
            {
                draft = " WHERE [IsApproved] =" + IsApproved;
            }
            else
            {
                draft = " AND [IsApproved] =" + IsApproved;
            }
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM " + view + where + draft;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion 

        #region Acct reports
        public Dictionary<string, string> GetAcctReports_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Code", "Gatepass_ID_string");
            cols.Add("ImpexRefNbr", "impex_string");
            cols.Add("ReturnDate", "returndate_date");
            cols.Add("Purpose", "purpose_string");
            cols.Add("Status", "status_string");
            cols.Add("Requestor", "requestor_string");
            cols.Add("Department", "department_string");
            cols.Add("DateAdded", "dateadded_date");
            cols.Add("Approver", "approver_string");

            return cols;
        }

        public DataTable GetAcctReports(int offset, int next, string where, string sorting, string IsApproved, string view)
        {
            string draft = "";
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAcctReports_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
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

            if (where == "")
            {
                draft = " WHERE [IsApproved] =" + IsApproved;
            }
            else
            {
                draft = " AND [IsApproved] =" + IsApproved;
            }

            //remove the trailing " ," from the right
            sql = sql.Substring(0, sql.Length - 2);

            //set the table name and other params
            sql += " FROM " + view + where + draft + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   3/16/2017 9:36 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetAcctReports_count(string where, string IsApproved, string view)
        {
            string draft = "";

            if (where == "")
            {
                draft = " WHERE [IsApproved] =" + IsApproved;
            }
            else
            {
                draft = " AND [IsApproved] =" + IsApproved;
            }
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM " + view + where + draft;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion 

        #region Override reports
        public Dictionary<string, string> GetOverrideReports_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "Gatepass_ID_string");
            cols.Add("ImpexRefNbr", "impex_string");
            cols.Add("ReturnDate", "returndate_date");
            cols.Add("Purpose", "purpose_string");
            cols.Add("Status", "status_string");
            cols.Add("Requestor", "requestor_string");
            cols.Add("Department", "department_string");
            cols.Add("DateAdded", "dateadded_date");
            cols.Add("Overriden", "overriden_approver_string");
            cols.Add("Approver", "override_by_string");

            return cols;
        }

        public DataTable GetOverrideReports(int offset, int next, string where, string sorting, string IsApproved, string view)
        {
            string draft = "";
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetOverrideReports_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
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
            sql += " FROM " + view + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   3/16/2017 9:36 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetOverrideReports_count(string where, string IsApproved, string view)
        {
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM " + view + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion

        #region Override reject reports
        public Dictionary<string, string> GetOverrideRejectReports_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "Gatepass_ID_string");
            cols.Add("ImpexRefNbr", "impex_string");
            cols.Add("ReturnDate", "returndate_date");
            cols.Add("Purpose", "purpose_string");
            cols.Add("Status", "status_string");
            cols.Add("Requestor", "requestor_string");
            cols.Add("Department", "department_string");
            cols.Add("DateAdded", "dateadded_date");

            return cols;
        }

        public DataTable GetOverrideRejectReports(int offset, int next, string where, string sorting, string IsApproved, string view)
        {
            string draft = "";
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetOverrideRejectReports_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
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
            sql += " FROM " + view + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   3/16/2017 9:36 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetOverrideRejectReports_count(string where, string IsApproved, string view)
        {
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM " + view + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion 

        /// <summary>
        /// for audittrail stored procedure
        /// </summary>
        /// <param name="operation">operation of the export</param>
        /// <param name="table">table that will be affected</param>
        /// <returns>true or false</returns>
        /// @ver 1.0 @author rherejias 3/6/2017
        public bool export(string operation, string table)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@ProjectCode", "GAT"),
                new SqlParameter("@Module", "REPORTS"),
                new SqlParameter("@Operation", operation),
                new SqlParameter("@Object", table),
                new SqlParameter("@ObjectId", " "),
                new SqlParameter("@ObjectCode", " "),
                new SqlParameter("@UserCode", Convert.ToInt32(HttpContext.Current.Session["userId_local"])),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateAdded", DateTime.Now),
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddAuditTrail", params_, CommandType.StoredProcedure);
        }

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   MARCH 10, 2017 1:30 PM
         * @description :   get transaction headers
         */
        public DataTable GetHeaderTransactions(string where, string viewName, string select)
        {
            //build the sql statement
            string sql = select + viewName + where;

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   MARCH 10, 2017 1:30 PM
         * @description :   get transaction details
         */
        public DataTable GetTransactionDetails(string loc, string where)
        {
            string location = "vwTransactionDetails";
            if (loc == "t")
            {
                location = "vwTransactionDetails_temp";
            }
            //build the sql statement
            string sql = "SELECT SerialNbr,TagNbr,PONbr,Remarks,ReturnSlipStatus,ItemName,CategoryName,ItemTypeName FROM " + location + " " + where;

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
    }
}