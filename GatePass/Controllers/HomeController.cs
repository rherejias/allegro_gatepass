using GatePass.Helpers;
using GatePass.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class HomeController : Controller
    {
        private CustomHelper custom_helper = new CustomHelper();
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private readonly HomeModal homemodel = new HomeModal();
        int percent = 100;


        /// <summary>
        /// @author : avillena
        /// @description : view of landing page/dahboard
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        [CustomAuthorize]
        public ActionResult Index()
        {
            ViewBag.Menu = custom_helper.PrepareMenu(1, 0, Session["user_type"].ToString(), Int32.Parse(Session["userId_local"].ToString()));
            ViewBag.Title = "Dashboard";
            ViewBag.PageHeader = "Dashboard";
            ViewBag.Breadcrumbs = "Guard / Dashboard";

            string loginname = Session["cn"].ToString();

            string usercodeLogin = Session["user_code"].ToString();
            int isapproverCount = homemodel.GetifLoginApprover(usercodeLogin);
            ViewBag.loginuser = Session["department"].ToString();



            if (isapproverCount != 0)
            {
                ViewBag.loginuser = "Approver";
                ///get the count of pending for approval ni approver to department,IT,purchasing and accounting
                int fordepartmentpending = homemodel.GetCountForApprovalApproverDepartment(usercodeLogin);
                int forITpending = homemodel.GetCountForApprovalApproverIT(usercodeLogin);
                int forpurchasingpending = homemodel.GetCountForApprovalApproverPurchasing(usercodeLogin);
                int foraccountingpending = homemodel.GetCountForApprovalApproverAccounting(usercodeLogin);
                ///end
                ///
                ///get the count of approved gate pass ni approver to department,IT,purchasing and accounting
                int fordepartmentapproved = homemodel.GetCountApprovedbydepartment(usercodeLogin);
                int forITapproved = homemodel.GetCountApprovedbyIT(usercodeLogin);
                int forpurchasingapproved = homemodel.GetCountApprovedbypurchasing(usercodeLogin);
                int foraccountingapproved = homemodel.GetCountApprovedbyaccounting(usercodeLogin);
                ///end
                //////get the count of rejected gate pass ni approver to department,IT,purchasing and accounting
                int fordepartmentrejected = homemodel.GetCountRejecteddepartment(usercodeLogin);
                int forITrejected = homemodel.GetCountRejectedIT(usercodeLogin);
                int forpurchasingrejected = homemodel.GetCountRejectedpurchasing(usercodeLogin);
                int foraccountingrejected = homemodel.GetCountRejectedaccounting(usercodeLogin);
                ///end
                ///
                ViewBag.totalcountpendingfoapproval = Convert.ToInt32(fordepartmentpending + forITpending + forpurchasingpending + foraccountingpending);
                ViewBag.ApprovedbyApprover = Convert.ToInt32(fordepartmentapproved + forITapproved + forpurchasingapproved + foraccountingapproved);
                ViewBag.RejectedbyApprover = Convert.ToInt32(fordepartmentrejected + forITrejected + forpurchasingrejected + foraccountingrejected);
                ViewBag.ApproverTransaction = Convert.ToInt32(ViewBag.totalcountpendingfoapproval + ViewBag.ApprovedbyApprover + ViewBag.RejectedbyApprover);

                if (ViewBag.totalcountpendingfoapproval != 0)
                {
                    double percentagepending = (((double) ViewBag.totalcountpendingfoapproval / (double) ViewBag.ApproverTransaction) * percent);
                    ViewBag.totalcountpendingfoapprovalpercentage = Convert.ToInt32(percentagepending);
                }
                else
                {
                    ViewBag.totalcountpendingfoapprovalpercentage = 0;
                }

                if (ViewBag.ApprovedbyApprover != 0)
                {
                    double percentagapproved = (((double) ViewBag.ApprovedbyApprover / (double) ViewBag.ApproverTransaction) * percent);
                    ViewBag.ApprovedbyApproverpercentage = Convert.ToInt32(percentagapproved);
                }
                else
                {
                    ViewBag.ApprovedbyApproverpercentage = 0;
                }
                if (ViewBag.RejectedbyApprover != 0)
                {
                    double percentagapproved = (((double) ViewBag.RejectedbyApprover / (double) ViewBag.ApproverTransaction) * percent);
                    ViewBag.RejectedbyApproverpercentage = Convert.ToInt32(percentagapproved);
                }
                else
                {
                    ViewBag.RejectedbyApproverpercentage = 0;
                }


            }
            else if (ViewBag.loginuser == "Guard")
            {
                ViewBag.loginuser = "Security";
                ViewBag.ForApprovalGuard = Convert.ToInt32(homemodel.GetCountForApprovalGuard());
                ViewBag.ApprovedGuard = Convert.ToInt32(homemodel.GetCountApprovedGuard());
                ViewBag.ReturnSlipGuard = Convert.ToInt32(homemodel.GetCountReturnSlipGuard());
                ViewBag.RejectedGuard = Convert.ToInt32(homemodel.GetCountRejectedGuard());

            }
            else
            {
                ViewBag.loginuser = "User";
                int addedby = Convert.ToInt32(Session["userId_local"]);
                ViewBag.Submitted = Convert.ToInt32(homemodel.GetCountAllSubmitted(addedby));
                ViewBag.Approved = Convert.ToInt32(homemodel.GetCountAllApproved(addedby));
                ViewBag.Rejected = Convert.ToInt32(homemodel.GetCountAllRejected(addedby));
                ViewBag.Drafted = Convert.ToInt32(homemodel.GetCountAllDrafted(addedby));
                ViewBag.TotalGatePass = Convert.ToInt32(homemodel.GetCountAllTotal(addedby));

                int noData = 0;
                if (ViewBag.TotalGatePass == 0)
                {
                    ViewBag.SubmittedPercentage = noData;
                    ViewBag.ApprovedPercentage = noData;
                    ViewBag.RejectedPercentage = noData;
                    ViewBag.DraftedPercentage = noData;
                }
                else
                {

                    double percentagesubmitted = (((double) ViewBag.Submitted / (double) ViewBag.TotalGatePass) * percent);
                    double percentageapproved = (((double) ViewBag.Approved / (double) ViewBag.TotalGatePass) * percent);
                    double percentagerejected = (((double) ViewBag.Rejected / (double) ViewBag.TotalGatePass) * percent);
                    double percentagedrafted = (((double) ViewBag.Drafted / (double) ViewBag.TotalGatePass) * percent);

                    ViewBag.SubmittedPercentage = Convert.ToInt32(percentagesubmitted);
                    ViewBag.ApprovedPercentage = Convert.ToInt32(percentageapproved);
                    ViewBag.RejectedPercentage = Convert.ToInt32(percentagerejected);
                    ViewBag.DraftedPercentage = Convert.ToInt32(percentagedrafted);
                }
            }





            return View();
        }

        public JsonResult Foo()
        {
            try
            {
                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", BuildChart());
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
         * @author      :   AC <aabasolo@ALLEGROMICRO.com>
         * @date        :   NOV. 28 2016 12:39 PM
         * @description :   prepare chart config for building the chart
         * @parameters  :
         *                  
         * @return      :   string object
         */
        #region BuildChart
        public string BuildChart()
        {
            Dictionary<string, object> chart_params = new Dictionary<string, object>();
            Dictionary<string, object> chartSeries = new Dictionary<string, object>();
            Dictionary<string, object> dataLabels = new Dictionary<string, object>();

            string x = "";
            string categories_str = "GP001,GP002,GP003,GP004,GP005";
            String[] categories_arr = categories_str.Split(',');
            chartSeries.Add("name", "[employee_name]");
            chartSeries.Add("data", new[] { 5, 7, -3, 2, -5 });

            dataLabels.Add("enabled", true);
            dataLabels.Add("format", "{point.y:.1f}%");

            chart_params.Add("legend", new Dictionary<string, object>() { { "enabled", false } });
            chart_params.Add("chart", new Dictionary<string, string>() { { "type", "column" } });
            chart_params.Add("title", new Dictionary<string, string>() { { "text", string.Empty } });
            chart_params.Add("xAxis", new Dictionary<string, object>() { { "categories", categories_arr } });
            chart_params.Add("series", new[] { chartSeries });
            x = custom_helper.BuildChart(chart_params, "chartid");

            return x;
        }
        #endregion

        /*
         * @author      :   AC <aabasolo@ALLEGROMICRO.com>
         * @date        :   NOV. 28 2016 4:21 PM
         * @description :   get statistics
         * @parameters  :
         *                  
         * @return      :   JSON object
         */
        #region GetStatistics
        public JsonResult GetStatistics()
        {
            try
            {
                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", BuildChart());
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}