using GatePass.Helpers;
using GatePass.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class TransactionModels
    {
        private string tblUnitofMeasure;
        UploaderHelper upload = new UploaderHelper();

        /*
       * @author      :   REN <rherejias@allegromicro.com>
       * @date        :   JAN 3, 2017
       * @description :   addtion of attachment on return slip 
       */
        #region rherjias
        public bool returnAttachment(TransactionHeaderObject transactionDetails)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@returnAttachment", UploaderHelper.FileName),
                new SqlParameter("@Code", transactionDetails.Code),
                new SqlParameter("@Id", transactionDetails.Id),
                new SqlParameter("@AddedBy", transactionDetails.AddedBy),
                new SqlParameter("@DateAdded", transactionDetails.DateAdded),
                new SqlParameter("@IP",transactionDetails.IP),
                new SqlParameter("@MAC", transactionDetails.MAC)
            };

            return Library.ConnectionString.returnCon.executeQuery("spReturnSlipAttachment", params_, CommandType.StoredProcedure);
        }

        public bool returnDelAttachment(TransactionHeaderObject transactionDetails)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Code", transactionDetails.Code),
                 new SqlParameter("@Id", transactionDetails.Id),
                new SqlParameter("@AddedBy", transactionDetails.AddedBy),
                new SqlParameter("@DateAdded", transactionDetails.DateAdded),
                new SqlParameter("@IP",transactionDetails.IP),
                new SqlParameter("@MAC", transactionDetails.MAC)
               // new SqlParameter("@table", "tblSuppliers")
            };

            return Library.ConnectionString.returnCon.executeQuery("spDelReturnSlipAttachment", params_, CommandType.StoredProcedure);
        }
        #endregion

        /*
         * @author      :   AV <avillena@allegromicro.com>
         * @date        :   DEC. 15, 2016
         * @description :   addition of header info of gate pass
         */
        #region AddHeader
        public string AddHeader(TransactionHeaderObject trans_object, string rdate)
        {
            string attachment;
            //return Library.ConnectionString.returnCon.executeQuery("spAddUser", params_, CommandType.StoredProcedure);
            if (trans_object.Attachment == "")
                attachment = "";
            else
                attachment = UploaderHelper.FileName;

            return Library.ConnectionString.returnCon.executeScalarQuery("exec spAddTransactionHeader " +
                " @SessionId = '" + trans_object.SessionId + "'," +
                " @ImpexRefNbr = '" + trans_object.ImpexRefNbr + "'," +
                " @SupplierCode = '" + trans_object.SupplierCode + "'," +
                " @ContactPersonCode = '" + trans_object.ContactPersonCode + "'," +
                " @DepartmentCode = '" + trans_object.DepartmentCode + "'," +
                // " @ReturnDate = '" + trans_object.ReturnDate.ToString() + "'," +
                " @ReturnDate = '" + rdate + "'," +
                " @TransType = '" + trans_object.TransType + "'," +
                " @CategoryCode = '" + trans_object.CategoryCode + "'," +
                " @TypeCode = '" + trans_object.TypeCode + "'," +
                " @Purpose = '" + trans_object.Purpose + "'," +
                " @IsActive = '" + trans_object.IsActive + "'," +
                " @Status = '" + trans_object.Status + "'," +
                " @AddedBy = '" + trans_object.AddedBy + "'," +
                " @UserCode = '" + trans_object.UserCode + "'," +
                " @ApproverCode = '" + trans_object.DepartmentApproverCode + "'," +
                " @DateAdded = '" + trans_object.DateAdded.ToString() + "'," +
                " @IP = '" + Helpers.CustomHelper.GetLocalIPAddress() + "'," +
                " @MAC = '" + Helpers.CustomHelper.GetMACAddress() + "'," +
                " @ReturnSlipStatus = '" + trans_object.ReturnSlipStatus + "'," +
                " @Comment = '" + "Test Comment Incomplete data" + "'," +
                " @Attachment = '" + attachment + "'", CommandType.Text).ToString();


        }// End
        #endregion



        /*
         * @author      :   AV <avillena@allegromicro.com>
         * @date        :   DEC. 15, 2016
         * @description :   addition of item details
         */
        #region AddItems
        public bool AddItems(Details item)
        {

            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@SessionId", value: item.SessionId),
                new SqlParameter(parameterName: "@Code", value: ""),
                new SqlParameter(parameterName: "@HeaderCode", value: item.HeaderCode),
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

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spAddTransactionDetail", params_: @params, type_: CommandType.StoredProcedure);
        }// End
        #endregion

        /*
         * @modified by     :   AC <aabasolo@allegromicro.com>
         * @date            :   NOV. 24, 2016 3:33PM
         * @modification    :   added session id, item name, category name, item type, dateadded in preparation of the view
         */
        #region GetTransDetails_cols
        public Dictionary<string, string> GetTransDetails_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();

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
            cols.Add(key: "ItemTypeName", value: "type_string");
            cols.Add(key: "DateAdded", value: "date_added_date");
            cols.Add(key: "Remarks", value: "remarks_string");
            cols.Add(key: "NameDepartment", value: "related_to_string");
            cols.Add(key: "Image", value: "image_image");
            cols.Add(key: "CategoryCode", value: "category_code_string");
            cols.Add(key: "ItemTypeCode", value: "item_type_code_string");



            return cols;
        }
        #endregion

        /*
         * @modified by     :   AC <aabasolo@allegromicro.com>
         * @date            :   NOV. 24, 2016 3:33PM
         * @modification    :   changed the source to vwTransactionDetails_temp and/or vwTransactionDetails
         */
        #region GetTransDetails
        public DataTable GetTransDetails(int offset, int next, string where, string sorting, string headerKey)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetTransDetails_cols();

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
        #region GetTransDetails_count
        public int GetTransDetails_count(string where, string headerKey)
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
            cols.Add(key: "SupplierName", value: "supplier_name_string");
            cols.Add(key: "ContactName", value: "contact_name_string");
            cols.Add(key: "Supplier_Address", value: "supplier_address_string");
            cols.Add(key: "Owner", value: "owner_string");
            //cols.Add("TagNbr", "tag_#_string");
            //cols.Add("PONbr", "p_o_#_string");
            //cols.Add("IsActive", "active_bool");
            //cols.Add("Remarks", "remarks_string");

            return cols;
        }// End
        #endregion




        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get and display all transaction in data grid
        */
        #region GetAllTrans
        public DataTable GetAllTrans(int offset, int next, string where, string sorting, string filterType, int addedby, bool viewAllTrans, string department)
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

            //sql = "Exec spApprovalTracking";


            if (viewAllTrans == true)
            {
                sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + "  AND Status <> 'Drafted' AND Department= '" + department + "' ORDER BY " + sorting + " " + pagination + ";";
            }
            else
            {
                sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + "  AND Status <> 'Drafted' AND AddedBy= " + addedby + " ORDER BY " + sorting + " " + pagination + ";";
            }



            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all transaction count of gatepass
        */
        #region GetAllTrans_count
        public int GetAllTrans_count(string where, int addedby, bool viewAllTrans, string department)
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
            //build the sql statement
            if (viewAllTrans == true)
            {
                sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND Status <> 'Drafted' AND Department = '" + department + "'";
            }
            else
            {
                sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND Status <> 'Drafted' AND AddedBy = " + addedby + "";
            }

            // string sql = "Exec spApprovalTracking";
            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));

        }// End
        #endregion



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   update the transaction header 
        */
        #region UpdateTransHeader      
        public bool UpdateTransHeader(TransactionHeaderObject HeaderComp, string returndate)
        {
            SqlParameter[] params_ = new SqlParameter[] {

                new SqlParameter("@Id", HeaderComp.Id),
                new SqlParameter("@ImpexRefNbr", HeaderComp.ImpexRefNbr),
                //new SqlParameter("@ReturnDate", HeaderComp.ReturnDate),
                new SqlParameter("@ReturnDate", returndate),
                new SqlParameter("@TransType", HeaderComp.TransType),
                new SqlParameter("@Purpose", HeaderComp.Purpose),
                new SqlParameter("@DateAdded", HeaderComp.DateAdded),
                new SqlParameter("@AddedBy", HeaderComp.AddedBy),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Attachment", HeaderComp.Attachment),
                new SqlParameter("@Supplier", HeaderComp.SupplierCode),
                 new SqlParameter("@ContactPerson", HeaderComp.ContactPersonCode),
                 new SqlParameter("@ApproverCode", HeaderComp.DepartmentApproverCode)

            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateTransHeaderDraft", params_, CommandType.StoredProcedure);

        }// End

        #endregion




        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   addition of item details in transaction drafted
        */
        #region AddItems_Update
        public int AddItems_Update(Details trans_addItems_update)
        {

            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery("exec spAddTransactionDetail_forUpdate " +
                " @Id = '" + trans_addItems_update.Id + "'," +
                " @SessionId = '" + trans_addItems_update.SessionId + "'," +
                " @Code = '" + "" + "'," +
                " @HeaderCode = '" + "" + "'," +
                " @ItemCode = '" + "" + "'," +
                " @Quantity = '" + trans_addItems_update.Quantity + "'," +
                " @UnitOfMeasureCode = '" + trans_addItems_update.UnitOfMeasureCode + "'," +
                " @SerialNbr = '" + trans_addItems_update.SerialNbr + "'," +
                " @TagNbr = '" + trans_addItems_update.TagNbr + "'," +
                " @PONbr = '" + trans_addItems_update.PONbr + "'," +
                " @IsActive = '" + trans_addItems_update.IsActive + "'," +
                " @AddedBy = '" + trans_addItems_update.AddedBy + "'," +
                " @DateAdded = '" + trans_addItems_update.DateAdded + "'," +
                " @Remarks = '" + "" + "'", CommandType.Text));

        }// End
        #endregion



        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   NOV. 22, 2016 9:06 AM
         * @description :   remove items with specific session id
         */
        #region RemoveItems_bySessionId
        public bool RemoveItems_bySessionId(string sessionId)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@SessionId", sessionId),
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeleteTransactionDetailBySessionId", params_, CommandType.StoredProcedure);
        }
        #endregion

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   NOV. 22, 2016 4:03 PM
         * @description :   update transaction header
         */
        #region UpdateHeader
        public bool UpdateHeader(TransactionHeaderObject transObject, string returndate)
        {

            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@Id", value: transObject.Id),
                new SqlParameter(parameterName:"@ImpexRefNbr",value: transObject.ImpexRefNbr),
                new SqlParameter(parameterName:"@DepartmentCode", value:transObject.DepartmentCode),
               // new SqlParameter(parameterName:"@ReturnDate",value: transObject.ReturnDate),
                new SqlParameter(parameterName:"@ReturnDate",value: returndate),
                new SqlParameter(parameterName:"@TransType",value: transObject.TransType),
                new SqlParameter(parameterName:"@CategoryCode",value: transObject.CategoryCode),
                new SqlParameter(parameterName:"@TypeCode",value: transObject.TypeCode),
                new SqlParameter(parameterName:"@Purpose",value: transObject.Purpose),
                new SqlParameter(parameterName:"@IsActive",value: transObject.IsActive),
                new SqlParameter(parameterName:"@Status",value: transObject.Status),
                new SqlParameter(parameterName:"@AddedBy",value: transObject.AddedBy),
                new SqlParameter(parameterName:"@DateAdded",value: transObject.DateAdded),
                new SqlParameter(parameterName:"@IP",value: Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter(parameterName:"@MAC",value: Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter(parameterName:"@Supplier",value: transObject.SupplierCode),
                new SqlParameter(parameterName:"@ContactPerson",value: transObject.ContactPersonCode),
                new SqlParameter(parameterName:"@Attachment",value: transObject.Attachment),
                new SqlParameter(parameterName:"@ApproverCode",value: transObject.DepartmentApproverCode)
            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spUpdateTransactionHeader", params_: @params, type_: CommandType.StoredProcedure);
        }
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   inactive the item details in transaction drafted
        */
        #region DeactivateAddItemDraft=
        public bool DeactivateAddItemDraft(Details item)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Id", item.Id),
                new SqlParameter("@IsActive", item.IsActive),
                new SqlParameter("@Action", item.Action),
               // new SqlParameter("@table", "tblSuppliers")
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivate_AddItem_Edit", params_, CommandType.StoredProcedure);

        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   inactive the transaction drafted
        */
        public bool DeactivateTransDraft(TransactionHeaderObject item)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Id", item.Id),
                new SqlParameter("@IsActive", item.IsActive),
                new SqlParameter("@AddedBy", item.AddedBy),
                new SqlParameter("@DateAdded", item.DateAdded),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
             
               // new SqlParameter("@table", "tblSuppliers")
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivate_TransactionsDraft", params_, CommandType.StoredProcedure);

        }// End



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   upload image file for item details
        */
        public bool uploadImage(string imageName)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Image", imageName),
            };
            return Library.ConnectionString.returnCon.executeQuery("spUploadImage", params_, CommandType.StoredProcedure);
        }// End

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
        public DataTable GetAllReturnSlip(int offset, int next, string where, string sorting, int isSearch, int addedby)
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
                sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND AddedBy = " + addedby + " AND ReturnSlipStatus <> '' ORDER BY " + sorting + " " + pagination + ";";
            }
            else
            {
                //set the table name and other params

                // sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND AddedBy = " + addedby + " AND ReturnSlipStatus = 'Not Returned' OR ReturnSlipStatus = 'Partially Returned' ORDER BY " + sorting + " " + pagination + ";";

                sql += " FROM vwTransactionHeaderwithSupplierContactPerson " + where + " AND AddedBy = " + addedby + " AND ReturnSlipStatus <> '' ORDER BY " + sorting + " " + pagination + ";";

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
        public int GetAllTransCount(string where, int isSearch, int addedby)
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
                sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + "AND AddedBy = " + addedby + " AND ReturnSlipStatus <> ''";
            }
            else
            {
                //build the sql statement
                // sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + "AND AddedBy = " + addedby + " AND ReturnSlipStatus = 'Not Returned' OR ReturnSlipStatus = 'Partially Returned'";
                sql = "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson " + where + "AND AddedBy = " + addedby + " AND ReturnSlipStatus <> ''";

            }
            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }// End
        #endregion



        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get and contract the column of item detail in data grid
        */
        #region GetItem_ReturnSlipCols
        public Dictionary<string, string> GetItem_ReturnSlipCols()
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
            cols.Add(key: "UOMName", value: "UoM_string");

            cols.Add(key: "CategoryName", value: "category_string");
            //cols.Add(key: "ItemType", value: "type_string");

            cols.Add(key: "DateAdded", value: "date_added_date");
            cols.Add(key: "Remarks", value: "remarks_string");
            cols.Add(key: "Image", value: "image_image");
            cols.Add(key: "ItemTypeCode", value: "item_type_code_string");
            cols.Add(key: "CategoryCode", value: "category_code_string");




            return cols;
        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get the item details of return slip
        */
        #region GetItem_ReturnSlip
        public DataTable GetItem_ReturnSlip(int offset, int next, string where, string sorting, string headerKey)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetItem_ReturnSlipCols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND HeaderCode = " + headerKey;
            }
            else
            {
                where += " AND IsActive = 1 AND HeaderCode = " + headerKey;
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

            string tableName = "vwTransactionDetails_temp";

            if (!headerKey.IsNullOrWhiteSpace())
            {
                tableName = "vwTransactionDetails";

            }

            //set the table name and other params
            sql += " FROM " + tableName + " " + where + " ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        } // End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   to get and display the detail item of gate pass
        */
        #region GetTransDetailsCount
        public int GetTransDetailsCount(string where, string headerKey)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND HeaderCode = " + headerKey;
            }
            else
            {
                where += " AND IsActive = 1 AND HeaderCode = " + headerKey;
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

        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   update return slip status of transaction details
        */
        #region UpdateItemsforReturn
        public bool UpdateItemsforReturn(Details itemDetails)
        {
            var @params = new SqlParameter[] {

                new SqlParameter(parameterName: "@Id", value: itemDetails.Id),
                new SqlParameter(parameterName: "@HeaderCode", value: itemDetails.HeaderCode),
                new SqlParameter(parameterName: "@ReturnSlipStatus", value: itemDetails.ReturnSlipStatus),
            };

            return Library.ConnectionString.returnCon.executeQuery(strQuery: "spUpdateReturnSlipStatus", params_: @params, type_: CommandType.StoredProcedure);
        }  // End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   to get gatepass header details by code in vwTransactionHeader views
        */
        public DataTable GetGatepassByCode(string headerCode)

        {
            return Library.ConnectionString.returnCon.executeSelectQuery("Select * from vwTransactionHeader Where code = '" + headerCode + "';", CommandType.Text);

        }// End
        public string GetRemarks(string headerCode, string approvalType)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("Select [Remarks] from tblApprovalLogs Where TransHeaderCode = '" + headerCode + "' AND ApprovalTypeCode = '" + approvalType + "' AND IsApproved = 0", CommandType.Text).ToString();
        }


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   to get gatepass details by code in vwTransactionDetails views
        */
        public DataTable GetGatepassDetailsByCode(string headerCode)

        {
            return Library.ConnectionString.returnCon.executeSelectQuery("Select * from vwTransactionDetails Where HeaderCode = '" + headerCode + "';", CommandType.Text);
        }// End


        #region ReadOverdueTransactionHeaders
        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   DEC 15, 2016
         * @description :   get overdue transaction headers
         */
        public DataTable ReadOverdueTransactionHeaders(string date)
        {
            string sql = "SELECT " +
                "t1.Code, " +
                "t1.ImpexRefNbr, " +
                "t1.ReturnDate, " +
                "t1.Purpose, " +
                "t1.UserCode, " +
                "CONCAT(t2.LastName,', ',t2.GivenName), " +
                "t2.Email, " +
                "t1.DateAdded, " +
                "t1.ReturnSlipStatus, " +
                "t2.Department " +
                "FROM tblTransactionHeaders t1 " +
                "LEFT OUTER JOIN tblUsers t2 " +
                "ON t2.Code = t1.UserCode " +
                "WHERE t1.Status ='Approved' and t1.ReturnSlipStatus='Not Returned' and t1.TransType='OUT' and t1.IsActive=1 and ReturnDate < '" + date + "';";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion

        #region ReadOverdueTransactionHeaders
        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   DEC 15, 2016
         * @description :   get overdue transaction details
         */
        public DataTable ReadOverdueTransactionItems(string headerCode)
        {
            string sql = "SELECT * FROM vwTransactionDetails WHERE HeaderCode='" + headerCode + "';";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion

        #region GetTransactionDetailsWithItemsToBeReturned
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 10, 2017 8:20 AM
         * @description     :   get transaction details with items to be returned
         */
        public DataTable GetTransactionDetailsWithItemsToBeReturned(string headerCode)
        {
            string sql = "SELECT " +
                         " Id " +
                         " ,HeaderCode " +
                         " ,ItemCode " +
                         " ,ItemName " +
                         " ,TagNbr " +
                         " ,PONbr " +
                         " ,SerialNbr " +
                         " ,CategoryName " +
                         " ,Quantity " +
                         " ,Image " +
                         " ,[GUID] " +
                         " ,(Quantity - QtyReturned) AS ToBeReturned " +
                         " ,(Quantity - QtyReturned) AS Remaining " +
                         " FROM [vwTransactionDetails] " +
                         " WHERE IsActive = 1 " +
                         " AND ItemTypeCode = '20170200000' " +
                         " AND HeaderCode = '" + headerCode + "' " +
                         " AND QtyReturned < Quantity; ";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        #endregion



        #region GetTransactionDetailsOfGatePass
        /*
         * @author          :   AC <aabasolo@allegromicro.com>
         * @date            :   JAN 10, 2017 8:20 AM
         * @description     :   get transaction details with items to be returned
         */
        public DataTable GetTransactionDetailsOfGatePass(string headerCode)
        {
            string sql = " SELECT " +
                         " Id " +
                         " ,HeaderCode " +
                         " ,ItemCode " +
                         " ,ItemName " +
                         " ,TagNbr " +
                         " ,PONbr " +
                         " ,SerialNbr " +
                         " ,CategoryName " +
                         " ,Quantity " +
                         " ,Image " +
                         " ,[GUID] " +
                         //  " ,(Quantity - QtyReturned) AS ToBeReturned " +
                         // " ,(Quantity - QtyReturned) AS Remaining " +
                         " FROM [vwTransactionDetails] " +
                         " WHERE IsActive = 1 " +
                         " AND HeaderCode = '" + headerCode + "'";
            //   " AND QtyReturned < Quantity; ";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        #endregion







        /// <summary>
        /// desc: to get the dept approver
        /// by: avillena@allegromicro.com
        /// date: january 11 ,2017
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public string Getdeptapprover(string deptCode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("select [ApproverName] from vwApprovers where [ApprovalType] = 'Primary Approver' AND [IsActive] = 1 AND [Department] = '" + deptCode + "';", CommandType.Text).ToString();
        }




        /// <summary>
        /// desc: to delete added items per user session in transactiondetails_temptbl
        /// by: avillena@allegromicro.com
        /// date: 03 29 ,2017
        /// </summary>
        /// <returns></returns>
        public DataTable DeleteAllItem_Temptbl(string userSessionid)
        {
            string sql = "Delete" +
                         " FROM [tblTransactionDetails_temp] Where SessionId = '" + userSessionid + "';";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }




        /// <summary>
        /// desc: to delete all added item in transactiondetails_temptbl
        /// by: avillena@allegromicro.com
        /// date: january 19 ,2017
        /// </summary>
        /// <returns></returns>
        public DataTable DeleteOneItem(string id)
        {
            string sql = "Delete" +
                         " FROM [tblTransactionDetails_temp] Where [Id] = '" + id + "';";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }

        public string getHeaderCodeEdit(string id)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery("SELECT [Code] FROM tblTransactionHeaders WHERE Id = " + id + "", CommandType.Text).ToString();
        }



        /// <summary>
        /// desc: to delete all added item in transactiondetails_temptbl
        /// by: avillena@allegromicro.com
        /// date: january 19 ,2017
        /// </summary>
        /// <returns></returns>
        public DataTable approvalTracking()
        {
            string sql = "Exec spApprovalTracking";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }


        ///select loginfullname
        public string Getfullnamelogin(int localloginId)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("select ([GivenName] + ' ' + [LastName]) as LoginFullName from [tblUsers] where [Id] = " + localloginId + "", CommandType.Text).ToString();
        }




        public DataTable GetApprovalStatus(string gatepassheadercode)
        {
            return Library.ConnectionString.returnCon.executeSelectQuery("SELECT * FROM tblApprovalLogs WHERE TransHeaderCode='" + gatepassheadercode + "'", CommandType.Text);
        }


        ///approval tracking
        public int GatePassApprovalTracking(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select Coalesce(min(Id),0) from [vwOverride] where [IsApproved] is null AND [TransHeaderCode] = '" + headercode + "'", CommandType.Text));
        }

        public string GatePassApprovalTrackingName(int idconverted)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
               ("Select (ApproverName + ' ' + '-' + ' ' + Replace(ApprovalType, 'Primary', '')) from [vwOverride] where [Id] = " + idconverted + "", CommandType.Text).ToString();
        }

        public string GetIfApproved(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
               ("Select [Status] from [tblTransactionHeaders] where [Code] = '" + headercode + "' AND [IsActive] = 1", CommandType.Text).ToString();
        }

        /// end

    }




}