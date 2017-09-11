using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GatePass.Models
{
    public class EmailModels
    {

         /*
         * @author      :   AV <avillena@allegromicro.com>
         * @date        :   DEC. 15, 2016
         * @description :   get the email address of the approver for email notification
         */
        public DataTable ReadEmailForReturnSlip(string usercode, string type)
        {
            //prepare stored procedure parameters
            var @params = new SqlParameter[] {
                new SqlParameter(parameterName: "@UserCode", value: usercode),
                new SqlParameter(parameterName: "@ApproverType", value: type)
            };

            //call the stored procedure's name and pass the params_ above
            return Library.ConnectionString.returnCon.executeSelectQuery(strQuery: "spReadApprovers", type_: CommandType.StoredProcedure, params_: @params);
        }// End

    }
}