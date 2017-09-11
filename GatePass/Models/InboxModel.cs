using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GatePass.ViewModels;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;
using GatePass.Helpers;
namespace GatePass.Models
{
    public class InboxModel
    {
        readonly UploaderHelper upload = new UploaderHelper();


        /*
 * @author      :   AV <avillena@allegromicro.com>
 * @date        :   DEC. 15, 2016
 * @description :   contract the data grid column of all transaction
 */
        #region GetAllTransCols
        public Dictionary<string, string> GetAllTransCols()
        {
            var cols = new Dictionary<string, string>();

            cols.Add(key: "Id", value: "id_number");
            cols.Add(key: "Code", value: "gate_pass_id_string");
            cols.Add(key: "DateAdded", value: "date_added_datetime");
            cols.Add(key: "Purpose", value: "purpose_string");
            cols.Add(key: "Status", value: "status_string");
            cols.Add(key: "ReturnDate", value: "return_date_datetime");
            cols.Add(key: "ImpexRefNbr", value: "impex_ref_number_string");
            cols.Add(key: "TransType", value: "transaction_type_string");
            cols.Add(key: "Attachment", value: "attachment_string");
            cols.Add(key: "SupplierCode", value: "supplier_string");
            cols.Add(key: "ContactPersonCode", value: "contact_person_string");
            cols.Add(key: "Comment", value: "comment_string");

            return cols;
        }// End
        #endregion




        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get and display all transaction in data grid
        */
        #region GetAllTrans
        public DataTable GetAllTrans(int offset, int next, string where, string sorting)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAllTransCols();

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
            sql += " FROM tblTransactionHeaders " + where + "AND Status = 'With Correction' ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all transaction count of gatepass
        */
        #region GetAllTransCount
        public int GetAllTransCount(string where)
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
            string sql = "SELECT COUNT(*) FROM tblTransactionHeaders " + where + " AND Status = 'With Correction'";

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));

        }// End
        #endregion





        /*
 * @modified by     :   AC <aabasolo@allegromicro.com>
 * @date            :   NOV. 24, 2016 3:33PM
 * @modification    :   added session id, item name, category name, item type, dateadded in preparation of the view
 */
        #region GetItemDetailsInboxCols
        public Dictionary<string, string> GetItemDetailsInboxCols()
        {
            var cols = new Dictionary<string, string>();

            cols.Add(key: "SessionId", value: "session_id_string");
            cols.Add(key: "Id", value: "id_number");
            cols.Add(key: "Code", value: "code_string");
            cols.Add(key: "HeaderCode", value: "header_code_string");
            cols.Add(key: "ItemCode", value: "item_code_string");
            cols.Add(key: "Quantity", value: "quantity_number");
            cols.Add(key: "UnitOfMeasureCode", value: "unit_of_measure_code_string");
            cols.Add(key: "ItemName", value: "item_name_string");

            cols.Add(key: "TagNbr", value: "tag_number_string");
            cols.Add(key: "PONbr", value: "p_o_number_string");
            cols.Add(key: "IsActive", value: "active_bool");
            cols.Add(key: "SerialNbr", value: "serial_number_string");
            cols.Add(key: "UOMName", value: "unit_of_measure_string");

            cols.Add(key: "CategoryName", value: "category_string");
            cols.Add(key: "ItemType", value: "type_string");

            cols.Add(key: "DateAdded", value: "date_added_date");
            cols.Add(key: "Remarks", value: "remarks_string");
            cols.Add(key: "Image", value: "image_image");


            return cols;
        }
        #endregion

        /*
         * @modified by     :   AC <aabasolo@allegromicro.com>
         * @date            :   NOV. 24, 2016 3:33PM
         * @modification    :   changed the source to vwTransactionDetails_temp and/or vwTransactionDetails
         */
        #region GetItemDetailsInbox
        public DataTable GetTransDetails(int offset, int next, string where, string sorting, string headerKey)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetItemDetailsInboxCols();

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

            string tableName = "vwTransactionDetails_temp";

            if (!headerKey.IsNullOrWhiteSpace())
            {
                tableName = "vwTransactionDetails";

            }

            //set the table name and other params
            sql += " FROM " + tableName + " " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion

        /*
         * @modified by     :   AC <aabasolo@allegromicro.com>
         * @date            :   NOV. 24, 2016 3:33PM
         * @modification    :   changed the source to vwTransactionDetails_temp and/or vwTransactionDetails
         */
        #region GetItemDetailsInboxCount
        public int GetItemDetailsInboxCount(string where, string headerKey)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1";
            }
            else
            {
                where += " AND IsActive = 1";
            }

            string tableName = "vwTransactionDetails_temp";

            if (!headerKey.IsNullOrWhiteSpace())
            {
                tableName = "vwTransactionDetails";

            }

            //build the sql statement
            string sql = "SELECT COUNT(*) FROM " + tableName + " " + where;

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion


    }
}