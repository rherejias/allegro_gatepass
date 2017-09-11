using GatePass.Helpers;
using GatePass.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace GatePass.Controllers
{
    public class ItemsController : Controller
    {
        private CustomHelper custom_helper = new CustomHelper();
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private ItemModels items_model = new ItemModels();
        private ItemMasterObject itemObj = new ItemMasterObject();
        private CategoryObject CatObj = new CategoryObject();
        private ItemTypeObject TypeObj = new ItemTypeObject();
        private UnitMeasureObject MeasureObj = new UnitMeasureObject();
        private ItemDepartmentObject DepartmentObj = new ItemDepartmentObject();




        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get the item dropdownlist
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for item type e.g. returnable, non-returnable
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetItems()
        {

            //@ver 1.0 rherejias 2/1/2017 used for where clause IsActive
            string IsActive;

            IsActive = Request["IsActive"].ToString();
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = items_model.GetItem_cols();

            //check for filters
            string where = "";
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
            {
                for (int i = 0; i < Int32.Parse(Request["filterscount"]); i++)
                {
                    filters.Add("filtervalue" + i, Request["filtervalue" + i]);
                    filters.Add("filtercondition" + i, Request["filtercondition" + i]);
                    filters.Add("filterdatafield" + i, Request["filterdatafield" + i]);
                    filters.Add("filteroperator" + i, Request["filteroperator" + i]);
                }
                where = custom_helper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
            }

            //check for sorting ops
            string sorting = "";
            if (Request["sortdatafield"] != null)
            {
                sorting = Request["sortdatafield"].ToString() + " " + Request["sortorder"].ToString().ToUpper();
            }

            //determine if cols_only
            if (Request["cols_only"] != null && bool.Parse(Request["cols_only"]) == true)
            {
                //get total row count
                int totalRows = items_model.GetItem_count(where, IsActive);

                //prepare column config
                var cols = new List<string>();
                foreach (var item in columns)
                {
                    cols.Add(item.Value);
                }

                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", totalRows);
            }
            else
            {
                //pagination initialization
                int pagenum = Request["pagenum"] == null ? 0 : Int32.Parse(Request["pagenum"].ToString());
                int pagesize = Request["pagesize"] == null ? 0 : Int32.Parse(Request["pagesize"].ToString());
                int start = pagenum * pagesize;

                //get data
                DataTable transactions = new DataTable();
                if (Request["showAll"] != null && bool.Parse(Request["showAll"]) == true)
                {
                    transactions = items_model.GetItem(0, 0, where, sorting, IsActive);
                }
                else
                {
                    transactions = items_model.GetItem(start, pagesize, where, sorting, IsActive);
                }

                //convert data into json object
                var data = custom_helper.DataTableToJson(transactions);

                result_config.Add("data", data);
                //prepare column config
                var cols = new List<string>();
                foreach (DataColumn column in transactions.Columns)
                {
                    cols.Add(column.ColumnName);
                }
                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", items_model.GetItem_count(where, IsActive));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion




        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get the unit of measure dropdownlist
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for unit of measure
        [HttpGet]
        public JsonResult GetUnitOfMeasures()
        {
            //@ver 1.0 rherejias 2/1/2017 used for where clause IsActive
            string IsActive;

            IsActive = Request["IsActive"].ToString();

            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = items_model.GetUnitOfMeasure_cols();

            //check for filters
            string where = "";
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
            {
                for (int i = 0; i < Int32.Parse(Request["filterscount"]); i++)
                {
                    filters.Add("filtervalue" + i, Request["filtervalue" + i]);
                    filters.Add("filtercondition" + i, Request["filtercondition" + i]);
                    filters.Add("filterdatafield" + i, Request["filterdatafield" + i]);
                    filters.Add("filteroperator" + i, Request["filteroperator" + i]);
                }
                where = custom_helper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
            }

            //check for sorting ops
            string sorting = "";
            if (Request["sortdatafield"] != null)
            {
                sorting = Request["sortdatafield"].ToString() + " " + Request["sortorder"].ToString().ToUpper();
            }

            //determine if cols_only
            if (Request["cols_only"] != null && bool.Parse(Request["cols_only"]) == true)
            {
                //get total row count
                int totalRows = items_model.GetUnitOfMeasure_count(where, IsActive);

                //prepare column config
                var cols = new List<string>();
                foreach (var item in columns)
                {
                    cols.Add(item.Value);
                }

                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", totalRows);
            }
            else
            {
                //pagination initialization
                int pagenum = Request["pagenum"] == null ? 0 : Int32.Parse(Request["pagenum"].ToString());
                int pagesize = Request["pagesize"] == null ? 0 : Int32.Parse(Request["pagesize"].ToString());
                int start = pagenum * pagesize;

                //get data
                DataTable transactions = new DataTable();
                if (Request["showAll"] != null && bool.Parse(Request["showAll"]) == true)
                {
                    transactions = items_model.GetUnitOfMeasure(0, 0, where, sorting, IsActive);
                }
                else
                {
                    transactions = items_model.GetUnitOfMeasure(start, pagesize, where, sorting, IsActive);
                }

                //convert data into json object
                var data = custom_helper.DataTableToJson(transactions);

                result_config.Add("data", data);
                //prepare column config
                var cols = new List<string>();
                foreach (DataColumn column in transactions.Columns)
                {
                    cols.Add(column.ColumnName);
                }
                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", items_model.GetUnitOfMeasure_count(where, IsActive));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get the category dropdownlist
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for item category e.g. office equipment
        [HttpGet]
        public JsonResult GetItemCategories()
        {
            try
            {
                //@ver 1.0 rherejias 2/1/2017 used for where clause IsActive


                string IsActive = Request["IsActive"].ToString();

                //create a dictionary that will contain the column + datafied config for the grid
                Dictionary<string, object> result_config = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = items_model.GetItemCategory_cols();

                //check for filters
                string where = "";
                Dictionary<string, string> filters = new Dictionary<string, string>();
                if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
                {
                    for (int i = 0; i < Int32.Parse(Request["filterscount"]); i++)
                    {
                        filters.Add("filtervalue" + i, Request["filtervalue" + i]);
                        filters.Add("filtercondition" + i, Request["filtercondition" + i]);
                        filters.Add("filterdatafield" + i, Request["filterdatafield" + i]);
                        filters.Add("filteroperator" + i, Request["filteroperator" + i]);
                    }
                    where = custom_helper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
                }

                //check for sorting ops
                string sorting = "";
                if (Request["sortdatafield"] != null)
                {
                    sorting = Request["sortdatafield"].ToString() + " " + Request["sortorder"].ToString().ToUpper();
                }

                //determine if cols_only
                if (Request["cols_only"] != null && bool.Parse(Request["cols_only"]) == true)
                {
                    //get total row count
                    int totalRows = items_model.GetItemCategory_count(where, IsActive);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (var item in columns)
                    {
                        cols.Add(item.Value);
                    }

                    Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                    result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                    result_config.Add("TotalRows", totalRows);
                }
                else
                {
                    //pagination initialization
                    int pagenum = Request["pagenum"] == null ? 0 : Int32.Parse(Request["pagenum"].ToString());
                    int pagesize = Request["pagesize"] == null ? 0 : Int32.Parse(Request["pagesize"].ToString());
                    int start = pagenum * pagesize;

                    //get data
                    DataTable transactions = new DataTable();
                    if (Request["showAll"] != null && bool.Parse(Request["showAll"]) == true)
                    {
                        transactions = items_model.GetItemCategory(0, 0, where, sorting, IsActive);
                    }
                    else
                    {
                        transactions = items_model.GetItemCategory(start, pagesize, where, sorting, IsActive);
                    }

                    //convert data into json object
                    var data = custom_helper.DataTableToJson(transactions);

                    result_config.Add("data", data);
                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                    result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                    result_config.Add("TotalRows", items_model.GetItemCategory_count(where, IsActive));
                }

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", result_config);
            }
            catch (Exception e)
            {

                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }



            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get the category dropdownlist
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for item category e.g. office equipment
        [HttpGet]
        public JsonResult GetItemCategories1()
        {
            try
            {
                //@ver 1.0 rherejias 2/1/2017 used for where clause IsActive


                string IsActive = Request["IsActive"].ToString();

                //create a dictionary that will contain the column + datafied config for the grid
                Dictionary<string, object> result_config = new Dictionary<string, object>();

                //get columns
                Dictionary<string, string> columns = items_model.GetItemCategory_cols();

                //check for filters
                string where = "";
                Dictionary<string, string> filters = new Dictionary<string, string>();
                if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
                {
                    for (int i = 0; i < Int32.Parse(Request["filterscount"]); i++)
                    {
                        filters.Add("filtervalue" + i, Request["filtervalue" + i]);
                        filters.Add("filtercondition" + i, Request["filtercondition" + i]);
                        filters.Add("filterdatafield" + i, Request["filterdatafield" + i]);
                        filters.Add("filteroperator" + i, Request["filteroperator" + i]);
                    }
                    where = custom_helper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
                }

                //check for sorting ops
                string sorting = "";
                if (Request["sortdatafield"] != null)
                {
                    sorting = Request["sortdatafield"].ToString() + " " + Request["sortorder"].ToString().ToUpper();
                }

                //determine if cols_only
                if (Request["cols_only"] != null && bool.Parse(Request["cols_only"]) == true)
                {
                    //get total row count
                    int totalRows = items_model.GetItemCategory_count(where, IsActive);

                    //prepare column config
                    var cols = new List<string>();
                    foreach (var item in columns)
                    {
                        cols.Add(item.Value);
                    }

                    Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                    result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                    result_config.Add("TotalRows", totalRows);
                }
                else
                {
                    //pagination initialization
                    int pagenum = Request["pagenum"] == null ? 0 : Int32.Parse(Request["pagenum"].ToString());
                    int pagesize = Request["pagesize"] == null ? 0 : Int32.Parse(Request["pagesize"].ToString());
                    int start = pagenum * pagesize;

                    //get data
                    DataTable transactions = new DataTable();
                    if (Request["showAll"] != null && bool.Parse(Request["showAll"]) == true)
                    {
                        transactions = items_model.GetItemCategory(0, 0, where, sorting, IsActive);
                    }
                    else
                    {
                        transactions = items_model.GetItemCategory(start, pagesize, where, sorting, IsActive);
                    }

                    //convert data into json object
                    var data = custom_helper.DataTableToJson(transactions);

                    result_config.Add("data", data);
                    //prepare column config
                    var cols = new List<string>();
                    foreach (DataColumn column in transactions.Columns)
                    {
                        cols.Add(column.ColumnName);
                    }
                    Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                    result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                    result_config.Add("TotalRows", items_model.GetItemCategory_count(where, IsActive));
                }

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", result_config);
            }
            catch (Exception e)
            {

                response.Add(key: "success", value: false);
                response.Add(key: "error", value: true);
                response.Add(key: "message", value: e.ToString());
            }



            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get the item type dropdownlist
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for item type e.g. returnable, non-returnable
        [HttpGet]
        public JsonResult GetItemTypes()
        {
            //@ver 1.0 rherejias 2/1/2017 used for where clause IsActive
            string IsActive;

            IsActive = Request["IsActive"].ToString();
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = items_model.GetItemType_cols();

            //check for filters
            string where = "";
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
            {
                for (int i = 0; i < Int32.Parse(Request["filterscount"]); i++)
                {
                    filters.Add("filtervalue" + i, Request["filtervalue" + i]);
                    filters.Add("filtercondition" + i, Request["filtercondition" + i]);
                    filters.Add("filterdatafield" + i, Request["filterdatafield" + i]);
                    filters.Add("filteroperator" + i, Request["filteroperator" + i]);
                }
                where = custom_helper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
            }

            //check for sorting ops
            string sorting = "";
            if (Request["sortdatafield"] != null)
            {
                sorting = Request["sortdatafield"].ToString() + " " + Request["sortorder"].ToString().ToUpper();
            }

            //determine if cols_only
            if (Request["cols_only"] != null && bool.Parse(Request["cols_only"]) == true)
            {
                //get total row count
                int totalRows = items_model.GetItemType_count(where, IsActive);

                //prepare column config
                var cols = new List<string>();
                foreach (var item in columns)
                {
                    cols.Add(item.Value);
                }

                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", totalRows);
            }
            else
            {
                //pagination initialization
                int pagenum = Request["pagenum"] == null ? 0 : Int32.Parse(Request["pagenum"].ToString());
                int pagesize = Request["pagesize"] == null ? 0 : Int32.Parse(Request["pagesize"].ToString());
                int start = pagenum * pagesize;

                //get data
                DataTable transactions = new DataTable();
                if (Request["showAll"] != null && bool.Parse(Request["showAll"]) == true)
                {
                    transactions = items_model.GetItemType(0, 0, where, sorting, IsActive);
                }
                else
                {
                    transactions = items_model.GetItemType(start, pagesize, where, sorting, IsActive);
                }

                //convert data into json object
                var data = custom_helper.DataTableToJson(transactions);

                result_config.Add("data", data);
                //prepare column config
                var cols = new List<string>();
                foreach (DataColumn column in transactions.Columns)
                {
                    cols.Add(column.ColumnName);
                }
                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", items_model.GetItemType_count(where, IsActive));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get the department dropdownlist
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for item type department
        [HttpGet]
        public JsonResult GetItemDepartment()
        {
            //@ver 1.0 rherejias 2/1/2017 used for where clause IsActive
            string IsActive;

            IsActive = Request["IsActive"].ToString();
            //create a dictionary that will contain the column + datafied config for the grid
            Dictionary<string, object> result_config = new Dictionary<string, object>();

            //get columns
            Dictionary<string, string> columns = items_model.GetItemDepartment_cols();

            //check for filters
            string where = "";
            Dictionary<string, string> filters = new Dictionary<string, string>();
            if (Request["filterscount"] != null && Int32.Parse(Request["filterscount"]) > 0)
            {
                for (int i = 0; i < Int32.Parse(Request["filterscount"]); i++)
                {
                    filters.Add("filtervalue" + i, Request["filtervalue" + i]);
                    filters.Add("filtercondition" + i, Request["filtercondition" + i]);
                    filters.Add("filterdatafield" + i, Request["filterdatafield" + i]);
                    filters.Add("filteroperator" + i, Request["filteroperator" + i]);
                }
                where = custom_helper.FormatFilterConditions(filters, Int32.Parse(Request["filterscount"]), columns);
            }

            //check for sorting ops
            string sorting = "";
            if (Request["sortdatafield"] != null)
            {
                sorting = Request["sortdatafield"].ToString() + " " + Request["sortorder"].ToString().ToUpper();
            }

            //determine if cols_only
            if (Request["cols_only"] != null && bool.Parse(Request["cols_only"]) == true)
            {
                //get total row count
                int totalRows = items_model.GetItemDepartment_count(where, IsActive);

                //prepare column config
                var cols = new List<string>();
                foreach (var item in columns)
                {
                    cols.Add(item.Value);
                }

                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", totalRows);
            }
            else
            {
                //pagination initialization
                int pagenum = Request["pagenum"] == null ? 0 : Int32.Parse(Request["pagenum"].ToString());
                int pagesize = Request["pagesize"] == null ? 0 : Int32.Parse(Request["pagesize"].ToString());
                int start = pagenum * pagesize;

                //get data
                DataTable transactions = new DataTable();
                if (Request["showAll"] != null && bool.Parse(Request["showAll"]) == true)
                {
                    transactions = items_model.GetItemDepartment(0, 0, where, sorting, IsActive);
                }
                else
                {
                    transactions = items_model.GetItemDepartment(start, pagesize, where, sorting, IsActive);
                }

                //convert data into json object
                var data = custom_helper.DataTableToJson(transactions);

                result_config.Add("data", data);
                //prepare column config
                var cols = new List<string>();
                foreach (DataColumn column in transactions.Columns)
                {
                    cols.Add(column.ColumnName);
                }
                Dictionary<string, object> cols_arr = custom_helper.PrepareStaticColumns(cols);
                result_config.Add("column_config", custom_helper.PrepareColumns(cols_arr));
                result_config.Add("TotalRows", items_model.GetItemDepartment_count(where, IsActive));
            }

            response.Add("success", true);
            response.Add("error", false);
            response.Add("message", result_config);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : edit the item list
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for item edit
        [HttpGet]
        public JsonResult EditItems()
        {
            try
            {
                itemObj.Id = Convert.ToInt32(Request["id"]);
                itemObj.Code = Request["code"].ToString();
                itemObj.Name = Request["name"].ToString();
                itemObj.Description = Request["description"].ToString();
                itemObj.ItemCategoryCode = Request["category"].ToString();
                itemObj.ItemTypeCode = Request["type"].ToString();
                itemObj.ItemDepRelCode = Request["department"].ToString();
                itemObj.ItemUOM = Request["uom"].ToString();
                itemObj.IsActive = true;
                itemObj.DateAdded = DateTime.Now;
                itemObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                items_model.EditItems(itemObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EditCategory()
        {
            try
            {
                CatObj.Id = Convert.ToInt32(Request["id"]);
                CatObj.Code = Request["code"].ToString();
                CatObj.Name = Request["name"].ToString();
                CatObj.Description = Request["description"].ToString();
                CatObj.IsActive = true;
                CatObj.DateAdded = DateTime.Now;
                CatObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                items_model.EditCategory(CatObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EditType()
        {
            try
            {
                TypeObj.Id = Convert.ToInt32(Request["id"]);
                TypeObj.Code = Request["code"].ToString();
                TypeObj.Name = Request["name"].ToString();
                TypeObj.Description = Request["description"].ToString();
                TypeObj.IsActive = true;
                TypeObj.DateAdded = DateTime.Now;
                TypeObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                items_model.EditType(TypeObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EditMeasure()
        {
            try
            {
                MeasureObj.Id = Convert.ToInt32(Request["id"]);
                MeasureObj.Code = Request["code"].ToString();
                MeasureObj.Name = Request["name"].ToString();
                MeasureObj.Description = Request["description"].ToString();
                MeasureObj.IsActive = true;
                MeasureObj.DateAdded = DateTime.Now;
                MeasureObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                items_model.EditMeasure(MeasureObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EditDepartment()
        {
            try
            {
                DepartmentObj.Id = Convert.ToInt32(Request["id"]);
                DepartmentObj.Code = Request["code"].ToString();
                DepartmentObj.Name = Request["name"].ToString();
                DepartmentObj.Description = Request["description"].ToString();
                DepartmentObj.IsActive = true;
                DepartmentObj.DateAdded = DateTime.Now;
                DepartmentObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                items_model.EditDepartment(DepartmentObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
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



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : deactivate the item list
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for deactivate controllers
        [HttpGet]
        public JsonResult DeactivateItem()
        {
            try
            {
                itemObj.Id = Convert.ToInt32(Request["Id"]);
                itemObj.Code = Request["code"].ToString();
                itemObj.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                itemObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                itemObj.DateAdded = DateTime.Now;

                items_model.DeactivateItem(itemObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeactivateCategory()
        {
            try
            {
                CatObj.Id = Convert.ToInt32(Request["Id"]);
                CatObj.Code = Request["code"].ToString();
                CatObj.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                CatObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                CatObj.DateAdded = DateTime.Now;

                items_model.DeactivateCategory(CatObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeactivateType()
        {
            try
            {
                TypeObj.Id = Convert.ToInt32(Request["Id"]);
                TypeObj.Code = Request["code"].ToString();
                TypeObj.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                TypeObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                TypeObj.DateAdded = DateTime.Now;

                items_model.DeactivateType(TypeObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeactivateMeasure()
        {
            try
            {
                MeasureObj.Id = Convert.ToInt32(Request["Id"]);
                MeasureObj.Code = Request["code"].ToString();
                MeasureObj.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                MeasureObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                MeasureObj.DateAdded = DateTime.Now;

                items_model.DeactivateMeasure(MeasureObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeactivateDepartment()
        {
            try
            {
                DepartmentObj.Id = Convert.ToInt32(Request["Id"]);
                DepartmentObj.Code = Request["code"].ToString();
                DepartmentObj.IsActive = Convert.ToBoolean(Request["isactive"].ToString());
                DepartmentObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                DepartmentObj.DateAdded = DateTime.Now;

                items_model.DeactivateDepartment(DepartmentObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
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



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : activate the item list
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for activate controllers
        [HttpGet]
        public JsonResult ActivateItem()
        {
            try
            {
                itemObj.Id = Convert.ToInt32(Request["Id"]);
                itemObj.Code = Request["code"].ToString();
                itemObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                itemObj.DateAdded = DateTime.Now;

                items_model.ActivateItem(itemObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ActivateCategory()
        {
            try
            {
                CatObj.Id = Convert.ToInt32(Request["Id"]);
                CatObj.Code = Request["code"].ToString();
                CatObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                CatObj.DateAdded = DateTime.Now;

                items_model.ActivateCategory(CatObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ActivateType()
        {
            try
            {
                TypeObj.Id = Convert.ToInt32(Request["Id"]);
                TypeObj.Code = Request["code"].ToString();
                TypeObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                TypeObj.DateAdded = DateTime.Now;

                items_model.ActivateType(TypeObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ActivateMeasure()
        {
            try
            {
                MeasureObj.Id = Convert.ToInt32(Request["Id"]);
                MeasureObj.Code = Request["code"].ToString();
                MeasureObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                MeasureObj.DateAdded = DateTime.Now;

                items_model.ActivateMeasure(MeasureObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ActivateDepartment()
        {
            try
            {
                DepartmentObj.Id = Convert.ToInt32(Request["Id"]);
                DepartmentObj.Code = Request["code"].ToString();
                DepartmentObj.AddedBy = Convert.ToInt32(Session["userId_local"]);
                DepartmentObj.DateAdded = DateTime.Now;

                items_model.ActivateDepartment(DepartmentObj);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "");
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




        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : adding of new item list
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for adding of items
        [HttpGet]
        public JsonResult AddItems()
        {
            try
            {
                itemObj.Name = Request["name"].ToString();
                itemObj.Description = Request["description"].ToString();
                itemObj.ItemCategoryCode = Request["category"].ToString();
                itemObj.ItemTypeCode = Request["type"].ToString();
                itemObj.ItemDepRelCode = Request["department"].ToString();
                itemObj.ItemUOM = Request["uom"].ToString();
                itemObj.IsActive = true;
                itemObj.DateAdded = DateTime.Now;
                itemObj.AddedBy = Convert.ToInt32(Session["userId_local"]);


                var IsExist = items_model.AddItems(itemObj);
                DataTable itemDetails = items_model.ItemDetailsByName(Request["name"].ToString());
                if (IsExist != false)
                {
                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", custom_helper.DataTableToJson(itemDetails));
                }
                else
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", "Item already exist.");
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

        [HttpGet]
        public JsonResult AddCategory()
        {
            try
            {
                CatObj.Name = Request["name"].ToString();
                CatObj.Description = Request["description"].ToString();
                CatObj.IsActive = true;
                CatObj.DateAdded = DateTime.Now;
                CatObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                var IsExist = items_model.AddCategory(CatObj);
                if (IsExist != false)
                {
                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", "");
                }
                else
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", "Item category already exist.");
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

        [HttpGet]
        public JsonResult AddType()
        {
            try
            {
                TypeObj.Name = Request["name"].ToString();
                TypeObj.Description = Request["description"].ToString();
                TypeObj.IsActive = true;
                TypeObj.DateAdded = DateTime.Now;
                TypeObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                var IsExist = items_model.AddType(TypeObj);
                if (IsExist != false)
                {
                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", "");
                }
                else
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", "Item type already exist.");
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

        [HttpGet]
        public JsonResult AddMeasure()
        {
            try
            {
                MeasureObj.Name = Request["name"].ToString();
                MeasureObj.Description = Request["description"].ToString();
                MeasureObj.IsActive = true;
                MeasureObj.DateAdded = DateTime.Now;
                MeasureObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                var IsExist = items_model.AddMeasure(MeasureObj);
                if (IsExist != false)
                {
                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", "");
                }
                else
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", "UOM already exist.");
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

        [HttpGet]
        public JsonResult AddDepartment()
        {
            try
            {
                DepartmentObj.Name = Request["name"].ToString();
                DepartmentObj.Description = Request["description"].ToString();
                DepartmentObj.IsActive = true;
                DepartmentObj.DateAdded = DateTime.Now;
                DepartmentObj.AddedBy = Convert.ToInt32(Session["userId_local"]);

                var IsExist = items_model.AddDepartment(DepartmentObj);
                if (IsExist != false)
                {
                    response.Add("success", true);
                    response.Add("error", false);
                    response.Add("message", "");
                }
                else
                {
                    response.Add("success", false);
                    response.Add("error", true);
                    response.Add("message", "Item department already exist.");
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
        #endregion



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : get the details by item code
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region GetItemDetailsByItemCode
        [HttpGet]
        public JsonResult GetItemDetailsByItemCode()
        {
            try
            {
                string itemCode = Request["itemcode"].ToString();

                DataTable ItemDetail = new DataTable();
                ItemDetail = items_model.ItemDetails(itemCode);

                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", custom_helper.DataTableToJson(ItemDetail));
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



        /// <summary>
        /// @author rherejias 2/27/2017 
        /// @description : expor to excel file
        /// @version : 1.0
        /// </summary>
        /// <returns>json</returns>
        #region for export
        public JsonResult export()
        {
            items_model.export(Request["operation"].ToString(), Request["table"].ToString());
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}