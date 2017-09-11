using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class FooController : Controller
    {
        Library.EncryptDecrypt.EncryptDecryptPassword enc = new Library.EncryptDecrypt.EncryptDecryptPassword();
        // GET: Foo
        public JsonResult asdf()
        {
            string encrypted_url = enc.EncryptPassword("20170200001-20170100001");

            return Json(encrypted_url, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult bar()
        {
            string decrypted_val = enc.DecryptPassword(Request["x"].ToString());

            string[] aa = decrypted_val.Split('-');

            return Json(aa, JsonRequestBehavior.AllowGet);
        }
    }
}