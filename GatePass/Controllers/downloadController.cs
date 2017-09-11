using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class downloadController : Controller
    {
        private Dictionary<string, object> response = new Dictionary<string, object>();




        /// <summary>
        /// @author: rherejias
        /// @desc: get the download file from tbl 
        /// @version : 1.0
        /// </summary>
        /// <returns></returns>
        // GET: download
        public JsonResult download(string filename, string filepath, string uname)

        {
            string writePath;
            string savePath;
            string user = Helpers.Common.GetWebCurrentUser(Helpers.Common.WebUserInformation.Username);
            try
            {
                if (Session["user_type"].ToString() == "ampiguard")
                {
                    writePath = @"C:\Users\" + ConfigurationManager.AppSettings["GuardUsername"] + "\\Downloads\\";
                    savePath = @"C:\Users\" + ConfigurationManager.AppSettings["GuardUsername"] + "\\Downloads\\";
                }
                else
                {
                    writePath = @"C:\Users\" + uname + "\\Downloads\\";
                    savePath = @"C:\Users\" + uname + "\\Downloads\\";
                }


                byte[] fileBytes = System.IO.File.ReadAllBytes(ConfigurationManager.AppSettings[filepath] + filename);
                System.IO.File.WriteAllBytes(writePath + filename, fileBytes);
                Process.Start(savePath + filename);

                response.Add("success", true);
                response.Add("failed", false);
                response.Add("message", "Download successful.");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("failed", true);
                response.Add("message", e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}