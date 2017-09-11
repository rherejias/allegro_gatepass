using GatePass.Extensions;
using GatePass.Models;
using GatePass.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;
using System.Web.Script.Serialization;

namespace GatePass.Helpers
{
    public class CustomHelper
    {
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private UserModels userModel = new UserModels();

        #region determine environment IsProd
        public bool IsProd()
        {
            return ConfigurationManager.AppSettings["env"] == "prod";
        }
        #endregion

        #region DataTableToJson
        public Object DataTableToJson(DataTable dTable)
        {
            var json = JsonConvert.SerializeObject(dTable);
            object jsonObj = new JavaScriptSerializer().DeserializeObject(json);
            return jsonObj;
        }
        #endregion

        #region prepare static columns
        public Dictionary<string, object> PrepareStaticColumns(List<string> arr)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            Dictionary<string, object> cols_arr = new Dictionary<string, object>();

            double w = (double)100 / (double)arr.Count();
            String col_width = String.Format("{0:0.00}", w) + "%";
            string colName = "";
            for (int i = 0; i < arr.Count(); i++)
            {
                colName = arr[i].ToString();
                Dictionary<string, object> cols_config = new Dictionary<string, object>();
                cols_config.Add("label", colName.Substring(0, colName.LastIndexOf("_")).Replace("_", " ").UcWords());
                cols_config.Add("type", colName.Substring((colName.LastIndexOf("_") + 1)));
                //cols_config.Add("width", col_width);
                cols_arr.Add(arr[i], cols_config);
            }

            return cols_arr;
        }
        #endregion

        #region PrepareColumns
        public Object PrepareColumns(Dictionary<string, object> arr)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            Dictionary<string, object> column_config = new Dictionary<string, object>();
            var dataFields = new List<object>();
            var columnFields = new List<object>();

