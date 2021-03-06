using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Xml.Serialization;
using SubSonic;

// <auto-generated />

namespace Vietbait.Lablink.Model
{
    /// <summary>
    ///     Strongly-typed collection for the LUser class.
    /// </summary>
    [Serializable]
    public class LUserCollection : ActiveList<LUser, LUserCollection>
    {
        /// <summary>
        ///     Filters an existing collection based on the set criteria. This is an in-memory filter
        ///     Thanks to developingchris for this!
        /// </summary>
        /// <returns>LUserCollection</returns>
        public LUserCollection Filter()
        {
            for (int i = Count - 1; i > -1; i--)
            {
                LUser o = this[i];
                foreach (Where w in wheres)
                {
                    bool remove = false;
                    PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
    }

    /// <summary>
    ///     This is an ActiveRecord class which wraps the L_USER table.
    /// </summary>
    [Serializable]
    public class LUser : ActiveRecord<LUser>, IActiveRecord
    {
        #region .ctors and Default Settings

        public LUser()
        {
            SetSQLProps();
            InitSetDefaults();
            MarkNew();
        }

        public LUser(bool useDatabaseDefaults)
        {
            SetSQLProps();
            if (useDatabaseDefaults)
                ForceDefaults();
            MarkNew();
        }

        public LUser(object keyID)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByKey(keyID);
        }

        public LUser(string columnName, object columnValue)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByParam(columnName, columnValue);
        }

        private void InitSetDefaults()
        {
            SetDefaults();
        }

        protected static void SetSQLProps()
        {
            GetTableSchema();
        }

        #endregion

        #region Schema and Query Accessor	

        public static TableSchema.Table Schema
        {
            get
            {
                if (BaseSchema == null)
                    SetSQLProps();
                return BaseSchema;
            }
        }

        public static Query CreateQuery()
        {
            return new Query(Schema);
        }

        private static void GetTableSchema()
        {
            if (!IsSchemaInitialized)
            {
                //Schema declaration
                var schema = new TableSchema.Table("L_USER", TableType.Table, DataService.GetInstance("ORM"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns

                var colvarUserId = new TableSchema.TableColumn(schema);
                colvarUserId.ColumnName = "User_ID";
                colvarUserId.DataType = DbType.Decimal;
                colvarUserId.MaxLength = 0;
                colvarUserId.AutoIncrement = true;
                colvarUserId.IsNullable = false;
                colvarUserId.IsPrimaryKey = true;
                colvarUserId.IsForeignKey = false;
                colvarUserId.IsReadOnly = false;
                colvarUserId.DefaultSetting = @"";
                colvarUserId.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUserId);

                var colvarUserName = new TableSchema.TableColumn(schema);
                colvarUserName.ColumnName = "User_Name";
                colvarUserName.DataType = DbType.String;
                colvarUserName.MaxLength = 100;
                colvarUserName.AutoIncrement = false;
                colvarUserName.IsNullable = true;
                colvarUserName.IsPrimaryKey = false;
                colvarUserName.IsForeignKey = false;
                colvarUserName.IsReadOnly = false;
                colvarUserName.DefaultSetting = @"";
                colvarUserName.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUserName);

                var colvarRoleId = new TableSchema.TableColumn(schema);
                colvarRoleId.ColumnName = "Role_ID";
                colvarRoleId.DataType = DbType.Int32;
                colvarRoleId.MaxLength = 0;
                colvarRoleId.AutoIncrement = false;
                colvarRoleId.IsNullable = true;
                colvarRoleId.IsPrimaryKey = false;
                colvarRoleId.IsForeignKey = false;
                colvarRoleId.IsReadOnly = false;
                colvarRoleId.DefaultSetting = @"";
                colvarRoleId.ForeignKeyTableName = "";
                schema.Columns.Add(colvarRoleId);

                var colvarUserCode = new TableSchema.TableColumn(schema);
                colvarUserCode.ColumnName = "User_Code";
                colvarUserCode.DataType = DbType.String;
                colvarUserCode.MaxLength = 50;
                colvarUserCode.AutoIncrement = false;
                colvarUserCode.IsNullable = true;
                colvarUserCode.IsPrimaryKey = false;
                colvarUserCode.IsForeignKey = false;
                colvarUserCode.IsReadOnly = false;
                colvarUserCode.DefaultSetting = @"";
                colvarUserCode.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUserCode);

                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["ORM"].AddSchema("L_USER", schema);
            }
        }

        #endregion

        #region Props

        [XmlAttribute("UserId")]
        [Bindable(true)]
        public decimal UserId
        {
            get { return GetColumnValue<decimal>(Columns.UserId); }
            set { SetColumnValue(Columns.UserId, value); }
        }

        [XmlAttribute("UserName")]
        [Bindable(true)]
        public string UserName
        {
            get { return GetColumnValue<string>(Columns.UserName); }
            set { SetColumnValue(Columns.UserName, value); }
        }

        [XmlAttribute("RoleId")]
        [Bindable(true)]
        public int? RoleId
        {
            get { return GetColumnValue<int?>(Columns.RoleId); }
            set { SetColumnValue(Columns.RoleId, value); }
        }

        [XmlAttribute("UserCode")]
        [Bindable(true)]
        public string UserCode
        {
            get { return GetColumnValue<string>(Columns.UserCode); }
            set { SetColumnValue(Columns.UserCode, value); }
        }

        #endregion

        #region ObjectDataSource support

        /// <summary>
        ///     Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void Insert(string varUserName, int? varRoleId, string varUserCode)
        {
            var item = new LUser();

            item.UserName = varUserName;

            item.RoleId = varRoleId;

            item.UserCode = varUserCode;


            if (HttpContext.Current != null)
                item.Save(HttpContext.Current.User.Identity.Name);
            else
                item.Save(Thread.CurrentPrincipal.Identity.Name);
        }

        /// <summary>
        ///     Updates a record, can be used with the Object Data Source
        /// </summary>
        public static void Update(decimal varUserId, string varUserName, int? varRoleId, string varUserCode)
        {
            var item = new LUser();

            item.UserId = varUserId;

            item.UserName = varUserName;

            item.RoleId = varRoleId;

            item.UserCode = varUserCode;

            item.IsNew = false;
            if (HttpContext.Current != null)
                item.Save(HttpContext.Current.User.Identity.Name);
            else
                item.Save(Thread.CurrentPrincipal.Identity.Name);
        }

        #endregion

        #region Typed Columns

        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[0]; }
        }


        public static TableSchema.TableColumn UserNameColumn
        {
            get { return Schema.Columns[1]; }
        }


        public static TableSchema.TableColumn RoleIdColumn
        {
            get { return Schema.Columns[2]; }
        }


        public static TableSchema.TableColumn UserCodeColumn
        {
            get { return Schema.Columns[3]; }
        }

        #endregion

        #region Columns Struct

        public struct Columns
        {
            public static string UserId = @"User_ID";
            public static string UserName = @"User_Name";
            public static string RoleId = @"Role_ID";
            public static string UserCode = @"User_Code";
        }

        #endregion

        #region Update PK Collections

        #endregion

        #region Deep Save

        #endregion

        //no foreign key tables defined (0)


        //no ManyToMany tables defined (0)
    }
}