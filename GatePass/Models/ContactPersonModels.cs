using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class ContactPersonModels
    {
        public Dictionary<string, string> GetSuppliers_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("SupplierKey", "supplier_key_string");
            cols.Add("Email", "Email_string");
            cols.Add("ContactNumber", "Contact_Number_string");
            cols.Add("Department", "Department_string");
            cols.Add("FirstName", "First_Name_string");
            cols.Add("MiddleName", "Middle_Name_string");
            cols.Add("LastName", "Last_Name_string");
            cols.Add("IsActive", "isactive_string");
            cols.Add("DateAdded", "dateadded_string");

            return cols;
        }

        public DataTable GetSuppliers(int offset, int next, string where, string sorting)
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
            sql += " FROM tblContactPerson " + where + " ORDER BY " + sorting + " " + pagination + ";";

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
        public int GetSuppliers_count(string where)
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
            string sql = "SELECT COUNT(*) FROM tblContactPerson " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }

        public bool SaveNewContactPerson(ContactPersonObject ConPerson)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@SupplierKey", ConPerson.SupplierKey),
                new SqlParameter("@FirstName", ConPerson.FirstName),
                new SqlParameter("@MiddleName", ConPerson.MiddleName),
                new SqlParameter("@LastName", ConPerson.LastName),
                new SqlParameter("@Email", ConPerson.Email),
                new SqlParameter("@ContactNumber", ConPerson.ContactNumber),
                new SqlParameter("@Department", ConPerson.Department),
                new SqlParameter("@IsActive", ConPerson.IsActive),
                new SqlParameter("@AddedBy", ConPerson.AddedBy),
                new SqlParameter("@DateAdded", ConPerson.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress()),
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddContactPerson", params_, CommandType.StoredProcedure);
        }

        public bool UpdateContactPerson(ContactPersonObject ConPerson)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Id",ConPerson.Id),
                new SqlParameter("@Code", ConPerson.Code),
                new SqlParameter("@FirstName", ConPerson.FirstName),
                new SqlParameter("@MiddleName", ConPerson.MiddleName),
                new SqlParameter("@LastName", ConPerson.LastName),
                new SqlParameter("@Email", ConPerson.Email),
                new SqlParameter("@ContactNumber", ConPerson.ContactNumber),
                new SqlParameter("@Department", ConPerson.Department),
                new SqlParameter("@AddedBy", ConPerson.AddedBy),
                new SqlParameter("@DateAdded", ConPerson.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress()),
                //new SqlParameter("@IsActive", ConPerson.IsActive)
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateContactPerson", params_, CommandType.StoredProcedure);
        }

        public bool DeactivateContact(ContactPersonObject ConPerson)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Id",ConPerson.Id),
                new SqlParameter("@Code", ConPerson.Code),
                new SqlParameter("@IsActive", ConPerson.IsActive),
                new SqlParameter("@table", "tblContactPerson"),
                new SqlParameter("@AddedBy", ConPerson.AddedBy),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateAdded", ConPerson.DateAdded),
                new SqlParameter("@Module", "CONTACT PERSON"),
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);
        }

        #region GetContactPersonsForCombo
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 12, 2017
         * @description     :   this model will get the contact person's name + code
         */
        public DataTable GetContactPersonsForCombo(string fk)
        {
            string sql = "SELECT CONCAT([FirstName],' ',[MiddleName],' ',[LastName]) AS [Name],[Code],[SupplierKey] AS [FK] FROM [tblContactPerson] WHERE [IsActive]!=0 AND [SupplierKey] LIKE '%" + fk + "%' ORDER BY [Name];";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion


        public DataTable GetContactPerson()
        {
            string sql = "SELECT CONCAT([FirstName],' ',[MiddleName],' ',[LastName]) AS [Name],[Code],[SupplierKey] FROM [tblContactPerson] WHERE [IsActive]!=0 ORDER BY [Name];";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }





        /// <summary>
        /// Desc: Validation of Contact Person if Exist to the Supplier
        /// Date : Jan, 27, 2017
        /// By: avillena
        /// </summary>
        /// <param name="supplierKey"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <returns></returns>
        public int IsContactPersonExistToSupplier(string supplierKey, string lastName, string firstName, string middleName)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblContactPerson where [SupplierKey] = '" + supplierKey + "' AND [LastName] = '" + lastName + "' AND [FirstName] = '" + firstName + "' AND [MiddleName] = '" + middleName + "'", CommandType.Text));
        }

    }
}