            foreach (var item in arr)
            {
                Dictionary<string, object> dataFields_config = new Dictionary<string, object>();
                Dictionary<string, object> columnFields_config = new Dictionary<string, object>();

                dataFields_config.Add("name", item.Key);
                var dictionary = (Dictionary<string, object>)item.Value;
                dataFields_config.Add("type", dictionary["type"]);
                if (dictionary["type"].ToString().ToLower() == "datetime")
                {
                    dataFields_config.Add("format", "yyyy-MM-dd HH:mm:ss");
                }
                dataFields.Add(dataFields_config);

                switch (dictionary["type"].ToString())
                {
                    case "number":
                    case "int":
                    case "float":
                        {
                            columnFields_config.Add("text", dictionary["label"].ToString());
                            columnFields_config.Add("datafield", item.Key);
                            columnFields_config.Add("cellsalign", "right");
                            columnFields_config.Add("filtertype", "input");

                            if (dictionary.ContainsKey("width"))
                            {
                                columnFields_config.Add("width", dictionary["width"].ToString());
                            }

                            columnFields_config.Add("filterable", true);
                            columnFields_config.Add("sortable", true);
                            columnFields_config.Add("cellclassname", string.Empty);
                            columnFields_config.Add("aggregates", new[] { "sum" });
                            columnFields_config.Add("cellsformat", "d0");
                            columnFields_config.Add("editable", false);
                            columnFields.Add(columnFields_config);
                            break;
                        }
                    case "date":
                        {
                            columnFields_config.Add("text", dictionary["label"].ToString());
                            columnFields_config.Add("datafield", item.Key);
                            columnFields_config.Add("cellsalign", "left");
                            columnFields_config.Add("filtertype", "date");

                            if (dictionary.ContainsKey("width"))
                            {
                                columnFields_config.Add("width", dictionary["width"].ToString());
                            }

                            columnFields_config.Add("filterable", true);
                            columnFields_config.Add("sortable", true);
                            columnFields_config.Add("cellclassname", string.Empty);
                            columnFields_config.Add("cellsformat", "yyyy-MM-dd");
                            columnFields_config.Add("editable", false);
                            columnFields.Add(columnFields_config);
                            break;
                        }
                    case "datetime":
                        {
                            columnFields_config.Add("text", dictionary["label"].ToString());
                            columnFields_config.Add("datafield", item.Key);
                            columnFields_config.Add("cellsalign", "left");
                            columnFields_config.Add("filtertype", "date");

                            if (dictionary.ContainsKey("width"))
                            {
                                columnFields_config.Add("width", dictionary["width"].ToString());
                            }

                            columnFields_config.Add("filterable", true);
                            columnFields_config.Add("sortable", true);
                            columnFields_config.Add("cellclassname", string.Empty);
                            columnFields_config.Add("cellsformat", "yyyy-MM-dd HH:mm:ss");
                            columnFields_config.Add("editable", false);
                            columnFields.Add(columnFields_config);
                            break;
                        }
                    case "link":
                        {
                            columnFields_config.Add("text", dictionary["label"].ToString());
                            columnFields_config.Add("datafield", item.Key);
                            columnFields_config.Add("cellsalign", "left");
                            columnFields_config.Add("filtertype", "input");

                            if (dictionary.ContainsKey("width"))
                            {
                                columnFields_config.Add("width", dictionary["width"].ToString());
                            }

                            columnFields_config.Add("filterable", true);
                            columnFields_config.Add("sortable", true);
                            columnFields_config.Add("cellclassname", string.Empty);
                            columnFields_config.Add("cellsrenderer", "link");
                            columnFields_config.Add("editable", false);
                            columnFields.Add(columnFields_config);
                            break;
                        }
                    //case "Image":
                    //    {
                    //        columnFields_config.Add("text", dictionary["label"].ToString());
                    //        columnFields_config.Add("datafield", item.Key);
                    //        columnFields_config.Add("cellsalign", "left");
                    //        columnFields_config.Add("filtertype", "input");

                    //        if (dictionary.ContainsKey("width"))
                    //        {
                    //            columnFields_config.Add("width", dictionary["width"].ToString());
                    //        }

                    //        columnFields_config.Add("filterable", true);
                    //        columnFields_config.Add("sortable", true);
                    //        columnFields_config.Add("cellclassname", string.Empty);
                    //        columnFields_config.Add("cellsrenderer", "imagerenderer");
                    //        columnFields.Add(columnFields_config);
                    //        break;
                    //    }
                    case "list":
                        {
                            columnFields_config.Add("text", dictionary["label"].ToString());
                            columnFields_config.Add("datafield", item.Key);
                            //columnFields_config.Add("cellsalign", "left");
                            //columnFields_config.Add("filtertype", "list");

                            if (dictionary.ContainsKey("width"))
                            {
                                columnFields_config.Add("width", dictionary["width"].ToString());
                            }

                            columnFields_config.Add("filterable", true);
                            columnFields_config.Add("sortable", true);
                            columnFields_config.Add("cellclassname", string.Empty);
                            columnFields_config.Add("editable", false);
                            columnFields.Add(columnFields_config);
                            break;
                        }
                    case "bool":
                        {
                            columnFields_config.Add("text", dictionary["label"].ToString());
                            columnFields_config.Add("datafield", item.Key);
                            columnFields_config.Add("cellsalign", "left");
                            columnFields_config.Add("filtertype", "list");

                            if (dictionary.ContainsKey("width"))
                            {
                                columnFields_config.Add("width", dictionary["width"].ToString());
                            }

                            columnFields_config.Add("filterable", true);
                            columnFields_config.Add("sortable", true);
                            columnFields_config.Add("cellclassname", string.Empty);
                            columnFields_config.Add("threestatecheckbox", false);
                            columnFields_config.Add("columntype", "checkbox");
                            columnFields_config.Add("editable", true);
                            columnFields.Add(columnFields_config);
                            break;
                        }
                    default:
                        {
                            columnFields_config.Add("text", dictionary["label"].ToString());
                            columnFields_config.Add("datafield", item.Key);
                            columnFields_config.Add("cellsalign", "left");
                            columnFields_config.Add("filtertype", "input");

                            if (dictionary.ContainsKey("width"))
                            {
                                columnFields_config.Add("width", dictionary["width"].ToString());
                            }

                            columnFields_config.Add("filterable", true);
                            columnFields_config.Add("sortable", true);
                            columnFields_config.Add("cellclassname", string.Empty);
                            columnFields_config.Add("editable", false);
                            columnFields.Add(columnFields_config);
                            break;
                        }
                }
            }


            column_config.Add("datafields", dataFields);
            column_config.Add("columnfields", columnFields);

