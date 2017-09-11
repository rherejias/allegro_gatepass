using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class AuditTrailModels
    {
        //set names for columns
        public Dictionary<string, string> GetAuditTrail_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("DateAdded", "date_added_datetime");
            cols.Add("Name", "name_string");
            cols.Add("Module", "module_string");
            cols.Add("Operation", "operation_string");
            cols.Add("IP", "ip_address_string");
            cols.Add("MAC", "mac_address_string");
            cols.Add("Id", "id_number");
            cols.Add("ProjectCode", "project_code_string");
            cols.Add("Object", "object_string");
            cols.Add("ObjectId", "object_id_number");
            cols.Add("ObjectCode", "object_code_string");

            return cols;
        }

        public DataTable GetAuditTrail(int offset, int next, string where, string sorting, string searchStr)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAuditTrail_cols();

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
            sql += " FROM vwAuditTrail " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   Ren <rherejias@allegromicro.com>
         * @date        :   12/78/2016 1:14 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetAuditTrail_count(string where, string searchStr)
        {
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwAuditTrail " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        /// <summary>
        /// * Model for all audittrail insert
        /// </summary>
        /// <param name="AuditTrail">parameter that will passed to the stored procedure</param>
        /// <returns>stored procedure for inserting data to tblAudittrail</returns>
        /// AddAuditTrail @ver 1.0 @author rherejias 1/13/2017
        public bool AddAuditTrail(AuditTrailObject AuditTrail)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@projectCode", AuditTrail.ProjectCode),
                new SqlParameter("@module", AuditTrail.Module),
                new SqlParameter("@operation", AuditTrail.Operation),
                new SqlParameter("@object", AuditTrail.Object),
                new SqlParameter("@objectId", AuditTrail.ObjectId),
                new SqlParameter("@objectCode", AuditTrail.ObjectCode),
                new SqlParameter("@userCode", AuditTrail.UserCode),
                new SqlParameter("@IP", AuditTrail.IP),
                new SqlParameter("@MAC", AuditTrail.MAC),
                new SqlParameter("@dateAdded", AuditTrail.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddAuditTrail", params_, CommandType.StoredProcedure);

        }
    }
}