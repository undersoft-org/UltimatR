﻿
namespace System.Instant.Sqlset
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Series;
    using System.Text;

    public class SqlInsert
    {
        #region Fields

        private SqlConnection _cn;

        #endregion

        #region Constructors

        public SqlInsert(SqlConnection cn)
        {
            _cn = cn;
        }

        public SqlInsert(string cnstring)
        {
            _cn = new SqlConnection(cnstring);
        }

        #endregion

        #region Methods

        public IDeck<IDeck<IFigure>> BatchInsert(IFigures table, bool buildMapping, int batchSize = 1000)
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
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchInsertQuery(ir, nMap.DbTableName, ic, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");
                        var bIFigures = afad.ExecuteInsert(sb.ToString(), tab);
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

                var rIFigures = afad.ExecuteInsert(sb.ToString(), tab);

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
                throw new SqlInsertException(ex.ToString());
            }
        }

        public IDeck<IDeck<IFigure>> BatchInsert(IFigures table, int batchSize = 1000)
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
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchInsertQuery(ir, nMap.DbTableName, ic, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");
                        var bIFigures = afad.ExecuteInsert(sb.ToString(), tab);
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

                var rIFigures = afad.ExecuteInsert(sb.ToString(), tab);

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
                throw new SqlInsertException(ex.ToString());
            }
        }

        public StringBuilder BatchInsertQuery(IFigure card, string tableName, MemberRubric[] columns, MemberRubric[] keys, bool updateKeys = true)
        {
            StringBuilder sbCols = new StringBuilder(), sbVals = new StringBuilder(), sbQry = new StringBuilder();
            string tName = tableName;
            IFigure ir = card;
            object[] ia = ir.ValueArray;
            MemberRubric[] ic = columns;
            MemberRubric[] ik = keys;

            sbCols.AppendLine(@"    ");
            sbCols.Append("INSERT INTO " + tableName + " (");
            sbVals.Append(@") OUTPUT inserted.* VALUES (");
            bool isUpdateCol = false;
            string delim = "";
            int c = 0;
            for (int i = 0; i < columns.Length; i++)
            {


                if (columns[i].RubricName.ToLower() == "updated")
                    isUpdateCol = true;
                if (ia[columns[i].FieldId] != DBNull.Value && !columns[i].IsIdentity)
                {
                    if (c > 0)
                        delim = ",";
                    sbCols.AppendFormat(CultureInfo.InvariantCulture, @"{0}[{1}]", delim,
                                                                                   columns[i].RubricName
                                                                                   );
                    sbVals.AppendFormat(CultureInfo.InvariantCulture, @"{0} {1}{2}{1}", delim,
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
            {
                sbCols.AppendFormat(CultureInfo.InvariantCulture, ", [updated]", DateTime.Now);
                sbVals.AppendFormat(CultureInfo.InvariantCulture, ", '{0}'", DateTime.Now);
            }
            if (columns.Length > 0)
                delim = ",";
            else
                delim = "";
            c = 0;
            for (int i = 0; i < keys.Length; i++)
            {

                if (ia[keys[i].FieldId] != DBNull.Value && !keys[i].IsIdentity)
                {
                    if (c > 0)
                        delim = ",";
                    sbCols.AppendFormat(CultureInfo.InvariantCulture, @"{0}[{1}]", delim,
                                                                                    keys[i].RubricName
                                                                                    );
                    sbVals.AppendFormat(CultureInfo.InvariantCulture, @"{0} {1}{2}{1}", delim,
                                                                                        (keys[i].RubricType == typeof(string) ||
                                                                                        keys[i].RubricType == typeof(DateTime)) ? "'" : "",
                                                                                        (keys[i].RubricType != typeof(string)) ?
                                                                                        Convert.ChangeType(ia[keys[i].FieldId], keys[i].RubricType) :
                                                                                        ia[keys[i].FieldId].ToString().Replace("'", "''")
                                                                                        );
                    c++;
                }
            }
            sbQry.Append(sbCols.ToString() + sbVals.ToString() + ") ");
            sbQry.AppendLine(@"    ");
            return sbQry;
        }

        public IDeck<IDeck<IFigure>> BulkInsert(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
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

                        string qry = BulkInsertQuery(dbName, tab.FigureType.Name, nMap.DbTableName, ic, ik, updateKeys).ToString();
                        sb.Append(qry);
                        sb.AppendLine(@"    ");
                    }
                    sb.AppendLine(@"    ");

                    IDeck<IDeck<IFigure>> bIFigures = afad.ExecuteInsert(sb.ToString(), tab, true);


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
                throw new SqlInsertException(ex.ToString());
            }
        }

        public StringBuilder BulkInsertQuery(string DBName, string buforName, string tableName, MemberRubric[] columns, MemberRubric[] keys, bool updateKeys = true)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbv = new StringBuilder();
            string bName = buforName;
            string tName = tableName;
            MemberRubric[] rubrics = keys.Concat(columns).ToArray();
            string dbName = DBName;
            sb.AppendLine(@"  ");
            sb.AppendFormat(@"INSERT INTO [{0}].[dbo].[" + tName + "] (", dbName);
            sbv.Append(@"SELECT ");
            bool isUpdateCol = false;
            string delim = "";
            int c = 0;
            for (int i = 0; i < rubrics.Length; i++)
            {

                if (rubrics[i].RubricName.ToLower() == "updated")
                    isUpdateCol = true;

                if (c > 0)
                    delim = ",";
                sb.AppendFormat(CultureInfo.InvariantCulture, @"{0}[{1}]", delim, rubrics[i].RubricName);
                sbv.AppendFormat(CultureInfo.InvariantCulture, @"{0}[S].[{1}]", delim, rubrics[i].RubricName);
                c++;
            }
            sb.AppendFormat(CultureInfo.InvariantCulture, @") OUTPUT inserted.* {0}", sbv.ToString());
            sb.AppendFormat(" FROM [tempdb].[dbo].[{0}] AS S ", bName, dbName, tName);
            sb.AppendLine("");
            sb.AppendLine(@"    ");
            sbv.Clear();
            return sb;
        }

        public IDeck<IDeck<IFigure>> Insert(IFigures table, bool keysFromDeckis = false, bool buildMapping = false, bool updateKeys = false, string[] updateExcept = null, BulkPrepareType tempType = BulkPrepareType.Trunc)
        {
            return BulkInsert(table, keysFromDeckis, buildMapping, updateKeys, updateExcept, tempType);
        }

        public int SimpleInsert(IFigures table, bool buildMapping, int batchSize = 1000)
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
                        ;
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchInsertQuery(ir, nMap.DbTableName, ic, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");

                        intSqlset += afad.ExecuteInsert(sb.ToString());

                        sb.Clear();
                        sb.AppendLine(@"    ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"    ");

                intSqlset += afad.ExecuteInsert(sb.ToString());
                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlInsertException(ex.ToString());
            }
        }

        public int SimpleInsert(IFigures table, int batchSize = 1000)
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
                        MemberRubric[] ic = tab.Rubrics.AsValues().Where(c => nMap.ColumnOrdinal.Contains(c.FieldId)).ToArray();
                        MemberRubric[] ik = tab.Rubrics.AsValues().Where(c => nMap.KeyOrdinal.Contains(c.FieldId)).ToArray();

                        string qry = BatchInsertQuery(ir, nMap.DbTableName, ic, ik).ToString();
                        sb.Append(qry);
                        count++;
                    }
                    if (count >= batchSize)
                    {
                        sb.AppendLine(@"    ");
                        intSqlset += afad.ExecuteInsert(sb.ToString());

                        sb.Clear();
                        sb.AppendLine(@"    ");
                        count = 0;
                    }
                }
                sb.AppendLine(@"    ");

                intSqlset += afad.ExecuteInsert(sb.ToString());

                return intSqlset;
            }
            catch (SqlException ex)
            {
                _cn.Close();
                throw new SqlInsertException(ex.ToString());
            }
        }

        #endregion
    }

    public class SqlInsertException : Exception
    {
        #region Constructors

        public SqlInsertException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
