
// <copyright file="SqlDelete.cs" company="UltimatR.Core">
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
    /// Class SqlDeleteException.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class SqlDeleteException : Exception
    {
        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDeleteException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SqlDeleteException(string message)
            : base(message)
        {
        }

        #endregion
    }




    /// <summary>
    /// Class SqlDelete.
    /// </summary>
    internal class SqlDelete
    {
        #region Fields

        /// <summary>
        /// The cn
        /// </summary>
        private SqlConnection _cn;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDelete" /> class.
        /// </summary>
        /// <param name="cn">The cn.</param>
        public SqlDelete(SqlConnection cn)
        {
            _cn = cn;
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDelete" /> class.
        /// </summary>
        /// <param name="cnstring">The cnstring.</param>
        public SqlDelete(string cnstring)
        {
            _cn = new SqlConnection(cnstring);
        }

        #endregion

        #region Methods








        /// <summary>
        /// Batches the delete.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="buildMapping">if set to <c>true</c> [build mapping].</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        /// <exception cref="System.Instant.Sqlset.SqlDeleteException"></exception>
        public IDeck<IDeck<IFigure>> BatchDelete(IFigures table, bool buildMapping, int batchSize = 1000)
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
                foreach (IFigure ir in tab)
                {

                    foreach (FieldMapping nMap in nMaps)
                    {
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchDeleteQuery(ir, nMap.DbTableName, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");
                        var bIFigures = afad.ExecuteDelete(sb.ToString(), tab);
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

                var rIFigures = afad.ExecuteDelete(sb.ToString(), tab);

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
                throw new SqlDeleteException(ex.ToString());
            }
        }







        /// <summary>
        /// Batches the delete.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        /// <exception cref="System.Instant.Sqlset.SqlDeleteException"></exception>
        public IDeck<IDeck<IFigure>> BatchDelete(IFigures table, int batchSize = 1000)
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
                foreach (IFigure ir in tab)
                {

                    foreach (FieldMapping nMap in nMaps)
                    {
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchDeleteQuery(ir, nMap.DbTableName, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");
                        var bIFigures = afad.ExecuteDelete(sb.ToString(), tab);
                        if (nSet.Count == 0)
                            nSet = bIFigures;
                        else
                            foreach (Album<IFigure> its in bIFigures.AsValues())
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

                var rIFigures = afad.ExecuteDelete(sb.ToString(), tab);

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
                throw new SqlDeleteException(ex.ToString());
            }
        }








        /// <summary>
        /// Batches the delete query.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>StringBuilder.</returns>
        public StringBuilder BatchDeleteQuery(IFigure card, string tableName, MemberRubric[] keys)
        {
            StringBuilder sb = new StringBuilder();
            string tName = tableName;
            IFigure ir = card;
            object[] ia = ir.ValueArray;
            MemberRubric[] ik = keys;

            sb.AppendLine(@"    ");
            sb.Append("DELETE FROM " + tableName + " OUTPUT deleted.* ");
            string delim = "";
            int c = 0;

            delim = "";
            c = 0;
            for (int i = 0; i < keys.Length; i++)
            {
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
        /// Bulks the delete.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keysFromDeckis">if set to <c>true</c> [keys from deckis].</param>
        /// <param name="buildMapping">if set to <c>true</c> [build mapping].</param>
        /// <param name="tempType">Type of the temporary.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        /// <exception cref="System.Instant.Sqlset.SqlDeleteException"></exception>
        public IDeck<IDeck<IFigure>> BulkDelete(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            try
            {
                IFigures tab = table;
                if (tab.Any())
                {
                    IList<FieldMapping> nMaps = new List<FieldMapping>();
                    if (buildMapping)
                    {
                        SqlMapper imapper = new SqlMapper(tab, keysFromDeckis);
                    }
                    nMaps = tab.Rubrics.Mappings;
                    string dbName = _cn.Database;
                    SqlAdapter adapter = new SqlAdapter(_cn);
                    adapter.DataBulk(tab, tab.FigureType.Name, tempType, BulkDbType.TempDB);
                    _cn.ChangeDatabase(dbName);
                    IDeck<IDeck<IFigure>> nSet = new Album<IDeck<IFigure>>();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(@"    ");
                    foreach (FieldMapping nMap in nMaps)
                    {
                        sb.AppendLine(@"    ");

                        string qry = BulkDeleteQuery(dbName, tab.FigureType.Name, nMap.DbTableName).ToString();
                        sb.Append(qry);

                        sb.AppendLine(@"    ");
                    }
                    sb.AppendLine(@"    ");

                    IDeck<IDeck<IFigure>> bIFigures = adapter.ExecuteDelete(sb.ToString(), tab, true);


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
                throw new SqlDeleteException(ex.ToString());
            }
        }








        /// <summary>
        /// Bulks the delete query.
        /// </summary>
        /// <param name="DBName">Name of the database.</param>
        /// <param name="buforName">Name of the bufor.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>StringBuilder.</returns>
        public StringBuilder BulkDeleteQuery(string DBName, string buforName, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            string bName = buforName;
            string tName = tableName;
            string dbName = DBName;
            sb.AppendLine(@"  ");
            sb.AppendFormat(@"DELETE FROM [{0}].[dbo].[" + tName + "] OUTPUT deleted.* WHERE EXISTS (", dbName);
            sb.AppendFormat("SELECT * FROM [tempdb].[dbo].[{0}] AS S)", bName);
            sb.AppendLine("");
            sb.AppendLine(@"    ");
            return sb;
        }









        /// <summary>
        /// Deletes the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="keysFromDeckis">if set to <c>true</c> [keys from deckis].</param>
        /// <param name="buildMapping">if set to <c>true</c> [build mapping].</param>
        /// <param name="tempType">Type of the temporary.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        public IDeck<IDeck<IFigure>> Delete(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkDelete(table, keysFromDeckis, buildMapping, tempType);
        }








        /// <summary>
        /// Simples the delete.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="buildMapping">if set to <c>true</c> [build mapping].</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.Instant.Sqlset.SqlDeleteException"></exception>
        public int SimpleDelete(IFigures table, bool buildMapping, int batchSize = 1000)
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
                foreach (IFigure ir in tab)
                {

                    foreach (FieldMapping nMap in nMaps)
                    {
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchDeleteQuery(ir, nMap.DbTableName, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");

                        intSqlset += afad.ExecuteDelete(sb.ToString());

                        sb.Clear();
                        sb.AppendLine(@"    ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"    ");

                intSqlset += afad.ExecuteDelete(sb.ToString());
                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlDeleteException(ex.ToString());
            }
        }







        /// <summary>
        /// Simples the delete.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.Instant.Sqlset.SqlDeleteException"></exception>
        public int SimpleDelete(IFigures table, int batchSize = 1000)
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
                foreach (IFigure ir in tab)
                {

                    foreach (FieldMapping nMap in nMaps)
                    {
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchDeleteQuery(ir, nMap.DbTableName, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");
                        intSqlset += afad.ExecuteDelete(sb.ToString());

                        sb.Clear();
                        sb.AppendLine(@"    ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"    ");

                intSqlset += afad.ExecuteDelete(sb.ToString());

                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlDeleteException(ex.ToString());
            }
        }

        #endregion
    }
}
