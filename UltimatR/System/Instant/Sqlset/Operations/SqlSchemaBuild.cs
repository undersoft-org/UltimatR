﻿
// <copyright file="SqlSchemaBuild.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Sqlset namespace.
/// </summary>
namespace System.Instant.Sqlset
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.Linq;

    #region Enums

    /// <summary>
    /// Enum BuildDbSchemaType
    /// </summary>
    public enum BuildDbSchemaType
    {
        /// <summary>
        /// The schema
        /// </summary>
        Schema,
        /// <summary>
        /// The temporary
        /// </summary>
        Temp
    }

    #endregion




    /// <summary>
    /// Class SqlSchemaBuild.
    /// </summary>
    public class SqlSchemaBuild
    {
        #region Fields

        /// <summary>
        /// The SQLCN
        /// </summary>
        private SqlConnection sqlcn;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="SqlSchemaBuild" /> class.
        /// </summary>
        /// <param name="_sqlcn">The SQLCN.</param>
        public SqlSchemaBuild(SqlConnection _sqlcn)
        {
            sqlcn = _sqlcn;
        }

        #endregion

        #region Methods





        /// <summary>
        /// Schemas the prepare.
        /// </summary>
        /// <param name="buildtype">The buildtype.</param>
        public void SchemaPrepare(BuildDbSchemaType buildtype = BuildDbSchemaType.Schema)
        {
            DataDbSchema dbs = new DataDbSchema(sqlcn);
            bool wasOpen = false;
            if (sqlcn.State == ConnectionState.Open)
                wasOpen = true;
            if (!wasOpen)
                sqlcn.Open();
            IEnumerable<DataRow> table = sqlcn.GetSchema("Tables").Rows.Cast<DataRow>().AsEnumerable().AsQueryable();
            IEnumerable<DataRow> columns = sqlcn.GetSchema("Columns").Rows.Cast<DataRow>().AsEnumerable().AsQueryable();
            IEnumerable<DataRow> index = sqlcn.GetSchema("IndexColumns").Rows.Cast<DataRow>().AsEnumerable().AsQueryable();
            List<DbTable> dbTables = table
                .Select(t =>
                new DbTable()
                {
                    TableName = t["TABLE_NAME"].ToString(),
                    DataDbColumns = new DbColumns()
                    {
                        List = columns.Where(c => t["TABLE_NAME"].Equals(c["TABLE_NAME"]))
                                .Select(k => new DbColumn
                                {
                                    ColumnName = k["COLUMN_NAME"].ToString(),
                                    RubricType = SqlNetType.SqlTypeToNet(k["DATA_TYPE"].ToString()),
                                    MaxLength = (k["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value) ? Convert.ToInt32(k["CHARACTER_MAXIMUM_LENGTH"]) : -1,
                                    isDBNull = (k["IS_NULLABLE"].ToString() == "YES") ? true : false,
                                    DbOrdinal = Convert.ToInt32(k["ORDINAL_POSITION"])
                                }).ToList()
                    },
                    DbPrimaryKey = index.Where(i =>
                    t["TABLE_NAME"].Equals(i["table_name"]))
                    .Where(it => columns
                    .Where(c => c["TABLE_NAME"].Equals(it["table_name"]) &&
                                c["COLUMN_NAME"].Equals(it["column_name"])).Any())
                    .Select(k => new DbColumn()
                    {
                        ColumnName = k["column_name"].ToString(),
                        isIdentity = (k["KeyType"].ToString() == "56") ? true : false,
                        isKey = true,
                        DbOrdinal = Convert.ToInt32(k["ordinal_position"]),
                        RubricType = SqlNetType.SqlTypeToNet(columns
                                             .Where(c => c["TABLE_NAME"].Equals(k["table_name"]) &&
                                                         c["COLUMN_NAME"].Equals(k["column_name"]))
                                                         .First()["DATA_TYPE"].ToString())
                    }).ToArray()
                }).ToList();

            dbs.DataDbTables.AddRange(dbTables.ToList());
            if (buildtype == BuildDbSchemaType.Schema)
                DbHand.Schema = dbs;
            else
                DbHand.Temp = dbs;
        }

        #endregion
    }
}
