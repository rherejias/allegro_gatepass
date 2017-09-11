using GatePass.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace GatePass.Models
{
    public class SupplierModels
    {
        public Dictionary<string, string> GetSuppliers_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("Name", "name_string");
            cols.Add("Email", "email_string");
            cols.Add("ContactNbr", "contact_number_string");
            cols.Add("UnitNbr", "unit_string");
            cols.Add("Street", "street_name_string");
            cols.Add("Municipality", "barangay_string");
            cols.Add("city", "city_string");
            cols.Add("Country", "country_string");
            cols.Add("ZipCode", "zip_string");
            cols.Add("IsActive", "active_bool");
            cols.Add("DateAdded", "date_added_datetime");
            cols.Add("ImpexRefNbr", "impex_number_string");

            return cols;
        }

        public DataTable GetSuppliers(int offset, int next, string where, string sorting, string searchStr)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetSuppliers_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " AND IsActive = 1";
            }

            if (!searchStr.IsNullOrWhiteSpace())
            {
                where += " AND (Name like '%" + searchStr + "%' " +
                     " OR Email like '%" + searchStr + "%' " +
                     " OR ContactNbr like '%" + searchStr + "%' " +
                     " OR UnitNbr like '%" + searchStr + "%' " +
                     " OR Street like '%" + searchStr + "%' " +
                     " OR Municipality like '%" + searchStr + "%' " +
                     " OR City like '%" + searchStr + "%' " +
                     " OR Country like '%" + searchStr + "%' " +
                     " OR ZipCode like '%" + searchStr + "%')";
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
            sql += " FROM tblSuppliers " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   10/28/2016 7:10 AM
         * @description :   this method counts the total records from the query, it will also be used in grid's serverside pagination
         * @parameters  :
         *                  (string) where - to be used in grid's server side filtering
         * @returns     :   datatable
         */
        public int GetSuppliers_count(string where, string searchStr)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " AND IsActive = 1";
            }

            if (!searchStr.IsNullOrWhiteSpace())
            {
                where += " AND (Name like '%" + searchStr + "%' " +
                     " OR Email like '%" + searchStr + "%' " +
                     " OR ContactNbr like '%" + searchStr + "%' " +
                     " OR UnitNbr like '%" + searchStr + "%' " +
                     " OR Street like '%" + searchStr + "%' " +
                     " OR Municipality like '%" + searchStr + "%' " +
                     " OR City like '%" + searchStr + "%' " +
                     " OR Country like '%" + searchStr + "%' " +
                     " OR ZipCode like '%" + searchStr + "%')";
            }

            //build the sql statement
            string sql = "SELECT COUNT(*) FROM tblSuppliers " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        public bool SaveNewSupplier(SupplierObject SupComp)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Name", SupComp.Name),
                new SqlParameter("@Email", SupComp.Email),
                new SqlParameter("@ContactNumber", SupComp.ContactNbr),
                new SqlParameter("@AddedBy", SupComp.AddedBy),
                new SqlParameter("@UnitNumber", SupComp.UnitNbr),
                new SqlParameter("@StreetName", SupComp.StreetName),
                new SqlParameter("@Municipality", SupComp.Municipality),
                new SqlParameter("@City", SupComp.City),
                new SqlParameter("@Country", SupComp.Country),
                new SqlParameter("@ZipCode", SupComp.Zip),
                new SqlParameter("@IsActive", SupComp.IsActive),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateCreated", SupComp.DateAdded),
                new SqlParameter("@ImpexRefNbr", SupComp.ImpexRefNbr),
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddSupplierCompany", params_, CommandType.StoredProcedure);
        }

        public bool UpdateSupplier(SupplierObject SupComp)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Id",SupComp.Id),
                new SqlParameter("@Code", SupComp.Code),
                new SqlParameter("@Name", SupComp.Name),
                new SqlParameter("@Email", SupComp.Email),
                new SqlParameter("@ContactNumber", SupComp.ContactNbr),
                new SqlParameter("@UnitNumber", SupComp.UnitNbr),
                new SqlParameter("@StreetName", SupComp.StreetName),
                new SqlParameter("@Municipality", SupComp.Municipality),
                new SqlParameter("@City", SupComp.City),
                new SqlParameter("@Country", SupComp.Country),
                new SqlParameter("@ZipCode", SupComp.Zip),
                new SqlParameter("@DateCreated", SupComp.DateAdded),
                new SqlParameter("@AddedBy", SupComp.AddedBy),
                new SqlParameter("@ImpexRefNbr", SupComp.ImpexRefNbr),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),

            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateSupplierCompany", params_, CommandType.StoredProcedure);
        }

        public Dictionary<string, string> GetSuppliersContactPersonCombo_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("Code", "code_string");
            cols.Add("Supplier", "supplier_string");
            cols.Add("IsActive", "is_active_bool");
            return cols;
        }

        public DataTable GetSuppliersContactPersonCombo(int offset, int next, string where, string sorting)
        {
            Dictionary<string, string> cols = GetSuppliersContactPersonCombo_cols();
            if (sorting == "")
            {
                sorting = "Supplier ASC";
            }
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " AND IsActive = 1";
            }
            string pagination = "";
            if (offset != 0 && next != 0)
            {
                if (next > 0)
                {
                    pagination = "OFFSET " + offset + " ROWS FETCH NEXT " + next + " ROWS ONLY";
                }
            }
            string sql = "SELECT ";
            foreach (var item in cols)
            {
                sql += item.Key + " AS " + item.Value + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += " FROM vwSuppliersContactPersonCombo " + where + " ORDER BY " + sorting + " " + pagination + ";";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        public int GetSuppliersContactPersonCombo_count(string where)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " IsActive = 1";
            }
            string sql = "SELECT COUNT(*) FROM vwSuppliersContactPersonCombo " + where;
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        // rherejias 11/21/16 for deactivate
        public bool DeactivateSupplier(SupplierObject supobj)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Id",supobj.Id),
                new SqlParameter("@Code", supobj.Code),
                new SqlParameter("@IsActive", supobj.IsActive),
                new SqlParameter("@table", "tblSuppliers"),
                new SqlParameter("@AddedBy", supobj.AddedBy),
                new SqlParameter("@DateAdded",supobj.DateAdded),
                new SqlParameter("@Module", "SUPPLIERS"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);
        }

        // rherejias 11/21/16 for search
        public DataTable Search(int offset, int next, string where, string sorting, string searchStr)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetSuppliers_cols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " AND IsActive = 1";
            }

            where += " AND (Name like '%" + searchStr + "%' " +
                     " OR Email like '%" + searchStr + "%' " +
                     " OR ContactNbr like '%" + searchStr + "%' " +
                     " OR UnitNbr like '%" + searchStr + "%' " +
                     " OR Street like '%" + searchStr + "%' " +
                     " OR Municipality like '%" + searchStr + "%' " +
                     " OR City like '%" + searchStr + "%' " +
                     " OR Country like '%" + searchStr + "%' " +
                     " OR ZipCode like '%" + searchStr + "%')";

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
            sql += " FROM tblSuppliers " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        public bool SaveTargets(SupplierObject targets)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@ProductGroup", targets.Name),
                new SqlParameter("@PackageGroupA", targets.Email),
                new SqlParameter("@PackageGroupB", targets.ContactNbr),
                new SqlParameter("@Target", targets.UnitNbr),
                new SqlParameter("@WorkWeek", targets.StreetName),
                new SqlParameter("@FiscalYear", targets.Municipality),
                new SqlParameter("@DateCreated", targets.City),
                new SqlParameter("@DateCreated", targets.Country),
                new SqlParameter("@DateCreated", targets.Zip),
                new SqlParameter("@DateCreated", targets.IsActive),
                new SqlParameter("@DateCreated", targets.DateAdded),
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddTargetsTest", params_, CommandType.StoredProcedure);
        }

        public bool export(string operation, string table)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@ProjectCode", "GAT"),
                new SqlParameter("@Module", "SUPPLIER"),
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

        public bool upload()
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@ProjectCode", "GAT"),
                new SqlParameter("@Module", "SUPPLIER"),
                new SqlParameter("@Operation", "Upload_Supplier"),
                new SqlParameter("@Object", "tblSuppliers"),
                new SqlParameter("@ObjectId", " "),
                new SqlParameter("@ObjectCode", " "),
                new SqlParameter("@UserCode", Convert.ToInt32(HttpContext.Current.Session["userId_local"])),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateAdded", DateTime.Now),
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddAuditTrail", params_, CommandType.StoredProcedure);
        }

        #region GetSuppliersForCombo
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 12, 2017
         * @description     :   this model will get the supplier name + code
         */
        public DataTable GetSuppliersForCombo()
        {
            string sql = "SELECT [Code],[Name] FROM [tblSuppliers] WHERE [IsActive]!=0 ORDER BY [Name];";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion
    }
}