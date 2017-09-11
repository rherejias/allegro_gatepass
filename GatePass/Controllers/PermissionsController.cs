using GatePass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class PermissionsController : Controller
    {
        Dictionary<string, object> response = new Dictionary<string, object>();
        PermissionModels permissions = new PermissionModels();

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   10/17/2016 3:25 PM
         * @description :   this method will update the permission per user
         * @method      :   get
         * @params      :   type(string); parentid(int); id(int); flag(int)
         * @returns     :   bool (true or false)
         */
        [HttpGet]
        public JsonResult Update(string type, int parentid, int id, bool flag, int userid)
        {
            try
            {
                bool f = false;

                if (flag)
                {
                    f = true;
                }

                if (permissions.UpdatePermission(type, parentid, id, flag, userid))
                {
                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", "Permissions successfully updated!");
                }
                else
                {
                    throw new Exception("Unable to update permissions. Please contact your systems administrator.");
                }

            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}