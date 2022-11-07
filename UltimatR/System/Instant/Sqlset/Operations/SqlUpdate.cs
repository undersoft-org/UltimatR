
// <copyright file="SqlUpdate.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Sqlset namespace.
/// </summary>
namespace System.Instant.Sqlset
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Series;
    using System.Text;




    /// <summary>
    /// Class SqlUpdate.
    /// </summary>
    public class SqlUpdate
    {
        #region Fields

        /// <summary>
        /// The cn
        /// </summary>
        private SqlConnection _cn;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="SqlUpdate" /> class.
        /// </summary>
        /// <param name="cn">The cn.</param>
        public SqlUpdate(SqlConnection cn)
        {
            _cn = cn;
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="SqlUpdate" /> class.
        /// </summary>
        /// <param name="cnstring">The cnstring.</param>
        public SqlUpdate(string cnstring)
        {
            _cn = new SqlConnection(cnstring);
        }

        #endregion

        #region Methods











        /// <summary>
        /// Batches the update.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keysFromDeck">if set to <c>true</c> [keys from deck].</param>
        /// <param name="buildMapping">if set to <c>true</c> [build mapping].</param>
        /// <param name="updateKeys">if set to <c>true</c> [update keys].</param>
        /// <param name="updateExcept">The update except.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        /// <exception cref="System.Instant.Sqlset.SqlUpdate.SqlUpdateException"></exception>
        public IDeck<IDeck<IFigure>> BatchUpdate(IFigures table, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, int batchSize = 250)
        {
            try
            {
                IFigures tab = table;
                IList<FieldMapping> nMaps = new List<FieldMapping>();
                SqlAdapter afad = new SqlAdapter(_cn);
                StringBuilder sb = new StringBuilder();
                IDeck<IDeck<IFigure>> nSet = new Album<IDeck<IFigure>>();
                sb.AppendLine(@"    ");
                int count = 0;
                foreach (IFigure ir in table)
                {
                    if (ir.GetType().DeclaringType != tab.FigureType)
                    {
                        if (buildMapping)
                        {
                            SqlMapper imapper = new SqlMapper(tab, keysFromDeck);
                        }
                        nMaps = tab.Rubrics.Mappings;
                    }

                    foreach (FieldMapping nMap in nMaps)
                    {
                        IDeck<int> co = nMap.ColumnOrdinal;
                        IDeck<int> ko = nMap.KeyOrdinal;
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();
                        if (updateExcept != null)
                        {
                            ic = ic.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                            ik = ik.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                        }

                        string qry = BatchUpdateQuery(ir, nMap.DbTableName, ic, ik, updateKeys).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");
                        IDeck<IDeck<IFigure>> bIFigures = afad.ExecuteUpdate(sb.ToString(), tab);
                        if (nSet.Count == 0)
                            nSet = bIFigures;
                        else
                            foreach (IDeck<IFigure> its in bIFigures.AsValues())
                            {
                                if (nSet.Contains(its))
                                {
                                    nSet[its].Put(its.AsValues());
                                }
                                else
                                    nSet.Add(its);
                            }
                        sb.Clear();
                        sb.AppendLine(@"    ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"    ");

                IDeck<IDeck<IFigure>> rIFigures = afad.ExecuteUpdate(sb.ToString(), tab, true);

                if (nSet.Count == 0)
                    nSet = rIFigures;
                else
                    foreach (IDeck<IFigure> its in rIFigures.AsValues())
                    {
                        if (nSet.Contains(its))
                        {
                            nSet[its].Put(its.AsValues());
                        }
                        else
                            nSet.Add(its);
                    }

                return nSet;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlUpdateException(ex.ToString());
            }
        }










        /// <summary>
        /// Batches the update query.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="updateKeys">if set to <c>true</c> [update keys].</param>
        /// <returns>StringBuilder.</returns>
        public StringBuilder BatchUpdateQuery(IFigure card, string tableName, MemberRubric[] columns, MemberRubric[] keys, bool updateKeys = true)
        {
            StringBuilder sb = new StringBuilder();
            string tName = tableName;
            IFigure ir = card;
            object[] ia = ir.ValueArray;
            MemberRubric[] ic = columns;
            MemberRubric[] ik = keys;

            sb.AppendLine(@"    ");
            sb.Append("UPDATE " + tName + " SET ");
            bool isUpdateCol = false;
            string delim = "";
            int c = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].RubricName.ToLower() == "updated")
                    isUpdateCol = true;
                if (ia[columns[i].FieldId] != DBNull.Value)
                {
                    if (c > 0)
                        delim = ",";
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                                    @"{0}[{1}] = {2}{3}{2}",
                                    delim,
                                    columns[i].RubricName,
                                    (columns[i].RubricType == typeof(string) ||
                                    columns[i].RubricType == typeof(DateTime)) ? "'" : "",
                                    (columns[i].RubricType != typeof(string)) ?
                                    Convert.ChangeType(ia[columns[i].FieldId], columns[i].RubricType) :
                                    ia[columns[i].FieldId].ToString().Replace("'", "''")
                                    );
                    c++;
                }
            }

            if (DbHand.Schema.DataDbTables[tableName].DataDbColumns.Have("updated") && !isUpdateCol)
                sb.AppendFormat(CultureInfo.InvariantCulture, ", [updated] = '{0}'", DateTime.Now);

            if (updateKeys)
            {
                if (columns.Length > 0)
                    delim = ",";
                else
                    delim = "";
                c = 0;
                for (int i = 0; i < keys.Length; i++)
                {

                    if (ia[keys[i].FieldId] != DBNull.Value)
                    {
                        if (c > 0)
                            delim = ",";
                        sb.AppendFormat(CultureInfo.InvariantCulture,
                                        @"{0}[{1}] = {2}{3}{2}",
                                        delim,
                                        keys[i].RubricName,
                                        (keys[i].RubricType == typeof(string) ||
                                        keys[i].RubricType == typeof(DateTime)) ? "'" : "",
                                        (keys[i].RubricType != typeof(string)) ?
                                        Convert.ChangeType(ia[keys[i].FieldId], keys[i].RubricType) :
                                        ia[keys[i].FieldId].ToString().Replace("'", "''")
                                        );
                        c++;
                    }
                }
            }
            sb.AppendLine(" OUTPUT inserted.*, deleted.* ");
            delim = "";
            c = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (i > 0)
                    delim = " AND ";
                else
                    delim = " WHERE ";

                if (ia[keys[i].FieldId] != DBNull.Value)
                {
                    if (c > 0)
                        delim = " AND ";
                    else
                        delim = " WHERE ";

                    sb.AppendFormat(CultureInfo.InvariantCulture,
                                    @"{0} [{1}] = {2}{3}{2}",
                                    delim,
                                    keys[i].RubricName,
                                    (keys[i].RubricType == typeof(string) ||
                                    keys[i].RubricType == typeof(DateTime)) ? "'" : "",
                                    (ia[keys[i].FieldId] != DBNull.Value) ?
                                    (keys[i].RubricType != typeof(string)) ?
                                    Convert.ChangeType(ia[keys[i].FieldId], keys[i].RubricType) :
                                    ia[keys[i].FieldId].ToString().Replace("'", "''") : ""
                                    );
                    c++;
                }
            }
            sb.AppendLine("");
            sb.AppendLine(@"    ");
            return sb;
        }











        /// <summary>
        /// Bulks the update.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keysFromDeck">if set to <c>true</c> [keys from deck].</param>
        /// <param name="buildMapping">if set to <c>true</c> [build mapping].</param>
        /// <param name="updateKeys">if set to <c>true</c> [update keys].</param>
        /// <param name="updateExcept">The update except.</param>
        /// <param name="tempType">Type of the temporary.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        /// <exception cref="System.Instant.Sqlset.SqlUpdate.SqlUpdateException"></exception>
        public IDeck<IDeck<IFigure>> BulkUpdate(IFigures table, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            try
            {
                IFigures tab = table;
                if (tab.Count > 0)
                {
                    IList<FieldMapping> nMaps = new List<FieldMapping>();
                    if (buildMapping)
                    {
                        SqlMapper imapper = new SqlMapper(tab, keysFromDeck);
                    }
                    nMaps = tab.Rubrics.Mappings;
                    string dbName = _cn.Database;
                    SqlAdapter afad = new SqlAdapter(_cn);
                    afad.DataBulk(tab, tab.FigureType.Name, tempType, BulkDbType.TempDB);
                    _cn.ChangeDatabase(dbName);
                    IDeck<IDeck<IFigure>> nSet = new Album<IDeck<IFigure>>();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(@"    ");
                    foreach (FieldMapping nMap in nMaps)
                    {
                        sb.AppendLine(@"    ");

                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        if (updateExcept != null)
                        {
                            ic = ic.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                            ik = ik.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                        }

                        string qry = BulkUpdateQuery(dbName, tab.FigureType.Name, nMap.DbTableName, ic, ik, updateKeys).ToString();
                        sb.Append(qry);
                        sb.AppendLine(@"    ");
                    }
                    sb.AppendLine(@"    ");

                    var bIFigures = afad.ExecuteUpdate(sb.ToString(), tab, true);

                    if (nSet.Count == 0)
                        nSet = bIFigures;
                    else
                        foreach (IDeck<IFigure> its in bIFigures.AsValues())
                        {
                            if (nSet.Contains(its))
                            {
                                nSet[its].Put(its.AsValues());
                            }
                            else
                                nSet.Add(its);
                        }
                    sb.Clear();

                    return nSet;
                }
                else
                    return null;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlUpdateException(ex.ToString());
            }
        }











        /// <summary>
        /// Bulks the update query.
        /// </summary>
        /// <param name="DBName">Name of the database.</param>
        /// <param name="buforName">Name of the bufor.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="updateKeys">if set to <c>true</c> [update keys].</param>
        /// <returns>StringBuilder.</returns>
        public StringBuilder BulkUpdateQuery(string DBName, string buforName, string tableName, MemberRubric[] columns, MemberRubric[] keys, bool updateKeys = true)
        {
            StringBuilder sb = new StringBuilder();
            string bName = buforName;
            string tName = tableName;
            MemberRubric[] ic = columns;
            MemberRubric[] ik = keys;
            string dbName = DBName;
            sb.AppendLine(@"    ");
            sb.AppendFormat(@"UPDATE [{0}].[dbo].[" + tName + "] SET ", dbName);
            bool isUpdateCol = false;
            string delim = "";
            int c = 0;
            for (int i = 0; i < columns.Length; i++)
            {

                if (columns[i].RubricName.ToLower() == "updated")
                    isUpdateCol = true;

                if (c > 0)
                    delim = ",";
                sb.AppendFormat(CultureInfo.InvariantCulture,
                                @"{0}[{1}] =[S].[{2}]",
                                delim,
                                columns[i].RubricName,
                                columns[i].RubricName
                                );
                c++;
            }

            if (updateKeys)
            {
                if (columns.Length > 0)
                    delim = ",";
                else
                    delim = "";
                c = 0;
                for (int i = 0; i < keys.Length; i++)
                {
                    if (c > 0)
                        delim = ",";
                    sb.AppendFormat(CultureInfo.InvariantCulture,
                                    @"{0}[{1}] = [S].[{2}]",
                                    delim,
                                    keys[i].RubricName,
                                    keys[i].RubricName
                                    );
                    c++;
                }
            }
            sb.AppendLine(" OUTPUT inserted.*, deleted.* ");
            sb.AppendFormat(" FROM [tempdb].[dbo].[{0}] AS S INNER JOIN [{1}].[dbo].[{2}] AS T ", bName, dbName, tName);
            delim = "";
            c = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (i > 0)
                    delim = " AND ";
                else
                    delim = " ON ";

                sb.AppendFormat(CultureInfo.InvariantCulture,
                                @"{0} [T].[{1}] = [S].[{2}]",
                                delim,
                                keys[i].RubricName,
                                keys[i].RubricName
                                );
                c++;
            }
            sb.AppendLine("");
            sb.AppendLine(@"    ");
            return sb;
        }










        /// <summary>
        /// Simples the update.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="buildMapping">if set to <c>true</c> [build mapping].</param>
        /// <param name="updateKeys">if set to <c>true</c> [update keys].</param>
        /// <param name="updateExcept">The update except.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.Instant.Sqlset.SqlUpdate.SqlUpdateException"></exception>
        public int SimpleUpdate(IFigures table, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, int batchSize = 500)
        {
            try
            {
                IFigures tab = table;
                IList<FieldMapping> nMaps = new List<FieldMapping>();
                SqlAdapter afad = new SqlAdapter(_cn);
                StringBuilder sb = new StringBuilder();
                int intSqlset = 0;
                sb.AppendLine(@"    ");
                int count = 0;
                foreach (IFigure ir in table)
                {
                    if (ir.GetType().DeclaringType != tab.FigureType)
                    {
                        if (buildMapping)
                        {
                            SqlMapper imapper = new SqlMapper(tab);
                        }
                        nMaps = tab.Rubrics.Mappings;
                    }

                    foreach (FieldMapping nMap in nMaps)
                    {
                        IDeck<int> co = nMap.ColumnOrdinal;
                        IDeck<int> ko = nMap.KeyOrdinal;
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();
                        if (updateExcept != null)
                        {
                            ic = ic.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                            ik = ik.Where(c => !updateExcept.Contains(c.RubricName)).ToArray();
                        }

                        string qry = BatchUpdateQuery(ir, nMap.DbTableName, ic, ik, updateKeys).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");
                        intSqlset += afad.ExecuteUpdate(sb.ToString());
                        sb.Clear();
                        sb.AppendLine(@"    ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"    ");

                intSqlset += afad.ExecuteUpdate(sb.ToString());
                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlUpdateException(ex.ToString());
            }
        }











        /// <summary>
        /// Updates the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keysFromDeck">if set to <c>true</c> [keys from deck].</param>
        /// <param name="buildMapping">if set to <c>true</c> [build mapping].</param>
        /// <param name="updateKeys">if set to <c>true</c> [update keys].</param>
        /// <param name="updateExcept">The update except.</param>
        /// <param name="tempType">Type of the temporary.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        public IDeck<IDeck<IFigure>> Update(IFigures table, bool keysFromDeck = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkUpdate(table, keysFromDeck, buildMapping, updateKeys, updateExcept, tempType);
        }

        #endregion




        /// <summary>
        /// Class SqlUpdateException.
        /// Implements the <see cref="System.Exception" />
        /// </summary>
        /// <seealso cref="System.Exception" />
        public class SqlUpdateException : Exception
        {
            #region Constructors





            /// <summary>
            /// Initializes a new instance of the <see cref="SqlUpdateException" /> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public SqlUpdateException(string message)
                : base(message)
            {
            }

            #endregion
        }
    }
}
