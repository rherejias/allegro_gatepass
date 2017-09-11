using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace GatePass.Models
{
    public class ItemModels
    {
        #region for item master list

        #region cols
        public Dictionary<string, string> GetItem_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("Id", "id_number");
            cols.Add("Name", "name_string");
            cols.Add("Description", "description_string");
            cols.Add("Category", "category_string");
            cols.Add("Type", "type_string");
            cols.Add("UOM", "uom_string");
            cols.Add("RelatedTo", "related_to_string");
            cols.Add("Code", "code_string");
            cols.Add("IsActive", "is_active_bool");
            cols.Add("CategoryCode", "category_code_string");
            cols.Add("UOMCode", "uom_code_string");
            cols.Add("TypeCode", "type_code_string");
            cols.Add("DeptCode", "dept_code_string");
            cols.Add("OrigName", "origname_string");
            return cols;
        }
        #endregion

        #region get records
        public DataTable GetItem(int offset, int next, string where, string sorting, string IsActive)
        {
            Dictionary<string, string> cols = GetItem_cols();
            if (sorting == "")
            {
                sorting = "RelatedTo ASC";
            }
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += " AND IsActive = " + IsActive;
            }
            string pagination = "";
            if (offset != 0 && next != 0)
            {
                if (next > 0)
                {
                    pagination = "OFFSET " + offset + " ROWS FETCH NEXT " + next + " ROWS ONLY";
                }
            }
            string sql = "SELECT ";
            foreach (var item in cols)
            {
                sql += item.Key + " AS " + item.Value + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += " FROM vwItemsMasterlist " + where + " AND Category <> 'OLD' ORDER BY " + sorting + " " + pagination + ";";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion

        #region count
        public int GetItem_count(string where, string IsActive)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += "AND IsActive = " + IsActive;
            }
            string sql = "SELECT COUNT(*) FROM vwItemsMasterlist " + where + " AND Category <> 'OLD'";
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion

        #endregion

        #region for unit of measure

        #region cols
        public Dictionary<string, string> GetUnitOfMeasure_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("Name", "name_string");
            cols.Add("Description", "description_string");
            cols.Add("IsActive", "is_active_bool");
            return cols;
        }
        #endregion

        #region get records
        public DataTable GetUnitOfMeasure(int offset, int next, string where, string sorting, string IsActive)
        {
            Dictionary<string, string> cols = GetUnitOfMeasure_cols();
            if (sorting == "")
            {
                sorting = "Name ASC";
            }
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += " AND IsActive = " + IsActive;
            }
            string pagination = "";
            if (offset != 0 && next != 0)
            {
                if (next > 0)
                {
                    pagination = "OFFSET " + offset + " ROWS FETCH NEXT " + next + " ROWS ONLY";
                }
            }
            string sql = "SELECT ";
            foreach (var item in cols)
            {
                sql += item.Key + " AS " + item.Value + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += " FROM tblUnitOfMeasure " + where + " ORDER BY " + sorting + " " + pagination + ";";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion

        #region count
        public int GetUnitOfMeasure_count(string where, string IsActive)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += "AND IsActive = " + IsActive;
            }
            string sql = "SELECT COUNT(*) FROM tblUnitOfMeasure " + where;
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion

        #endregion

        #region for category e.g. office equipment

        #region cols
        public Dictionary<string, string> GetItemCategory_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("Name", "name_string");
            cols.Add("Description", "description_string");
            cols.Add("IsActive", "is_active_bool");
            return cols;
        }
        #endregion

        #region get records
        public DataTable GetItemCategory(int offset, int next, string where, string sorting, string IsActive)
        {
            Dictionary<string, string> cols = GetUnitOfMeasure_cols();
            if (sorting == "")
            {
                sorting = "Name ASC";
            }
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += " AND IsActive = " + IsActive;
            }
            string pagination = "";
            if (offset != 0 && next != 0)
            {
                if (next > 0)
                {
                    pagination = "OFFSET " + offset + " ROWS FETCH NEXT " + next + " ROWS ONLY";
                }
            }
            string sql = "SELECT ";
            foreach (var item in cols)
            {
                sql += item.Key + " AS " + item.Value + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += " FROM tblItemCategories " + where + " AND Name <> 'OLD' ORDER BY " + sorting + " " + pagination + ";";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion

        #region count
        public int GetItemCategory_count(string where, string IsActive)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += "AND IsActive = " + IsActive;
            }
            string sql = "SELECT COUNT(*) FROM tblItemCategories " + where + " AND Name <> 'OLD'";
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion


        #endregion

        #region for item type e.g. returnable, non-returnable

        #region cols
        public Dictionary<string, string> GetItemType_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("Name", "name_string");
            cols.Add("Description", "description_string");
            cols.Add("IsActive", "is_active_bool");
            return cols;
        }
        #endregion

        #region get records
        public DataTable GetItemType(int offset, int next, string where, string sorting, string IsActive)
        {
            Dictionary<string, string> cols = GetUnitOfMeasure_cols();
            if (sorting == "")
            {
                sorting = "Name ASC";
            }
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += " AND IsActive = " + IsActive;
            }
            string pagination = "";
            if (offset != 0 && next != 0)
            {
                if (next > 0)
                {
                    pagination = "OFFSET " + offset + " ROWS FETCH NEXT " + next + " ROWS ONLY";
                }
            }
            string sql = "SELECT ";
            foreach (var item in cols)
            {
                sql += item.Key + " AS " + item.Value + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += " FROM tblItemTypes " + where + " ORDER BY " + sorting + " " + pagination + ";";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion

        #region count
        public int GetItemType_count(string where, string IsActive)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += "AND IsActive = " + IsActive;
            }
            string sql = "SELECT COUNT(*) FROM tblItemTypes " + where;
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion

        #endregion

        #region for item Department 

        #region cols
        public Dictionary<string, string> GetItemDepartment_cols()
        {
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("Id", "id_number");
            cols.Add("Code", "code_string");
            cols.Add("Name", "name_string");
            cols.Add("Description", "description_string");
            cols.Add("IsActive", "is_active_bool");
            return cols;
        }
        #endregion

        #region get records
        public DataTable GetItemDepartment(int offset, int next, string where, string sorting, string IsActive)
        {
            Dictionary<string, string> cols = GetUnitOfMeasure_cols();
            if (sorting == "")
            {
                sorting = "Name ASC";
            }
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += " AND IsActive = " + IsActive;
            }
            string pagination = "";
            if (offset != 0 && next != 0)
            {
                if (next > 0)
                {
                    pagination = "OFFSET " + offset + " ROWS FETCH NEXT " + next + " ROWS ONLY";
                }
            }
            string sql = "SELECT ";
            foreach (var item in cols)
            {
                sql += item.Key + " AS " + item.Value + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += " FROM tblItemDepartmentRelationship " + where + " ORDER BY " + sorting + " " + pagination + ";";
            return Library.ConnectionString.returnCon.executeSelectQuery(sql, CommandType.Text);
        }
        #endregion

        #region count
        public int GetItemDepartment_count(string where, string IsActive)
        {
            if (where == null || where == "" || where == string.Empty)
            {
                where = "WHERE IsActive = " + IsActive;
            }
            else
            {
                where += "AND IsActive = " + IsActive;
            }
            string sql = "SELECT COUNT(*) FROM tblItemDepartmentRelationship " + where;
            return Convert.ToInt32(Library.ConnectionString.returnCon.executeScalarQuery(sql, CommandType.Text));
        }
        #endregion

        #endregion

        #region for deactivate models
        public bool DeactivateItem(ItemMasterObject itemMas)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemMas.Id),
                new SqlParameter("@Code", itemMas.Code),
                new SqlParameter("@IsActive", itemMas.IsActive),
                new SqlParameter("@table", "tblItems"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemMas.AddedBy),
                new SqlParameter("@DateAdded", itemMas.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);
        }

        public bool DeactivateCategory(CategoryObject itemcat)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemcat.Id),
                new SqlParameter("@Code", itemcat.Code),
                new SqlParameter("@IsActive", itemcat.IsActive),
                new SqlParameter("@table", "tblItemCategories"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemcat.AddedBy),
                new SqlParameter("@DateAdded", itemcat.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);
        }

        public bool DeactivateType(ItemTypeObject itemtype)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemtype.Id),
                new SqlParameter("@Code", itemtype.Code),
                new SqlParameter("@IsActive", itemtype.IsActive),
                new SqlParameter("@table", "tblItemTypes"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemtype.AddedBy),
                new SqlParameter("@DateAdded", itemtype.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);
        }

        public bool DeactivateMeasure(UnitMeasureObject itemmeasure)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemmeasure.Id),
                new SqlParameter("@Code", itemmeasure.Code),
                new SqlParameter("@IsActive", itemmeasure.IsActive),
                new SqlParameter("@table", "tblUnitOfMeasure"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemmeasure.AddedBy),
                new SqlParameter("@DateAdded", itemmeasure.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);
        }

        public bool DeactivateDepartment(ItemDepartmentObject itemdepartment)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                 new SqlParameter("@id", itemdepartment.Id),
                new SqlParameter("@Code", itemdepartment.Code),
                new SqlParameter("@IsActive", itemdepartment.IsActive),
                new SqlParameter("@table", "tblItemDepartmentRelationship"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemdepartment.AddedBy),
                new SqlParameter("@DateAdded", itemdepartment.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spDeactivateItem", params_, CommandType.StoredProcedure);
        }
        #endregion

        #region for activate models
        public bool ActivateItem(ItemMasterObject itemMas)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemMas.Id),
                new SqlParameter("@Code", itemMas.Code),
                new SqlParameter("@table", "tblItems"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemMas.AddedBy),
                new SqlParameter("@DateAdded", itemMas.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spActivateItem", params_, CommandType.StoredProcedure);
        }

        public bool ActivateCategory(CategoryObject itemcat)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemcat.Id),
                new SqlParameter("@Code", itemcat.Code),
                new SqlParameter("@table", "tblItemCategories"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemcat.AddedBy),
                new SqlParameter("@DateAdded", itemcat.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spActivateItem", params_, CommandType.StoredProcedure);
        }

        public bool ActivateType(ItemTypeObject itemtype)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemtype.Id),
                new SqlParameter("@Code", itemtype.Code),
                new SqlParameter("@table", "tblItemTypes"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemtype.AddedBy),
                new SqlParameter("@DateAdded", itemtype.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spActivateItem", params_, CommandType.StoredProcedure);
        }

        public bool ActivateMeasure(UnitMeasureObject itemmeasure)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemmeasure.Id),
                new SqlParameter("@Code", itemmeasure.Code),
                new SqlParameter("@table", "tblUnitOfMeasure"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemmeasure.AddedBy),
                new SqlParameter("@DateAdded", itemmeasure.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spActivateItem", params_, CommandType.StoredProcedure);
        }

        public bool ActivateDepartment(ItemDepartmentObject itemdepartment)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                 new SqlParameter("@id", itemdepartment.Id),
                new SqlParameter("@Code", itemdepartment.Code),
                new SqlParameter("@table", "tblItemDepartmentRelationship"),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@AddedBy", itemdepartment.AddedBy),
                new SqlParameter("@DateAdded", itemdepartment.DateAdded)
            };

            return Library.ConnectionString.returnCon.executeQuery("spActivateItem", params_, CommandType.StoredProcedure);
        }
        #endregion

        #region for adding of items
        public bool AddItems(ItemMasterObject itemMas)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Name", itemMas.Name),
                new SqlParameter("@Description", itemMas.Description),
                new SqlParameter("@ItemCategoryCode", itemMas.ItemCategoryCode),
                new SqlParameter("@ItemTypeCode", itemMas.ItemTypeCode),
                new SqlParameter("@ItemDepRelCode", itemMas.ItemDepRelCode),
                new SqlParameter("@UOM", itemMas.ItemUOM),
                new SqlParameter("@IsActive", itemMas.IsActive),
                new SqlParameter("@AddedBy", itemMas.AddedBy),
                new SqlParameter("@DateAdded", itemMas.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddItems", params_, CommandType.StoredProcedure);
        }

        public bool AddCategory(CategoryObject itemCat)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Name", itemCat.Name),
                new SqlParameter("@Description", itemCat.Description),
                new SqlParameter("@IsActive", itemCat.IsActive),
                new SqlParameter("@AddedBy", itemCat.AddedBy),
                new SqlParameter("@DateAdded", itemCat.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddCategoryItem", params_, CommandType.StoredProcedure);
        }

        public bool AddType(ItemTypeObject itemType)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Name", itemType.Name),
                new SqlParameter("@Description", itemType.Description),
                new SqlParameter("@IsActive", itemType.IsActive),
                new SqlParameter("@AddedBy", itemType.AddedBy),
                new SqlParameter("@DateAdded", itemType.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddTypeItem", params_, CommandType.StoredProcedure);
        }

        public bool AddMeasure(UnitMeasureObject itemMeasure)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Name", itemMeasure.Name),
                new SqlParameter("@Description", itemMeasure.Description),
                new SqlParameter("@IsActive", itemMeasure.IsActive),
                new SqlParameter("@AddedBy", itemMeasure.AddedBy),
                new SqlParameter("@DateAdded", itemMeasure.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddMeasureItem", params_, CommandType.StoredProcedure);
        }

        public bool AddDepartment(ItemDepartmentObject itemDepartment)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@Name", itemDepartment.Name),
                new SqlParameter("@Description", itemDepartment.Description),
                new SqlParameter("@IsActive", itemDepartment.IsActive),
                new SqlParameter("@AddedBy", itemDepartment.AddedBy),
                new SqlParameter("@DateAdded", itemDepartment.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddDepartment", params_, CommandType.StoredProcedure);
        }
        #endregion

        #region for editing of items 
        public bool EditItems(ItemMasterObject itemMas)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemMas.Id),
                new SqlParameter("@Code", itemMas.Code),
                new SqlParameter("@Name", itemMas.Name),
                new SqlParameter("@Description", itemMas.Description),
                new SqlParameter("@ItemCategoryCode", itemMas.ItemCategoryCode),
                new SqlParameter("@ItemTypeCode", itemMas.ItemTypeCode),
                new SqlParameter("@ItemDepRelCode", itemMas.ItemDepRelCode),
                new SqlParameter("@UOM", itemMas.ItemUOM),
                new SqlParameter("@IsActive", itemMas.IsActive),
                new SqlParameter("@AddedBy", itemMas.AddedBy),
                new SqlParameter("@DateAdded", itemMas.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateItems", params_, CommandType.StoredProcedure);
        }

        public bool EditCategory(CategoryObject itemCat)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemCat.Id),
                new SqlParameter("@Code", itemCat.Code),
                new SqlParameter("@Name", itemCat.Name),
                new SqlParameter("@Description", itemCat.Description),
                new SqlParameter("@IsActive", itemCat.IsActive),
                new SqlParameter("@AddedBy", itemCat.AddedBy),
                new SqlParameter("@DateAdded", itemCat.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateCategoryItem", params_, CommandType.StoredProcedure);
        }

        public bool EditType(ItemTypeObject itemType)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemType.Id),
                new SqlParameter("@Code", itemType.Code),
                new SqlParameter("@Name", itemType.Name),
                new SqlParameter("@Description", itemType.Description),
                new SqlParameter("@IsActive", itemType.IsActive),
                new SqlParameter("@AddedBy", itemType.AddedBy),
                new SqlParameter("@DateAdded", itemType.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateTypeItem", params_, CommandType.StoredProcedure);
        }

        public bool EditMeasure(UnitMeasureObject itemMeasure)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemMeasure.Id),
                new SqlParameter("@Code", itemMeasure.Code),
                new SqlParameter("@Name", itemMeasure.Name),
                new SqlParameter("@Description", itemMeasure.Description),
                new SqlParameter("@IsActive", itemMeasure.IsActive),
                new SqlParameter("@AddedBy", itemMeasure.AddedBy),
                new SqlParameter("@DateAdded", itemMeasure.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateMeasureItem", params_, CommandType.StoredProcedure);
        }

        public bool EditDepartment(ItemDepartmentObject itemDepartment)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@id", itemDepartment.Id),
                new SqlParameter("@Code", itemDepartment.Code),
                new SqlParameter("@Name", itemDepartment.Name),
                new SqlParameter("@Description", itemDepartment.Description),
                new SqlParameter("@IsActive", itemDepartment.IsActive),
                new SqlParameter("@AddedBy", itemDepartment.AddedBy),
                new SqlParameter("@DateAdded", itemDepartment.DateAdded),
                new SqlParameter("@IP",Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC",Helpers.CustomHelper.GetMACAddress())
            };

            return Library.ConnectionString.returnCon.executeQuery("spUpdateDepartment", params_, CommandType.StoredProcedure);
        }
        #endregion

        #region for export
        public bool export(string operation, string table)
        {
            SqlParameter[] params_ = new SqlParameter[] {
                new SqlParameter("@ProjectCode", "GAT"),
                new SqlParameter("@Module", "ITEM"),
                new SqlParameter("@Operation", operation),
                new SqlParameter("@Object", table),
                new SqlParameter("@ObjectId", " "),
                new SqlParameter("@ObjectCode", " "),
                new SqlParameter("@UserCode", Convert.ToInt32(HttpContext.Current.Session["userId_local"])),
                new SqlParameter("@IP", Helpers.CustomHelper.GetLocalIPAddress()),
                new SqlParameter("@MAC", Helpers.CustomHelper.GetMACAddress()),
                new SqlParameter("@DateAdded", DateTime.Now),
            };

            return Library.ConnectionString.returnCon.executeQuery("spAddAuditTrail", params_, CommandType.StoredProcedure);
        }
        #endregion


        public DataTable ItemDetails(string ItemCode)
        {
            return Library.ConnectionString.returnCon.executeSelectQuery("SELECT * FROM vwItemsMasterlist " +
                " WHERE Code=" + ItemCode, CommandType.Text);
        }

        public DataTable ItemDetailsByName(string ItemName)
        {
            return Library.ConnectionString.returnCon.executeSelectQuery("SELECT * FROM tblItems " +
                " WHERE Name= '" + ItemName + "' ", CommandType.Text);
        }
    }
}