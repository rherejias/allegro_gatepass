using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GatePass.Models
{
    public class ForApprovalModel
    {
        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   construct the data grid column of all transaction
        */
        #region GetAllTransForApprovalcols
        public Dictionary<string, string> GetAllTransForApprovalcols()
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
            return cols;
        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all transaction count of gatepass
        */
        #region GetAllTransForApprovalCount
        public int GetAllTransForApprovalCount(string where, string userdept, string userlogincode)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND Department = '" + userdept + "' AND DepartmentApproverCode = '" + userlogincode + "' AND IsApproved is null";
            }
            else
            {
                where += " AND IsActive = 1 AND Department = '" + userdept + "' AND DepartmentApproverCode = '" + userlogincode + "' AND IsApproved is null";
            }
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwTransactionHeaderWithUserDepartment1 " + where + " AND Status = 'Submitted'";

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));

        }// End
        #endregion

        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   get and display all transaction in data grid
      */
        #region GetAllTransForApproval
        public DataTable GetAllTransForApproval(int offset, int next, string where, string sorting, string userdept, string userlogincode)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAllTransForApprovalcols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND Department = '" + userdept + "' AND DepartmentApproverCode = '" + userlogincode + "' AND IsApproved is null";
            }
            else
            {
                where += " AND IsActive = 1 AND Department = '" + userdept + "' AND DepartmentApproverCode = '" + userlogincode + "' AND IsApproved is null";
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
            sql += " FROM vwTransactionHeaderWithUserDepartment1 " + where + " AND Status='Submitted' ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
        #endregion


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   get all transaction count of gatepass for IT Related Approver
        */
        #region GetAllTransForITApprovalCount
        public int GetAllTransForITApprovalCount(string where, string userlogincode)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            else
            {
                where += " AND IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwToBeApprovedByITRelated " + where + " AND Status = 'Submitted'";

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));

        }// End
        #endregion

        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   get and display all transaction in data grid
      */
        #region GetAllTransForITRelatedApproval
        public DataTable GetAllTransForITRelatedApproval(int offset, int next, string where, string sorting, string userlogincode)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAllTransForApprovalcols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            else
            {
                where += " AND IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
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
            sql += " FROM vwToBeApprovedByITRelated " + where + " AND Status='Submitted' ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
        #endregion





        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   get all transaction count of gatepass for IT Related Approver
      */
        #region GetAllTransForPurchasingApprovalCount
        public int GetAllTransForPurchasingApprovalCount(string where, string userlogincode)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            else
            {
                where += " AND IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwToBeApprovedByPurchasingRelated " + where + " AND Status = 'Submitted'";

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));

        }// End
        #endregion

        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   get and display all transaction in data grid
      */
        #region GetAllTransForPurchasingRelatedApproval
        public DataTable GetAllTransForPurchasingRelatedApproval(int offset, int next, string where, string sorting, string userlogincode)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAllTransForApprovalcols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            else
            {
                where += " AND IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
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
            sql += " FROM vwToBeApprovedByPurchasingRelated " + where + " AND Status='Submitted' ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
        #endregion











        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   get all transaction count of gatepass for IT Related Approver
      */
        #region GetAllTransForAccountingApprovalCount
        public int GetAllTransForAccountingApprovalCount(string where, string userlogincode)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            else
            {
                where += " AND IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            //build the sql statement
            string sql = "SELECT COUNT(*) FROM vwToBeApprovedByAccountingApprover_finalCombo " + where + " AND Status = 'Submitted'";

            //execute the sql statement
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));

        }// End
        #endregion

        /*
      * @author      :   AV <avillena@allegromicro.com>
      * @date        :   DEC. 15, 2016
      * @description :   get and display all transaction in data grid
      */
        #region GetAllTransForAccountingRelatedApproval
        public DataTable GetAllTransForAccountingRelatedApproval(int offset, int next, string where, string sorting, string userlogincode)
        {
            //call for the method in getting the columns
            Dictionary<string, string> cols = GetAllTransForApprovalcols();

            //set default sorting
            if (sorting == "")
            {
                sorting = "DateAdded DESC";
            }

            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
            }
            else
            {
                where += " AND IsActive = 1 AND ToBeApprovedBy = '" + userlogincode + "' AND IsApproved is null AND IsOverride is null";
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
            sql += " FROM vwToBeApprovedByAccountingApprover_finalCombo " + where + " AND Status='Submitted' ORDER BY " + sorting + " " + pagination + ";";

            //execute the sql statement
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);

        }// End
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

        public string GetDeptApproverEmail(int loginId)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Email] from tblUsers where [Id] = " + loginId + "", CommandType.Text).ToString();
        }




        /// <summary>
        /// desc: to get the dept approver
        /// by: avillena@allegromicro.com
        /// date: january 11 ,2017
        /// </summary>
        /// <param name="deptCode"></param>
        ///// <returns></returns>
        public string GetGatePassFromLink(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [ImpexRefNbr] from vwTransactionHeaderwithSupplierContactPerson where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }

        public string GetGatePassFromLinkTransType(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [TransType] from vwTransactionHeaderwithSupplierContactPerson where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }
        public string GetGatePassReturnDate(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select COALESCE([ReturnDate],'') from vwTransactionHeaderwithSupplierContactPerson where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }

        public string GetGatePassSupplier(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [SupplierName] from vwTransactionHeaderwithSupplierContactPerson where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }
        public string GetGatePassAttachment(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Attachment] from vwTransactionHeaderwithSupplierContactPerson where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }
        public string GetGatePassPurpose(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Purpose] from vwTransactionHeaderwithSupplierContactPerson where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }
        public string GetGatePassContactName(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select COALESCE(ContactName,'') from vwTransactionHeaderwithSupplierContactPerson where [Code] = '" + headercode + "';", CommandType.Text).ToString();
        }

        /// count of headercode if existing
        public int IsHeaderCodeIsExisting(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from vwTransactionHeaderwithSupplierContactPerson where [Code] =  '" + headercode + "' AND Status <> 'Drafted' AND IsActive = 1", CommandType.Text));
        }


        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   approval of gatepass by logging to tblapprovallogs
        */
        #region UpdateUpprovalLogsByDept
        public int UpdateUpprovalLogsByDept(ApprovalLogsObject approvallogs, string approvaltype)
        {

            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery("exec ApprovedByDepatment_UpdateApproverLogs " +
                " @ApprovalType = '" + approvaltype + "'," +
                " @DateModified = '" + approvallogs.DateModified + "'," +
                " @TransHeaderCode = '" + approvallogs.Code + "'", CommandType.Text));

        }// End

        /*
        * @author      :   AV <avillena@allegromicro.com>
        * @date        :   DEC. 15, 2016
        * @description :   rejection of gatepass by logging to tblapprovallogs
        */
        //#region UpdateUpprovalLogsByDept_Rejected
        //public int UpdateUpprovalLogsByDept_Rejected(ArppovalLogs approvallogs, string approvaltype)
        //{

        //    return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery("exec RejectedByApprovers_UpdateApproverLogs " +
        //        " @ApprovalType = '" + approvaltype + "'," +
        //        " @DateModified = '" + approvallogs.DateModified + "'," +
        //        " @RejectedRemarks = '" + approvallogs.Remarks + "'," +
        //        " @RejectedBy = '" + approvallogs.RejectedBy + "'," +
        //        " @TransHeaderCode = '" + approvallogs.Code + "'", CommandType.Text));

        //}// End

        public bool UpdateUpprovalLogsByDept_Rejected(ApprovalLogsObject approvalLog, string approvaltype)
        {
            var @params = new SqlParameter[] {

                new SqlParameter(parameterName: "ApprovalType", value: approvaltype),
                new SqlParameter(parameterName: "DateRejected", value: approvalLog.DateRejected),
                new SqlParameter(parameterName: "RejectedRemarks", value: approvalLog.Remarks),
                new SqlParameter(parameterName: "RejectedBy", value: approvalLog.RejectedBy),
                new SqlParameter(parameterName: "TransHeaderCode", value: approvalLog.Code),

            };
            return Library.ConnectionString.returnCon.executeQuery(strQuery: "RejectedByApprovers_UpdateApproverLogs", params_: @params, type_: CommandType.StoredProcedure);
        }




        #endregion
        /// <summary>
        /// /
        /// </summary>
        /// <param name="headercode"></param>
        /// <returns></returns>
        public int GettheNextApprover(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Id] from tblApprovalLogs where [TransHeaderCode] = '" + headercode + "' AND ApprovalTypeCode= '" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "';", CommandType.Text));
        }

        /// <summary>
        /// IT and Purchasing
        /// </summary>
        /// <param name="headercode"></param>
        /// <param name="tlblogsId"></param>
        /// <returns></returns>
        public DataTable GetITandPurch(string headercode)
        {
            return Library.ConnectionString.returnCon.executeSelectQuery
                ("Select [Email], [ToBeApprovedBy], Replace(ApprovalType, ' Primary Approver', '') as Approvaltype  from vwOverride where [TransHeaderCode] = '" + headercode + "' AND IsOverride is null AND [ApprovalTypeCode] NOT IN ('" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "', '" + ConfigurationManager.AppSettings["accountingApprover"] + "')", CommandType.Text);
        }



        public DataTable GettheAccountingApprover(string headercode)
        {
            return Library.ConnectionString.returnCon.executeSelectQuery
                 ("Select [Email], [ToBeApprovedBy], Replace(ApprovalType, ' Primary Approver', '') as Approvaltype from vwOverride where [TransHeaderCode] = '" + headercode + "' AND [ApprovalTypeCode] = '" + ConfigurationManager.AppSettings["accountingApprover"] + "' AND [IsOverride] is null", CommandType.Text);
        }

        /// <summary>
        /// get the email of approver who was approved the gate pass
        /// </summary>
        /// <param name="headercode"></param>
        /// <returns></returns>
        public DataTable GettheEmailWhoApprovedGP(string headercode)
        {
            return Library.ConnectionString.returnCon.executeSelectQuery
                 ("Select [Email] from vwOverride where [TransHeaderCode] = '" + headercode + "' AND [IsApproved] = 1", CommandType.Text);
        }



        /// count of purchasing approver if existing
        public int IsHaveAPurchasingApprover(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from vwOverride where [TransHeaderCode] = '" + headercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["purchasingApprover"] + "' AND [IsApproved] is null AND [IsOverride] is null", CommandType.Text));
        }

        /// count of it approver if existing
        public int IsHaveA_ITApproverandItsApproved(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from vwOverride where [TransHeaderCode] = '" + headercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["itApprover"] + "' AND [IsApproved] is null AND [IsOverride] is null", CommandType.Text));
        }






        /// <summary>
        /// desc: get the count of gatepass if its already approved?
        /// by: avillena@allegromicro.com
        /// date: january 11 ,2017
        /// </summary>
        /// <param name="deptCode"></param>
        ///// <returns></returns>
        public int GetCountIfGatePassAlreadyApproved(string headercode, string approvercode, string approvertype)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblApprovalLogs where [TransHeaderCode] = '" + headercode + "' AND [ToBeApprovedBy] =  '" + approvercode + "' AND [ApprovalTypeCode] = '" + approvertype + "' AND IsApproved is not null", CommandType.Text));
        }



        public int GetApproverHeaderIDDepartment(string approvercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select COALESCE([Id],0) from vwApprovers where [UserCode] = '" + approvercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["departmentApprover"] + "' AND IsActive = 1", CommandType.Text));
        }
        public int GetApproverHeaderIDIT(string approvercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select COALESCE([Id],0) from vwApprovers where [UserCode] = '" + approvercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["itApprover"] + "' AND IsActive = 1", CommandType.Text));
        }
        public int GetApproverHeaderIDPurchasing(string approvercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select COALESCE([Id],0) from vwApprovers where [UserCode] = '" + approvercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["purchasingApprover"] + "' AND IsActive = 1", CommandType.Text));
        }
        public int GetApproverHeaderIDAccounting(string approvercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select COALESCE([Id],0) from vwApprovers where [UserCode] = '" + approvercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["accountingApprover"] + "' AND IsActive = 1", CommandType.Text));
        }




        //get count if the header was already approved?
        public int DeparmentIfApproved(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select  count(*) from tblApprovalLogs where [TransHeaderCode] = '" + headercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["departmentApprover"] + "' AND [IsApproved] = 1", CommandType.Text));
        }
        public int ITIfApproved(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select  count(*) from tblApprovalLogs where [TransHeaderCode] = '" + headercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["itApprover"] + "' AND [IsApproved] = 1", CommandType.Text));
        }
        public int PurchasingIfApproved(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select  count(*) from tblApprovalLogs where [TransHeaderCode] = '" + headercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["purchasingApprover"] + "' AND [IsApproved] = 1", CommandType.Text));
        }
        public int AccountingIfApproved(string headercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select  count(*) from tblApprovalLogs where [TransHeaderCode] = '" + headercode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["accountingApprover"] + "' AND [IsApproved] = 1", CommandType.Text));
        }
        // END //




        /// <summary>
        /// desc: get the current login if approver and what type
        /// by: avillena@allegromicro.com
        /// date: january 11 ,2017
        /// </summary>
        /// <param name="deptCode"></param>
        ///// <returns></returns>
        public int GetIfDepartmentApprover(string userloginCode, int deptapprovalheaderID)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery

                ("Select count(*) from vwApprovers where [UserCode] = '" + userloginCode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "' AND IsActive = 1", CommandType.Text));
        }

        public string GetDepartmentApproverType(string userloginCode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [ApprovalTypeCode] from vwApprovers where [UserCode] = '" + userloginCode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["departmentApprover"].ToString() + "' AND IsActive = 1", CommandType.Text).ToString();
        }


        public int GetIfITApprover(string userloginCode, int itapprovalheaderID)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery

                ("Select count(*) from vwApprovers where [UserCode] = '" + userloginCode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["itApprover"] + "' AND IsActive = 1", CommandType.Text));
        }

        public string GetITApproverType(string userloginCode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [ApprovalTypeCode] from vwApprovers where [UserCode] = '" + userloginCode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["itApprover"] + "' AND IsActive = 1", CommandType.Text).ToString();
        }

        public int GetIfPurchasingApprover(string userloginCode, int purchasingapprovalheaderID)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery

                ("Select count(*) from vwApprovers where [UserCode] = '" + userloginCode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["purchasingApprover"] + "' AND IsActive = 1", CommandType.Text));

        }

        public string GetPurchasingApproverType(string userloginCode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery

                ("Select [ApprovalTypeCode] from vwApprovers where [UserCode] = '" + userloginCode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["purchasingApprover"] + "' AND IsActive = 1", CommandType.Text).ToString();

        }

        public int GetIfAccountingApprover(string userloginCode, int accountingapprovalheaderID)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            ("Select count(*) from vwApprovers where [UserCode] = '" + userloginCode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["accountingApprover"] + "' AND IsActive = 1", CommandType.Text));

        }

        public string GetAccountingApproverType(string userloginCode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [ApprovalTypeCode] from vwApprovers where [UserCode] = '" + userloginCode + "' AND [ApprovalTypeCode] =  '" + ConfigurationManager.AppSettings["accountingApprover"] + "' AND IsActive = 1", CommandType.Text).ToString();
        }


        public string GetTobeApprovedBy(int nextapproverId, string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [ApprovalTypeCode] from [tblApprovalLogs] where [Id] = " + nextapproverId + " AND [TransHeaderCode] =  '" + headercode + "' AND [IsApproved] is null", CommandType.Text).ToString();
        }


        ///approval tracking
        public int GatePassIdNextApprover(string headercode, string approvercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select Coalesce(min(Id),0) from [vwOverride] where [IsApproved] is null AND [TransHeaderCode] = '" + headercode + "' AND [ToBeApprovedBy] = '" + approvercode + "'", CommandType.Text));
        }


        public string GetGPStatus(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select [Status] from [tblTransactionHeaders] where [Code] = '" + headercode + "' AND [IsActive] =  1", CommandType.Text).ToString();
        }


        //// Get the full name who's reject the gate pass
        public string GetApproverFullname(string email)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select CONCAT(LastName, ', ', GivenName) AS Rejectedby from tblUsers where [Email] = '" + email + "'", CommandType.Text).ToString();
        }


        // desc : get the user code of the gate pass owner | author :  avillena@allegromciro.com | date : 04/20/17
        public string GetgatepassownerCode(string headercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
                ("Select UserCode from [vwTransactionHeaderwithSupplierContactPerson] where [Code] = '" + headercode + "'", CommandType.Text).ToString();
        }
        public string GetgatepassownerName(string usercode)
        {
            return Library.ConnectionString.returnCon.executeScalarQuery
               ("Select CONCAT(LastName, ', ', GivenName) AS GatePassOwner from tblUsers where [Code] = '" + usercode + "'", CommandType.Text).ToString();
        }
        //End

    }
}