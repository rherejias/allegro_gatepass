using GatePass.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GatePass.Models
{
    public class PermissionModels
    {
        CustomHelper custom_helper = new CustomHelper();

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   11/25/2016 2:46 PM
         * @description :   this method will update the permission per user
         * @params      :   type(string); parentid(int); id(int); flag(int)
         * @returns     :   bool (true or false)
         */
        public bool UpdatePermission(string type, int parentid, int id, bool flag, int userid)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@type", type),
                new SqlParameter("@parentid", parentid),
                new SqlParameter("@id", id),
                new SqlParameter("@userid", userid),
                new SqlParameter("@flag", flag==true ? 1 : 0),
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdatePermissions", params_, CommandType.StoredProcedure);
        }
    }
}