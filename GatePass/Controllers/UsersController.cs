using GatePass.Helpers;
using GatePass.Models;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class UsersController : Controller
    {
        private CustomHelper custom_helper = new CustomHelper();
        private UserModels user_model = new UserModels();
        private Dictionary<string, object> response = new Dictionary<string, object>();

        /* 
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   11/25/2016 2:24 PM
         * @description :   get registered users
         * @parameters  : n/a
         * @returns     : json object
         */
        #region GetRegisteredUsers
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetRegisteredUsers()
        {
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            DataTable dtUsers = user_model.GetRegisteredUsers();
            var data = custom_helper.DataTableToJson(dtUsers);

            result_config.Add("data", data);

            //prepare column config
            var cols = new List<string>();
            foreach (DataColumn column in dtUsers.Columns)
            {
                cols.Add(column.ColumnName);
            }
            Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
            result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}