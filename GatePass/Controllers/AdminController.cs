using GatePass.Helpers;
using System;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class AdminController : Controller
    {
        private CustomHelper custom_helper = new CustomHelper();



        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Admin Access Rights Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageAccessRights()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(5, 4, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Manage Access Rights";
                ViewBag.PageHeader = "Manage Access Rights";
                ViewBag.Breadcrumbs = "Admin / Manage Access Rights";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }
        }




        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Approvers Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageApprovers()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(6, 4, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Manage Approvers";
                ViewBag.PageHeader = "Manage Approvers";
                ViewBag.Breadcrumbs = "Admin / Manage Approvers";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }
        }



        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult Maintenance()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(8, 7, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Maintenance";
                ViewBag.PageHeader = "Maintenance";
                ViewBag.Breadcrumbs = "Maintenance / Suppliers";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }

        }





        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Items Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult Items()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(10, 7, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Items";
                ViewBag.PageHeader = "Items";
                ViewBag.Breadcrumbs = "Maintenance / Items";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }

        }




        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Approvers Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult Approver()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(15, 4, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Approver";
                ViewBag.PageHeader = "Approver";
                ViewBag.Breadcrumbs = "Maintenance / Approvers";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }

        }





        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Department Approver Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult DepartmentApprover()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(18, 4, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Department Approver";
                ViewBag.PageHeader = "Department Approver";
                ViewBag.Breadcrumbs = "Maintenance / Department Approver";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }

        }



        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Approvers Types Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproverType()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(17, 7, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Approver Type";
                ViewBag.PageHeader = "Approver Type";
                ViewBag.Breadcrumbs = "Maintenance / Approver Type";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }
        }



        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : IT Approvers Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult ITApprover()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(29, 4, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "IT Approver";
                ViewBag.PageHeader = "IT Approver";
                ViewBag.Breadcrumbs = "Admin / IT Approver";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }
        }




        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Purchasing Approvers Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchasingApprover()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(30, 4, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Purchasing Approver";
                ViewBag.PageHeader = "Purchasing Approver";
                ViewBag.Breadcrumbs = "Admin / Purchasing Approver";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }
        }






        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Admin Override Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult Override()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(31, 4, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Override";
                ViewBag.PageHeader = "Override";
                ViewBag.Breadcrumbs = "Admin / Override";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }
        }




        /// <summary>
        /// author: abasolo@allegromicro.com
        /// description : Admin Guard Maintenance View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult GuardMaintenance()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(36, 7, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Guard";
                ViewBag.PageHeader = "Guard";
                ViewBag.Breadcrumbs = "Maintenance / Guard";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }

        }

        /// <summary>
        /// author: avillena@allegromicro.com
        /// description : Department View
        /// version : 1.0
        /// </summary>
        /// <returns></returns>
        public ActionResult DepartmentMaintenance()
        {
            try
            {
                ViewBag.Menu = custom_helper.PrepareMenu(38, 7, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
                ViewBag.Title = "Department";
                ViewBag.PageHeader = "Department";
                ViewBag.Breadcrumbs = "Maintenance / Department";
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Account/login.cshtml");
            }

        }
    }
}