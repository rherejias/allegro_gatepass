using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GatePass.Models;
using GatePass.Helpers;
using System.Data;

namespace GatePass.Controllers
{
    public class ModulesController : Controller
    {
        ModuleModels modules = new ModuleModels();
        CustomHelper custom_helper = new CustomHelper();
        Dictionary<string, object> response = new Dictionary<string, object>();

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   11/25/2016 2:30 PM
         * @description :   this get all the modules in preparation for the tree view
         * @params      :   userId(int)
         * @returns     :   JSON object
         */
        [HttpGet]
        public JsonResult GetModules(int userId)
        {
            try
            {
                List<object> header = new List<object>();
                DataTable dtMod = modules.GetAvailabeModules();
                bool is_checked = false;
                List<int> active_for_user = GetUserModules(userId);
                int indx = 0;
                foreach (DataRow row in dtMod.Rows)
                {
                    Dictionary<string, object> details = new Dictionary<string, object>();

                    if (active_for_user.Contains(Int32.Parse(row["Id"].ToString())))
                    {
                        is_checked = true;
                    }
                    else
                    {
                        is_checked = false;
                    }

                    details.Add("id", row["id"].ToString());
                    details.Add("parent", (Int32.Parse(row["ParentId"].ToString()) == 0 || row["ParentId"].ToString() == "") ? "-1" : row["ParentId"].ToString());
                    details.Add("text", row["Name"].ToString());
                    details.Add("checked", is_checked);

                    header.Add(details);
                    indx++;
                }

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", header);
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /*
         * @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   11/25/2016 2:30 PM
         * @description :   get permissions per module per user
         * @params      :   userId(int)
         * @returns     :   array
         */
        public List<int> GetUserModules(int userId)
        {
            List<int> res = new List<int>();

            DataTable user_mod = modules.GetUserModules(userId);

            foreach (DataRow row in user_mod.Rows)
            {
                res.Add(Int32.Parse(row["ObjectId"].ToString()));
            }

            return res;
        }
    }
}