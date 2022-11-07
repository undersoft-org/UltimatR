
namespace System.Instant.Sqlset
{
    using System.Collections.Generic;
    using Microsoft.Data.SqlClient;

    public class DataDbSchema
    {
        #region Constructors

        public DataDbSchema(SqlConnection sqlDbConnection)
        {
            DataDbTables = new DbTables();
            DbConfig = new DbConfig(sqlDbConnection.ConnectionString);
        }

        public DataDbSchema(string dbConnectionString)
        {
            DataDbTables = new DbTables();
            DbConfig = new DbConfig(dbConnectionString);
            SqlDbConnection = new SqlConnection(dbConnectionString);
        }

        #endregion

        #region Properties

        public DbTables DataDbTables { get; set; }

        public DbConfig DbConfig { get; set; }

        public string DbName { get; set; }

        public List<DbTable> DbTables
        {
            get { return DataDbTables.List; }
            set { DataDbTables.List = value; }
        }

        public SqlConnection SqlDbConnection { get; set; }

        #endregion
    }

    public class DbConfig
    {
        #region Constructors

        public DbConfig()
        {
        }

        public DbConfig(string _User, string _Password, string _Source, string _Catalog, string _Provider = "SQLNCLI11")
        {
            User = _User;
            Password = _Password;
            Provider = _Provider;
            Source = _Source;
            InitCatalog = _Catalog;
            DbConnectionString = string.Format("Provider={0};Data Source = {1}; Persist Security Info=True;Password={2};User ID = {3}; Initial Catalog = {4}", Provider, Source, Password, User, InitCatalog);
        }

        public DbConfig(string dbConnectionString)
        {
            DbConnectionString = dbConnectionString;
        }

        #endregion

        #region Properties

        public string InitCatalog { get; set; }

        public string DbConnectionString { get; set; }

        public string Password { get; set; }

        public string Provider { get; set; }

        public string Source { get; set; }

        public string User { get; set; }

        #endregion
    }

    public static class DbHand
    {
        #region Properties




        public static DataDbSchema Schema { get; set; }




        public static DataDbSchema Temp { get; set; }

        #endregion
    }
}