            result.Add("column_config", column_config);
            var json = JsonConvert.SerializeObject(result);
            //object jsonObj = new JavaScriptSerializer().DeserializeObject(json);
            return column_config;
        }
        #endregion

        #region FormatDateFromWidget
        public string FormatDateFromWidget(string source, string raw_date)
        {
            DateTime dt = Convert.ToDateTime(raw_date.Substring(4, 11));
            String result = "";
            switch (source)
            {
                case "jqxdaterange":
                    {
                        //result = String.Format("{yyyy-MM-dd}", dt);
                        result = dt.ToString("yyyy-MM-dd");
                        break;
                    }

            }

            return result;
        }
        #endregion

        #region GetAvailableModules
        public Dictionary<string, object> GetAvailableModules(string _id, string _parentid)
        {
            ModuleModels modules = new ModuleModels();

            Dictionary<string, object> response = new Dictionary<string, object>();
            Dictionary<string, object> menu = new Dictionary<string, object>();
            Dictionary<string, object> submenu_temp_arr = new Dictionary<string, object>();
            List<object> submenu_header = new List<object>();
            List<object> z = new List<object>();
            DataTable dTableModules = modules.GetAvailabeModules();

            foreach (DataRow dataRow in dTableModules.Rows)
            {
                if (dataRow["ParentId"].ToString() == "0")
                {
                    bool isActive = false;
                    string alvin = dataRow["Id"].ToString();
                    if (_parentid == alvin)
                    {
                        isActive = true;
                    }
                    menu.Add(dataRow["Id"].ToString(),
                            new Dictionary<string, object> {
                                { "Id", dataRow["Id"].ToString() },
                                { "Name", dataRow["Name"].ToString() },
                                { "Icon", dataRow["Icon"].ToString() },
                                { "items", new List<object>() },
                                { "is_active", isActive },
                                { "URL", dataRow["URL"].ToString() },
                            }
                        );
                }
                else
                {
                    bool isActive = false;
                    if (_id == dataRow["Id"].ToString())
                    {
                        isActive = true;
                    }

                    Dictionary<string, object> submenu_details = new Dictionary<string, object>();
                    Dictionary<string, object> y = new Dictionary<string, object>();
                    submenu_details.Add("Id", dataRow["Id"].ToString());
                    submenu_details.Add("Name", dataRow["Name"].ToString());
                    submenu_details.Add("Icon", dataRow["Icon"].ToString());
                    submenu_details.Add("items", new Dictionary<string, object>());
                    submenu_details.Add("is_active", isActive);
                    submenu_details.Add("URL", dataRow["URL"].ToString());

                    //submenu_temp_arr.Add(dataRow["ParentId"].ToString(), submenu_details);

                    submenu_header.Add(submenu_details);
                    //menu.Add(dataRow["Id"].ToString(), new Dictionary<string, object>() {
                    //    { "items",submenu }
                    //});

                    if (menu.ContainsKey(dataRow["ParentId"].ToString()))
                    {
                        var x = menu[dataRow["ParentId"].ToString()];
                        y = x as Dictionary<string, object>;
                        z = y["items"] as List<object>;
                        z.Add(submenu_details);
                        y["items"] = z;
                    }

                    //foreach (DataColumn column in dTableModules.Columns)
                    //{
                    //    //ColumnName = column.ColumnName;
                    //    //ColumnData = dataRow[column].ToString();
                    //}
                    //Console.WriteLine("--- Row ---"); // Print separator.
                    //foreach (var item in dataRow.ItemArray) // Loop over the items.
                    //{
                    //    Console.Write("Item: "); // Print label.
                    //    Console.WriteLine(item); // Invokes ToString abstract method.
                    //}
                }


            }
            return menu;
        }
        #endregion

        #region SetSessionVariableUser
        public bool SetSessionVariableUser(string username)
        {
            HttpClient client = new HttpClient();
            //url for getting user info from AD
            var url = ConfigurationManager.AppSettings["api_base_" + ConfigurationManager.AppSettings["env"].ToString()].ToString() + "login/userinfo?username=" + username + "&json=true";
            HttpResponseMessage res = client.GetAsync(url).Result;
            if (res.IsSuccessStatusCode)
            {
                string strJson = res.Content.ReadAsStringAsync().Result;
                dynamic jObj = (JObject)JsonConvert.DeserializeObject(strJson);
                JavaScriptSerializer j = new JavaScriptSerializer();
                object a = j.Deserialize(strJson, typeof(object));
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);

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
                userObject.ThumbnailPhoto = dict["thumbnailPhoto"].ToString();

                int IsInLocal = userModel.CheckIdFromLocal(dict["mail"].ToString());
                //int IsInLocal = 0;
                if (IsInLocal <= 0)
                {
                    IsInLocal = userModel.AddUser(userObject);
                }

                HttpContext.Current.Session.Add("userId_local", IsInLocal);
                HttpContext.Current.Session.Add("cn", dict["cn"]);
                HttpContext.Current.Session.Add("title", dict["title"]);
                HttpContext.Current.Session.Add("department", dict["department"]);
                HttpContext.Current.Session.Add("company", dict["company"]);
                HttpContext.Current.Session.Add("employeeNumber", dict["employeeNumber"]);
                HttpContext.Current.Session.Add("mail", dict["mail"]);
                HttpContext.Current.Session.Add("thumbnailPhoto", dict["thumbnailPhoto"]);
            }

            return true;
        }
        #endregion

        #region GetCurrentUser
        public string GetCurrentUser()
        {
            string[] userInformation = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.ToString().Split(@"\".ToCharArray());
            string result = userInformation[1].ToString();
            //string result = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.ToString();
            //return result.Substring(8);
            return result;
        }
        #endregion

        #region ConvertToTimestamp
        /* @author      :   AC <aabasolo@allegromicro.com>
         * @date        :   10/05/2016 12:54 PM
         * @description :   get current unix timestamp
         */
        public Int32 ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (Int32)span.TotalSeconds;
        }
        #endregion

        #region PrepareMenu
        public List<MenuObject> PrepareMenu(int child, int parent, string usertype, int userId)
        {
            Dictionary<int, MenuObject> header = new Dictionary<int, MenuObject>();
            List<MenuObject> res = new List<MenuObject>();
            ModuleModels mod = new ModuleModels();
            DataTable dtMod = mod.GetAvailabeModules();
            DataTable dtUserMod = mod.GetUserModules(userId);

            bool is_active = false;
            if (usertype == "superadmin")
            {
                foreach (DataRow row in dtMod.Rows)
                {
                    List<MenuObject> subitems = new List<MenuObject>();
                    MenuObject menuobj = new MenuObject();

                    Dictionary<string, object> details = new Dictionary<string, object>();
                    is_active = false;
                    if (Int32.Parse(row["ParentId"].ToString()) == 0)
                    {
                        if (Int32.Parse(row["Id"].ToString()) == parent || Int32.Parse(row["Id"].ToString()) == child)
                        {
                            is_active = true;
                        }

                        menuobj.Id = Int32.Parse(row["Id"].ToString());
                        menuobj.Name = row["Name"].ToString();
                        menuobj.Icon = row["Icon"].ToString();
                        menuobj.IsActive = is_active;
                        menuobj.URL = row["URL"].ToString();

                        header.Add(Int32.Parse(row["Id"].ToString()), menuobj);

                        res.Add(menuobj);
                    }
                    else
                    {
                        if (Int32.Parse(row["Id"].ToString()) == child)
                        {
                            is_active = true;
                        }

                        menuobj.Id = Int32.Parse(row["Id"].ToString());
                        menuobj.Name = row["Name"].ToString();
                        menuobj.Icon = row["Icon"].ToString();
                        menuobj.IsActive = is_active;
                        menuobj.URL = row["URL"].ToString();

                        if (header[Int32.Parse(row["ParentId"].ToString())].Items != null)
                        {
                            subitems = header[Int32.Parse(row["ParentId"].ToString())].Items;
                        }

                        subitems.Add(menuobj);

                        header[Int32.Parse(row["ParentId"].ToString())].Items = subitems;
                    }


                }
            }
            else
            {
                List<int> p = new List<int>();
                List<int> c = new List<int>();

                foreach (DataRow row in dtUserMod.Rows)
                {
                    p.Add(Int32.Parse(row["ParentId"].ToString()));
                    c.Add(Int32.Parse(row["ObjectId"].ToString()));
                }

                foreach (DataRow row in dtMod.Rows)
                {
                    List<MenuObject> subitems = new List<MenuObject>();
                    MenuObject menuobj = new MenuObject();

                    Dictionary<string, object> details = new Dictionary<string, object>();
                    is_active = false;
                    if (Int32.Parse(row["ParentId"].ToString()) == 0)
                    {
                        if (p.Contains(Int32.Parse(row["Id"].ToString())) || c.Contains(Int32.Parse(row["Id"].ToString())))
                        {
                            if (Int32.Parse(row["Id"].ToString()) == parent || Int32.Parse(row["Id"].ToString()) == child)
                            {
                                is_active = true;
                            }

                            menuobj.Id = Int32.Parse(row["Id"].ToString());
                            menuobj.Name = row["Name"].ToString();
                            menuobj.Icon = row["Icon"].ToString();
                            menuobj.IsActive = is_active;
                            menuobj.URL = row["URL"].ToString();

                            header.Add(Int32.Parse(row["Id"].ToString()), menuobj);

                            res.Add(menuobj);
                        }
                    }
                    else
                    {
                        if (c.Contains(Int32.Parse(row["Id"].ToString())))
                        {
                            if (Int32.Parse(row["Id"].ToString()) == child)
                            {
                                is_active = true;
                            }

                            menuobj.Id = Int32.Parse(row["Id"].ToString());
                            menuobj.Name = row["Name"].ToString();
                            menuobj.Icon = row["Icon"].ToString();
                            menuobj.IsActive = is_active;
                            menuobj.URL = row["URL"].ToString();

                            if (header[Int32.Parse(row["ParentId"].ToString())].Items != null)
                            {
                                subitems = header[Int32.Parse(row["ParentId"].ToString())].Items;
                            }

                            subitems.Add(menuobj);

                            header[Int32.Parse(row["ParentId"].ToString())].Items = subitems;
                        }

                    }


                }
            }

            return res;
        }
        #endregion

        #region prepare PM Grid custom columns
        public string PreparePMGridCols(DateTime start, DateTime end)
        {
            string cols = start.ToString("MMM-yyyy") + ",";
            DateTime xDate = start;
            DateTime firstDay = new DateTime(start.Year, start.Month, 1);
            DateTime lastDay = new DateTime(end.Year, end.Month, 1);

            while (firstDay < lastDay)
            {
                cols += firstDay.AddMonths(1).ToString("MMM-yyyy") + ',';
                firstDay = firstDay.AddMonths(1);
            }

            return cols.TrimEnd(',');
        }
        #endregion

        #region format new date param
        public Dictionary<string, string> FormatNewDateParam(string str)
        {
            Dictionary<string, string> dateParam = new Dictionary<string, string>();
            string[] d = str.Split('-');

            var r = DateTime.Parse(d[1].ToString() + "-" + MonthWordsToNumber(d[0].ToString()) + "-01");
            var lastDayOfMonth = DateTime.DaysInMonth(r.Year, r.Month);

            dateParam.Add("dateFrom", d[1].ToString() + "-" + MonthWordsToNumber(d[0].ToString()) + "-01 12:00:01");
            dateParam.Add("dateTo", d[1].ToString() + "-" + MonthWordsToNumber(d[0].ToString()) + "-" + lastDayOfMonth.ToString() + " 12:00:01");

            return dateParam;
        }
        #endregion

        #region format month from 3letter word
        public string MonthWordsToNumber(string str)
        {
            string monthnum = "";

            switch (str.ToLower())
            {
                case "jan":
                    monthnum = "01";
                    break;
                case "feb":
                    monthnum = "02";
                    break;
                case "mar":
                    monthnum = "03";
                    break;
                case "apr":
                    monthnum = "04";
                    break;
                case "may":
                    monthnum = "05";
                    break;
                case "jun":
                    monthnum = "06";
                    break;
                case "jul":
                    monthnum = "07";
                    break;
                case "aug":
                    monthnum = "08";
                    break;
                case "sep":
                    monthnum = "09";
                    break;
                case "oct":
                    monthnum = "10";
                    break;
                case "nov":
                    monthnum = "11";
                    break;
                case "dec":
                    monthnum = "12";
                    break;
            }
            return monthnum;
        }
        #endregion

        #region build chart
        public string BuildChart(Dictionary<string, object> arr, string chart_id)
        {
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(arr);

            string _chart = "";

            _chart += "<script type=\"text/javascript\">" +
                        "function init_hchart_" + chart_id + "()" +
                        "{" +
                        "$(\"#" + chart_id + "\").highcharts(" + json + ");" +
                        "}" +
                        "function redraw_hchart_" + chart_id + "()" +
                        "{" +
                        "var chart = $(\"#" + chart_id + "\").highcharts();" +
                        "setTimeout(function(){" +
                        "chart.reflow();" +
                        "}, 500);" +
                        "}" +
                        "</script>" +
                        "<div id=\"" + chart_id + "\" class=\"hchart_container\"></div>";

            return _chart;
        }
        #endregion

        #region FormatFilterConditions
        public string FormatFilterConditions(Dictionary<string, string> filters, int filterCount, Dictionary<string, string> columns)
        {
            string where = " WHERE ((";
            string tmpdatafield = "";
            int tmpfilteroperator = 0;
            string valuesPrep = "";
            List<string> values = new List<string>();

            string filtervalue = "";
            string filtercondition = "";
            string filterdatafield = "";
            string filteroperator = "";

            for (int i = 0; i < filterCount; i++)
            {
                string condition = "";
                string value = "";

                // get the filter's value.
                filtervalue = filters["filtervalue" + i].ToString();

                // get the filter's condition.
                filtercondition = filters["filtercondition" + i].ToString();

                // get the filter's column.
                //filterdatafield = filters["filterdatafield" + i].ToString();
                filterdatafield = columns.First(x => x.Value.Contains(filters["filterdatafield" + i].ToString())).Key;

                // get the filter's operator.
                filteroperator = filters["filteroperator" + i].ToString();

                if (tmpdatafield == "")
                {
                    tmpdatafield = filterdatafield;
                }
                else if (tmpdatafield != filterdatafield)
                {
                    // rherejias 11/22/16 change "where" value to ")OR(" for filtering of search".
                    // original value: ")AND(".
                    // incase of bugs return to original value.
                    where += ")OR(";
                }
                else if (tmpdatafield == filterdatafield)
                {
                    if (tmpfilteroperator == 0)
                    {
                        where += " AND ";
                    }
                    else
                    {
                        where += " OR ";
                    }
                }

                // build the "WHERE" clause depending on the filter's condition, value and datafield.
                switch (filtercondition)
                {
                    case "CONTAINS":
                        condition = " LIKE ";
                        value = "'%" + filtervalue + "%'";
                        break;
                    case "DOES_NOT_CONTAIN":
                        condition = " NOT LIKE ";
                        value = "'%" + filtervalue + "%'";
                        break;
                    case "EQUAL":
                        condition = " = ";
                        value = filtervalue;
                        break;
                    case "NOT_EQUAL":
                        condition = " != ";
                        value = filtervalue;
                        break;
                    case "GREATER_THAN":
                        condition = " > ";
                        value = filtervalue;
                        break;
                    case "LESS_THAN":
                        condition = " < ";
                        value = filtervalue;
                        break;
                    case "GREATER_THAN_OR_EQUAL":
                        condition = " >= ";
                        value = filtervalue;
                        break;
                    case "LESS_THAN_OR_EQUAL":
                        condition = " <= ";
                        value = filtervalue;
                        break;
                    case "STARTS_WITH":
                        condition = " LIKE ";
                        value = "'" + filtervalue + "%'";
                        break;
                    case "ENDS_WITH":
                        condition = " LIKE ";
                        value = "'%" + filtervalue + "'";
                        break;
                    case "NULL":
                        condition = " IS NULL ";
                        value = "'%" + filtervalue + "%'";
                        break;
                    case "NOT_NULL":
                        condition = " IS NOT NULL ";
                        value = "'%" + filtervalue + "%'";
                        break;
                }
                where += " " + filterdatafield + condition + value + " ";
                valuesPrep = valuesPrep + "s";
                values.Add(value);

                if (i == (filterCount - 1))
                {
                    where += "))";
                }

                tmpfilteroperator = Int32.Parse(filteroperator);
                tmpdatafield = filterdatafield;
            }
            string alvin = where;
            return where;
        }
        #endregion

        #region FormatSortingConditions
        public string FormatSortingConditions()
        {
            return "hello world";
        }
        #endregion

        //rherejias, 12/2/2016, for retrieval of user IP and MAC address 
        #region Retrieve user IP and MAC address
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static string GetMACAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces().Where
                (nic => nic.OperationalStatus == OperationalStatus.Up).Select
                (nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
        }
        #endregion
    }
}