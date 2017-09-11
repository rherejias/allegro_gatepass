using GatePass.Helpers;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class DraftsModel
    {

        private string tblUnitofMeasure;
        UploaderHelper upload = new UploaderHelper();


        /*
       * @author      :   AV <avillena@allegromicro.com>
       * @date        :   DEC. 15, 2016
       * @description :   contract the data grid column of all transaction
       */
        #region GetAllTrans_cols
        public Dictionary<string, string> GetAllTrans_cols()
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
            cols.Add(key: "ContactPersonCode", value: "contact_person_string");
            cols.Add(key: "SupplierName", value: "supplier_name_string");
            cols.Add(key: "ContactName", value: "contact_name_string");
            cols.Add(key: "DepartmentApproverCode", value: "dept_approver_code_string");
            cols.Add(key: "DepartmentApproverName", value: "dept_approver_name_string");

            return cols;
        }// End
        #endregion




        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get and display all transaction in data grid
        */
        #region GetAllTrans
        public DataTable GetAllTrans(int offset, int next, string where, string sorting, int addedby)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAllTrans_cols();

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
            sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + "AND Status = 'Drafted' AND AddedBy = " + addedby + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all transaction count of gatepass
        */
        #region GetAllTrans_count
        public int GetAllTrans_count(string where, int addedby)
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
            string sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND AddedBy = " + addedby + " AND Status = 'Drafted'";

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));

        }// End
        #endregion


        public int IsItemAndSerialisExistToDrafted_GP(string id, string itemCode, string serialNbr, int itemId)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionDetails where [Id] != '" + itemId + "' AND [HeaderCode] = '" + id + "' AND [ItemCode] = '" + itemCode + "' AND [SerialNbr] = '" + serialNbr + "'", CommandType.Text));
        }


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   addition of item details
        */
        #region UpdateItemsDraft
        public bool UpdateItemsDraft(Details item)
        {

            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@SessionId", value: item.SessionId),
                new SqlParameter(parameterName: "@HeaderCode", value: item.HeaderCode),
                new SqlParameter(parameterName: "@ID_AddedITem", value: item.Id),
                new SqlParameter(parameterName: "@ItemCode", value: item.ItemCode),  //must not be empty
                new SqlParameter(parameterName: "@Quantity", value: item.Quantity),
                new SqlParameter(parameterName: "@UnitOfMeasureCode", value: item.UnitOfMeasureCode),
                new SqlParameter(parameterName: "@CategoryCode", value: item.CategoryCode),
                new SqlParameter(parameterName: "@ItemTypeCode", value: item.ItemTypeCode),
                new SqlParameter(parameterName: "@SerialNbr", value: item.SerialNbr),
                new SqlParameter(parameterName: "@TagNbr", value: item.TagNbr),
                new SqlParameter(parameterName: "@PONbr", value: item.PONbr),
                new SqlParameter(parameterName: "@IsActive", value: item.IsActive),
                new SqlParameter(parameterName: "@AddedBy", value: item.AddedBy),
                new SqlParameter(parameterName: "@DateAdded", value: item.DateAdded),
                new SqlParameter(parameterName: "@Remarks", value: item.Remarks),
                new SqlParameter(parameterName: "@Image", value: (item.Image == "") ? "" : UploaderHelper.FileName)

            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spUpdateAddedItemMyDraft", params_: @params, type_: CommandType.StoredProcedure);
        }// End
        #endregion



        /// <summary>
        /// desc: count of non returnable item into temp details table
        /// by: avillena@allegromicro.com
        /// version : 1.0
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public int Hasnonreturnableitem(int draftId)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from tblTransactionDetails where [ItemTypeCode] = '20170200000' AND [HeaderCode] = '" + draftId + "'", type_: CommandType.Text));
        }



    }
}