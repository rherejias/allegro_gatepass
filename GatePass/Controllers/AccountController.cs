using GatePass.Models;
using GatePass.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace GatePass.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserModels userModel = new UserModels();
        private readonly Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly Dictionary<string, object> r = new Dictionary<string, object>();





        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : user log in view
        /// version : 1.0
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }





        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : user log in and validation , get the log in details to AD server
        /// version : 1.0
        /// updated by: avillena@allegromicro.com | desc: addiotin of try catch to validate the thumbnail photo | date : 4/20/17 | ver. : 2.0
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Attempt(string username, string password, string returnurl, bool isauto)
        {
            try
            {
                string userType = "";
                if (ModelState.IsValid)
                {
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ api login
                    var e = new Library.EncryptDecrypt.EncryptDecryptPassword();

                    var hndlr = new HttpClientHandler();
                    hndlr.UseDefaultCredentials = true;

                    var client = new HttpClient(hndlr);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));

                    string pw = password;

                    if (!isauto)
                    {
                        pw = Server.UrlEncode(e.EncryptPassword(password));
                    }

                    string url = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_api_base_url"].ToString() + "login/Authenticate?username=" + username + "&password=" + pw + "&json=true";
                    HttpResponseMessage res = client.GetAsync(url).Result;

                    if (res.IsSuccessStatusCode)
                    {
                        string strJson = res.Content.ReadAsStringAsync().Result;
                        dynamic jObj = (JObject) JsonConvert.DeserializeObject(strJson);

                        var j = new JavaScriptSerializer();
                        object a = j.Deserialize(strJson, typeof(object));

                        Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);

                        if (bool.Parse(dict["success"].ToString()))
                        {
                            //url for getting user info from AD
                            url = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_api_base_url"].ToString() + "login/userinfo?username=" + username + "&json=true";
                            res = new HttpResponseMessage();
                            res = client.GetAsync(url).Result;
                            if (res.IsSuccessStatusCode)
                            {
                                strJson = res.Content.ReadAsStringAsync().Result;
                                jObj = (JObject) JsonConvert.DeserializeObject(strJson);
                                a = j.Deserialize(strJson, typeof(object));
                                dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);


                                var userObject = new ADUserObject();

                                //userObject.Username = dict["sAMAccountName"].ToString();
                                //userObject.LastName = dict["sn"].ToString();
                                //userObject.GivenName = dict["givenName"].ToString();
                                //userObject.EmployeeNbr = dict["employeeNumber"].ToString();
                                //userObject.Email = dict["mail"].ToString();                               
                                //userObject.Department = dict["department"].ToString();

                                try { userObject.Username = dict["sAMAccountName"].ToString(); } catch { userObject.Username = ""; }
                                try { userObject.LastName = dict["sn"].ToString(); } catch { userObject.LastName = ""; }
                                try { userObject.GivenName = dict["givenName"].ToString(); } catch { userObject.GivenName = ""; }
                                try { userObject.EmployeeNbr = dict["employeeNumber"].ToString(); } catch { userObject.EmployeeNbr = ""; }
                                try { userObject.Email = dict["mail"].ToString(); } catch { userObject.Email = ""; }
                                try { userObject.Department = dict["department"].ToString(); } catch { userObject.Department = ""; }
                                try { userObject.ThumbnailPhoto = dict["thumbnailPhoto"].ToString(); } catch { userObject.ThumbnailPhoto = ""; }

                                userObject.IsActive = true;
                                userObject.AddedBy = 0;
                                userObject.DateAdded = DateTime.Now;
                                userObject.Source = "AD";




                                int isInLocal = userModel.CheckIdFromLocal(dict["mail"].ToString());

                                if (isInLocal <= 0)
                                {
                                    userObject.Type = "user";
                                    isInLocal = userModel.AddUser(userObject);
                                    userType = userModel.GetUserDetail(isInLocal, col: "Type");
                                }
                                else
                                {
                                    userType = userModel.GetUserDetail(isInLocal, col: "Type");

                                    userObject.Id = isInLocal;
                                    userObject.Type = userType;

                                    isInLocal = userModel.UpdateUser(userObject);
                                }

                                string userCode = userModel.GetUserDetail(isInLocal, col: "Code");

                                HttpContext.Session.Add(name: "loggedIn", value: true);
                                HttpContext.Session.Add(name: "userId_local", value: isInLocal);
                                HttpContext.Session.Add(name: "user_code", value: userCode);
                                HttpContext.Session.Add(name: "user_type", value: userType);

                                //HttpContext.Session.Add(name: "cn", value: dict["cn"]);
                                //HttpContext.Session.Add(name: "title", value: dict["title"]);
                                //HttpContext.Session.Add(name: "department", value: dict["department"]);
                                //HttpContext.Session.Add(name: "company", value: dict["company"]);
                                //HttpContext.Session.Add(name: "employeeNumber", value: dict["employeeNumber"]);
                                //HttpContext.Session.Add(name: "mail", value: dict["mail"]);

                                /// try catch for checking of thumbnail photo of the user
                                try { HttpContext.Session.Add(name: "cn", value: dict["cn"]); } catch { HttpContext.Session.Add(name: "cn", value: ""); }
                                try { HttpContext.Session.Add(name: "title", value: dict["title"]); } catch { HttpContext.Session.Add(name: "title", value: ""); }
                                try { HttpContext.Session.Add(name: "department", value: dict["department"]); } catch { HttpContext.Session.Add(name: "department", value: ""); }
                                try { HttpContext.Session.Add(name: "company", value: dict["company"]); } catch { HttpContext.Session.Add(name: "company", value: ""); }
                                try { HttpContext.Session.Add(name: "employeeNumber", value: dict["employeeNumber"]); } catch { HttpContext.Session.Add(name: "employeeNumber", value: ""); }
                                try { HttpContext.Session.Add(name: "mail", value: dict["mail"]); } catch { HttpContext.Session.Add(name: "mail", value: ""); }
                                try { HttpContext.Session.Add(name: "thumbnailPhoto", value: dict["thumbnailPhoto"]); } catch { HttpContext.Session.Add(name: "thumbnailPhoto", value: ""); }



                                HttpContext.Session.Add(name: "username", value: username);

                                FormsAuthentication.SetAuthCookie(username, true);

                                //set account credentials into browser cookie
                                var logInCookie = new HttpCookie(name: "logInCookie");
                                logInCookie.Values["UName"] = username;
                                logInCookie.Values["PWord"] = pw;
                                logInCookie.Values["lastVisit"] = DateTime.Now.ToString();
                                logInCookie.Expires = DateTime.Now.AddDays(value: 30);
                                Response.Cookies.Add(logInCookie);

                                //return Redirect(returnUrl);
                                if (returnurl == "" || returnurl == null || returnurl == "/")
                                {
                                    response.Add(key: "message", value: ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_base_url"].ToString());
                                }
                                else
                                {
                                    response.Add(key: "message", value: ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_base_url"].ToString() + returnurl);
                                }
                                response.Add(key: "success", value: true);
                                response.Add(key: "error", value: false);
                            }
                            else
                            {
                                throw new Exception("Page return: " + res.IsSuccessStatusCode);
                            }
                        }
                        else
                        {
                            //throw new Exception(dict["message"].ToString());
                            // check for local db

                            DataTable dtLocal = userModel.AuthenticateUserToLocalDB(username, password);

                            if (dtLocal.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtLocal.Rows)
                                {
                                    if (bool.Parse(row["IsActive"].ToString()))
                                    {
                                        HttpContext.Session.Add(name: "loggedIn", value: true);
                                        HttpContext.Session.Add(name: "userId_local", value: row["Id"]);
                                        HttpContext.Session.Add(name: "user_code", value: row["Code"]);
                                        HttpContext.Session.Add(name: "user_type", value: row["Type"]);
                                        HttpContext.Session.Add(name: "cn", value: row["LastName"] + ", " + row["GivenName"]);
                                        HttpContext.Session.Add(name: "title", value: "Guard");
                                        HttpContext.Session.Add(name: "department", value: row["Department"]);
                                        HttpContext.Session.Add(name: "company", value: "AMPI");
                                        HttpContext.Session.Add(name: "employeeNumber", value: 0);
                                        HttpContext.Session.Add(name: "mail", value: "");
                                        HttpContext.Session.Add(name: "thumbnailPhoto", value: "");

                                        FormsAuthentication.SetAuthCookie(username, true);

                                        if (returnurl == "" || returnurl == null || returnurl == "/")
                                        {
                                            response.Add(key: "message", value: ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_base_url"].ToString());
                                        }
                                        else
                                        {
                                            response.Add(key: "message", value: ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_base_url"].ToString() + returnurl);
                                        }
                                        response.Add(key: "success", value: true);
                                        response.Add(key: "error", value: false);
                                    }
                                    else
                                    {
                                        throw new Exception(message: "Your account is currently inactive. Please contact your system administrator.");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception(message: "User does not exist on the local database.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Page return: " + res.IsSuccessStatusCode);
                    }
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ api login
                }
                else
                {
                    //ModelState.AddModelError("", "Log In Failed");
                    throw new Exception(message: "ModelState is invalid!");
                }
            }
            catch (Exception e)
            {
                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                if (e.ToString().IndexOf(value: "The supplied credential is invalid") != -1)
                {
                    response.Add(key: "message", value: "Username and/or password is incorrect!");
                    var logInCookie = new HttpCookie(name: "logInCookie");
                    logInCookie.Expires = DateTime.Now.AddDays(-1);
                }
                else
                {
                    var logInCookie = new HttpCookie(name: "logInCookie");
                    response.Add(key: "message", value: "Username and/or password is incorrect!");
                    // return Redirect(returnUrl);
                    logInCookie.Expires = DateTime.Now.AddDays(-1);


                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : user log out
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public JsonResult Logout()
        {
            Session.Remove(name: "userId_local");
            Session.Remove(name: "cn");
            Session.Remove(name: "title");
            Session.Remove(name: "department");
            Session.Remove(name: "company");
            Session.Remove(name: "employeeNumber");
            Session.Remove(name: "mail");
            Session.Remove(name: "thumbnailPhoto");
            Session.Remove(name: "loggedIn");
            response.Add(key: "success", value: true);
            response.Add(key: "error", value: false);
            response.Add(key: "message", value: "");
            var loginCookie = new HttpCookie(name: "loginCookie");
            loginCookie.Values["UName"] = string.Empty;
            loginCookie.Values["PWord"] = string.Empty;
            loginCookie.Values["lastVisit"] = DateTime.Now.ToString();
            loginCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(loginCookie);
            return Json(response, JsonRequestBehavior.AllowGet);
        }






        /// <summary>
        /// author: avillena@allegromicro.com
        /// description : validate add new user to local tbluser
        /// Update by : avillena@allegromicro.com | Date : 04/20/17
        /// version : 1.0
        /// updated by: avillena@allegromicro.com | desc: addiotin of try catch to validate the thumbnail photo | date : 4/20/17 | ver. : 2.0
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Validate()

        {
            try
            {
                string username = Request["username"].ToString();
                string adduser = Request["adduser"].ToString();
                var hndlr = new HttpClientHandler();
                hndlr.UseDefaultCredentials = true;

                HttpClient client = new HttpClient(hndlr);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string url;
                url = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["env"].ToString() + "_api_base_url"].ToString() + "login/userinfo?username=" + username + "&json=true";
                HttpResponseMessage res = client.GetAsync(url).Result;

                if (res.IsSuccessStatusCode)
                {
                    string strJson = res.Content.ReadAsStringAsync().Result;
                    dynamic jObj = (JObject) JsonConvert.DeserializeObject(strJson);

                    JavaScriptSerializer j = new JavaScriptSerializer();
                    object a = j.Deserialize(strJson, typeof(object));

                    Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);

                    if (dict.Count > 0)
                    {
                        //url for getting user info from AD

                        res = new HttpResponseMessage();
                        res = client.GetAsync(url).Result;
                        if (res.IsSuccessStatusCode)
                        {
                            strJson = res.Content.ReadAsStringAsync().Result;
                            jObj = (JObject) JsonConvert.DeserializeObject(strJson);
                            a = j.Deserialize(strJson, typeof(object));
                            dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);


                            ADUserObject userObject = new ADUserObject();
                            userObject.Username = dict["sAMAccountName"].ToString();
                            userObject.LastName = dict["sn"].ToString();
                            userObject.GivenName = dict["givenName"].ToString();
                            userObject.EmployeeNbr = dict["employeeNumber"].ToString();
                            userObject.Email = dict["mail"].ToString();
                            userObject.IsActive = true;
                            userObject.Department = dict["department"].ToString();
                            userObject.AddedBy = 0;
                            userObject.DateAdded = DateTime.Now;
                            userObject.Source = "AD";
                            userObject.Type = "user";

                            //try catch for validating of thumbnail photo of user
                            try
                            {
                                userObject.ThumbnailPhoto = dict["thumbnailPhoto"].ToString();
                            }
                            catch
                            {
                                userObject.ThumbnailPhoto = "";
                            }


                            if (adduser == "YES")
                            {

                                int usernameCount = userModel.Username_count(userObject.Username);

                                if (usernameCount == 0)
                                {
                                    userModel.AddUser(userObject);
                                    response.Add(key: "success", value: true);
                                    response.Add("error", false);
                                    response.Add("message", "Added succesfully");

                                    return Json(response, JsonRequestBehavior.AllowGet);

                                }
                                else
                                {

                                    response.Add("success", false);
                                    response.Add("error", true);
                                    response.Add("message", "Username has already been added!");

                                    return Json(response, JsonRequestBehavior.AllowGet);
                                }


                            }


                            response.Add("success", true);
                            response.Add("error", false);
                            response.Add("message", userObject);

                        }



                    }
                    else
                    {

                        throw new Exception("Invalid Username!");

                    }



                }


            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", "Invalid Username!");

            }


            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}