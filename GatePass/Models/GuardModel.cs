using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class GuardModel


    {

        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   get and contract the column of return slip transaction into data grid
      */
        #region GetAllReturnSlipCols
        public Dictionary<string, string> GetAllReturnSlipCols()
        {
            var cols = new Dictionary<string, string>();

            cols.Add(key: "Id", value: "id_number");
            cols.Add(key: "Code", value: "gate_pass_id_string");
            cols.Add(key: "DateAdded", value: "date_added_datetime");
            cols.Add(key: "Purpose", value: "purpose_string");
            cols.Add(key: "Status", value: "status_string");
            cols.Add(key: "ReturnDate", value: "return_date_date");
            cols.Add(key: "ImpexRefNbr", value: "impex_ref_number_string");
            cols.Add(key: "TransType", value: "transaction_type_string");
            cols.Add(key: "ReturnSlipStatus", value: "return_slip_status_string");
            cols.Add(key: "UserCode", value: "usercode_string");
            cols.Add(key: "Attachment", value: "attachment_string");
            cols.Add(key: "ReturnSlipAttachment", value: "return_slip_attachment_string");
            cols.Add(key: "SupplierCode", value: "supplier_string");
            cols.Add(key: "SupplierName", value: "supplier_name_string");
            return cols;

        }// End
        #endregion



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all records for return slip transaction
        */
        #region GetAllReturnSlip
        public DataTable GetAllReturnSlip(int offset, int next, string where, string sorting, int isSearch)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAllReturnSlipCols();

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
            sql = sql.Substring(startIndex: 0, length: sql.Length - 2);
            if (isSearch == 1)
            {
                //set the table name and other params
                sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND ReturnSlipStatus <> 'Returned' AND ReturnSlipStatus <> '' AND ReturnSlipStatus <> 'Non Returnable' ORDER BY " + sorting + " " + pagination + ";";
            }
            else
            {
                //set the table name and other params

                // sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND AddedBy = " + addedby + " AND ReturnSlipStatus = 'Not Returned' OR ReturnSlipStatus = 'Partially Returned' ORDER BY " + sorting + " " + pagination + ";";

                sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND ReturnSlipStatus <> 'Returned'  AND ReturnSlipStatus <> '' AND ReturnSlipStatus <> 'Non Returnable' ORDER BY " + sorting + " " + pagination + ";";

            }


            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
        #endregion



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all the transaction count and details of gatepass
        */
        #region GetAllTransCount
        public int GetAllTransCount(string where, int isSearch)
        {
            string sql;

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " AND IsActive = 1";
            }

            if (isSearch == 1)
            {
                //build the sql statement
                sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND ReturnSlipStatus <> 'Returned' AND ReturnSlipStatus <> '' AND ReturnSlipStatus <> 'Non Returnable'";
            }
            else
            {
                //build the sql statement
                // sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + "AND AddedBy = " + addedby + " AND ReturnSlipStatus = 'Not Returned' OR ReturnSlipStatus = 'Partially Returned'";
                sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND ReturnSlipStatus <> 'Returned' AND ReturnSlipStatus <> '' AND ReturnSlipStatus <> 'Non Returnable'";

            }
            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }// End
        #endregion





        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   construct the data grid column of all transaction
        */
        #region GetAllTransGuardcols
        public Dictionary<string, string> GetAllTransGuardcols()
        {
            var cols = new Dictionary<string, string>();

            cols.Add(key: "Id", value: "id_number");
            cols.Add(key: "Code", value: "gate_pass_id_string");
            cols.Add(key: "DateAdded", value: "date_added_datetime");
            cols.Add(key: "Purpose", value: "purpose_string");
            cols.Add(key: "Status", value: "status_string");
            cols.Add(key: "ReturnDate", value: "return_date_date");
            cols.Add(key: "ImpexRefNbr", value: "impex_ref_number_string");
            cols.Add(key: "TransType", value: "transaction_type_string");
            cols.Add(key: "Attachment", value: "attachment_string");
            cols.Add(key: "SupplierCode", value: "supplier_string");
            cols.Add(key: "SupplierName", value: "supplier_name_string");
            return cols;
        }// End
        #endregion




        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get and display all transaction in data grid
        */
        #region GetAllTransGuard
        public DataTable GetAllTransGuard(int offset, int next, string where, string sorting)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAllTransGuardcols();

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
            sql = sql.Substring(startIndex: 0, length: sql.Length - 2);

            //set the table name and other params
            sql += " FROM vwToBeApprovedByGuardApprover_finalCombo " + where + " AND Status='Submitted' ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all transaction count of gatepass
        */
        #region GetAllTransGuardCount
        public int GetAllTransGuardCount(string where)
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
            string sql = "SELECT COUNT(*) FROM vwToBeApprovedByGuardApprover_finalCombo " + where + " AND Status = 'Submitted'";

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));

        }// End
        #endregion



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   update the item quantity by the partial return
        */
        #region PartialReturn_Qty
        public bool PartialReturn_Qty(ItemReturnLogsObject item)
        {

            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@HeaderCode", value: item.HeaderCode),
                new SqlParameter(parameterName: "@ItemCode", value: item.ItemCode),  //must not be empty
                new SqlParameter(parameterName: "@PartialQuantity", value: item.Quantity),
                new SqlParameter(parameterName: "@IsActive", value: item.IsActive),
                new SqlParameter(parameterName: "@AddedBy", value: item.AddedBy),
                new SqlParameter(parameterName: "@DateAdded", value: item.DateAdded),
                new SqlParameter(parameterName: "@Remarks", value: item.Remarks),
                new SqlParameter(parameterName: "@GUID", value: item.GUID),

            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spAddPartialReturnQuantity", params_: @params, type_: CommandType.StoredProcedure);
        }// End
        #endregion



        #region GatePassRejected
        /*
         * @author          :   AC <avillena@allegromicro.com>
         * @date            :   JAN 23, 2017 8:20 AM
         * @description     :   update the gatepass status as approved and generate return slip
         */
        public DataTable GatePassRejected(string id, string comment)
        {
            string sql = "Update [tblTransactionHeaders] SET [Status] = 'Rejected', [Comment] = '" + comment + "' Where [Code] = '" + id + "';";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);


        }
        #endregion

        #region GatePassApproved
        /*
         * @author          :   AC <avillena@allegromicro.com>
         * @date            :   JAN 23, 2017 8:20 AM
         * @description     :   update the gatepass status as rejected
         */
        //public DataTable GatePassApproved(string id)
        //{
        //    string sql = "Update [tblTransactionHeaders] SET [ReturnSlipStatus] = 'Not Returned', [Status] = 'Approved' Where [Code] = '" + id + "';" + "Update [tblTransactionDetails] SET [ReturnSlipStatus] = 'Not Returned' Where [HeaderCode] = '" + id + "';";
        //    return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);


        //}

        public bool GatePassApproved(string headercode, string comment, int guardIdLocal, string guardRights, string guradCode, string guradname, string guardDateAdded, bool guardIsApproved, string returndate)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@HeaderCode", headercode),
                new SqlParameter("@Comment", comment),
                new SqlParameter("@GuardLocalId", guardIdLocal),
                new SqlParameter("@GuardRights", guardRights),
                 new SqlParameter("@GPReturnDate", returndate),

                new SqlParameter("@GuardCode",guradCode),
                new SqlParameter("@GuardName", guradname),
                new SqlParameter("@GuardDateApproved", guardDateAdded),
                new SqlParameter("@GuardIsApproved", Convert.ToInt32(guardIsApproved))

            };

            return Library.ConnectionString.returnCon.executeQuery("spApprovedGatePassByGuard", params_, CommandType.StoredProcedure);

        }


        #endregion

        /// <summary>
        /// desc: department owner addedby and email from headercode
        /// by: avillena@allegromicro.com
        /// date: january 11 ,2017
        /// </summary>
        /// <param name="deptCode"></param>
        ///// <returns></returns>
        public string GetOwnerofHeader(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [AddedBy] from tblTransactionHeaders where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }
        public string GetOwnerofHeaderEmail(string headercodeOwnerId)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Email] from tblUsers where [Id] = '" + headercodeOwnerId + "'", CommandType.Text).ToString();
        }
        public string GetAccountingEmail(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Email] from vwToBeApprovedByGuardApprover_finalCombo where [Code] = '" + headercode + "'", CommandType.Text).ToString();
        }

        public string GetDepartmentCodebaseOnHeader(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [DepartmentApproverCode] from tblTransactionHeaders where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }
        public string GetDepartmentEmailReturnSlip(string deptcodeapprover)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Email] from tblUsers where [Code] = '" + deptcodeapprover + "'", CommandType.Text).ToString();
        }

        public string GetAccountingEmailReturnSlip(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Email] from vwOverride where [TransHeaderCode] = '" + headercode + "' AND [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["accountingApprover"] + "'", CommandType.Text).ToString();
        }

        public int getHeaderCodeId(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery("Select [Id] from tblTransactionHeaders where [Code] = '" + headercode + "'", CommandType.Text));
        }

    }
}