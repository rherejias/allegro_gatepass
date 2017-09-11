using System;
using System.Data;

namespace GatePass.Models
{
    public class HomeModal
    {

        public int GetCountAllSubmitted(int addedby)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionHeaders where [AddedBy] =  " + addedby + " AND Status = 'Submitted' AND IsActive = 1", CommandType.Text));
        }

        public int GetCountAllApproved(int addedby)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionHeaders where [AddedBy] =  " + addedby + " AND Status = 'Approved' AND IsActive = 1", CommandType.Text));
        }

        public int GetCountAllRejected(int addedby)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionHeaders where [AddedBy] =  " + addedby + " AND Status = 'Rejected' AND IsActive = 1", CommandType.Text));
        }

        public int GetCountAllDrafted(int addedby)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionHeaders where [AddedBy] =  " + addedby + " AND Status = 'Drafted' AND IsActive = 1", CommandType.Text));
        }
        public int GetCountAllTotal(int addedby)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from tblTransactionHeaders where [AddedBy] =  " + addedby + " AND IsActive = 1", CommandType.Text));
        }


        //// GUARD DASHBOARD

        public int GetCountForApprovalGuard()
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwToBeApprovedByGuardApprover_finalCombo]", type_: CommandType.Text));
        }
        public int GetCountApprovedGuard()
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [tblGuardApprovedLogs] Where [GuardRights] = 'Security' AND [GuardIsApproved] = 1", type_: CommandType.Text));
        }

        public int GetCountReturnSlipGuard()
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "SELECT COUNT(*) FROM vwTransactionHeaderwithSupplierContactPerson Where IsActive = 1 AND ReturnSlipStatus <> 'Returned' AND ReturnSlipStatus <> '' AND ReturnSlipStatus <> 'Non Returnable'", type_: CommandType.Text));
        }
        public int GetCountRejectedGuard()
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [tblGuardApprovedLogs] Where [GuardRights] = 'Security' AND [GuardIsApproved] = 0", type_: CommandType.Text));
        }


        ////
        /// approver dashboard
        /// 
        public int GetifLoginApprover(string usercode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
                ("Select count(*) from [tblApprovalDepartmentRelationship] where [UserCode] =  " + usercode + " AND IsActive = 1", CommandType.Text));
        }


        /// desc : get count of approved gate pass
        /// author : avillena
        /// version : 1.0
        public int GetCountApprovedbydepartment(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwReportDeptApproved] where [DepartmentApproverCode] = '" + userloginCode + "' AND [IsApproved] = 1", type_: CommandType.Text));
        }
        public int GetCountApprovedbyIT(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwReportITApproved] where [ApproverCode] = '" + userloginCode + "' AND [IsApproved] = 1", type_: CommandType.Text));
        }
        public int GetCountApprovedbypurchasing(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwReportPurch] where [ApproverCode] = '" + userloginCode + "' AND [IsApproved] = 1", type_: CommandType.Text));
        }
        public int GetCountApprovedbyaccounting(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwReportAcct] where [ApproverCode] = '" + userloginCode + "' AND [IsApproved] = 1", type_: CommandType.Text));
        }
        ///end


        /// desc : get count of pending for approval of approver
        /// author : avillena
        /// version : 1.0
        public int GetCountForApprovalApproverDepartment(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwTransactionHeaderWithUserDepartment1] where [DepartmentApproverCode] = '" + userloginCode + "' AND [IsApproved] is null", type_: CommandType.Text));
        }
        public int GetCountForApprovalApproverIT(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwToBeApprovedByITRelated] where [ToBeApprovedBy] = '" + userloginCode + "' AND [IsApproved] is null", type_: CommandType.Text));
        }
        public int GetCountForApprovalApproverPurchasing(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwToBeApprovedByPurchasingRelated] where [ToBeApprovedBy] = '" + userloginCode + "' AND [IsApproved] is null", type_: CommandType.Text));
        }
        public int GetCountForApprovalApproverAccounting(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwToBeApprovedByAccountingApprover_finalCombo] where [ToBeApprovedBy] = '" + userloginCode + "' AND [IsApproved] is null", type_: CommandType.Text));
        }
        /// end 

        /// desc : get count of pending for approval of approver
        /// author : avillena
        /// version : 1.0
        public int GetCountRejecteddepartment(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwRejectedByDeptHead] where [ApproverCode] = '" + userloginCode + "' AND [IsApproved] = 0", type_: CommandType.Text));
        }
        public int GetCountRejectedIT(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwReportITReject] where [ApproverCode] = '" + userloginCode + "' AND [IsApproved] = 0", type_: CommandType.Text));
        }
        public int GetCountRejectedpurchasing(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwReportPurchReject] where [ApproverCode] = '" + userloginCode + "' AND [IsApproved] = 0", type_: CommandType.Text));
        }
        public int GetCountRejectedaccounting(string userloginCode)
        {
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery
            (strQuery: "Select count(*) from [vwReportAcctReject] where [ApproverCode] = '" + userloginCode + "' AND [IsApproved] = 0", type_: CommandType.Text));
        }
        /// end 

    }
}