using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class AttachmentController : Controller
    {
        [HttpGet]
        public FileResult DownloadAttachment(string path, string fileName)
        {
            try
            {
                //var fs = System.IO.File.OpenRead(System.Web.HttpUtility.UrlDecode(System.Configuration.ConfigurationManager.AppSettings[path + "_upload_attachment"] + fileName));
                var fs = System.IO.File.OpenRead(System.Configuration.ConfigurationManager.AppSettings[path + "_upload_attachment"] + fileName);

                // this is needed for IE to save the file instead of opening it
                HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileName);

                return File(fs, "application/octet-stream");
            }
            catch (System.Exception)
            {
                return null;
            }

        }

        [HttpGet]
        public FileResult DownloadReturnAttachment(string path, string fileName)
        {
            try
            {
                var fs = System.IO.File.OpenRead(System.Configuration.ConfigurationManager.AppSettings[path + "_upload_returnAttachment"] + fileName);

                // this is needed for IE to save the file instead of opening it
                HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileName);

                return File(fs, "application/octet-stream");
            }
            catch (System.Exception)
            {
                return null;
            }

        }

    }
}