
// <copyright file="SqlReader.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Sqlset namespace.
/// </summary>
namespace System.Instant.Sqlset
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Series;
    using System.Uniques;





    /// <summary>
    /// Interface IDataReader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataReader<T> where T : class
    {
        #region Methods






        /// <summary>
        /// Deletes the read.
        /// </summary>
        /// <param name="toInsertCards">To insert cards.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        IDeck<IDeck<IFigure>> DeleteRead(IFigures toInsertCards);







        /// <summary>
        /// Injects the read.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="keyNames">The key names.</param>
        /// <returns>IFigures.</returns>
        IFigures InjectRead(string tableName, IDeck<string> keyNames = null);






        /// <summary>
        /// Inserts the read.
        /// </summary>
        /// <param name="toInsertCards">To insert cards.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        IDeck<IDeck<IFigure>> InsertRead(IFigures toInsertCards);






        /// <summary>
        /// Updates the read.
        /// </summary>
        /// <param name="toUpdateCards">To update cards.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        IDeck<IDeck<IFigure>> UpdateRead(IFigures toUpdateCards);

        #endregion
    }




    /// <summary>
    /// Class SqlReader.
    /// Implements the <see cref="System.Instant.Sqlset.IDataReader{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Instant.Sqlset.IDataReader{T}" />
    public class SqlReader<T> : IDataReader<T> where T : class
    {
        #region Fields

        /// <summary>
        /// The dr
        /// </summary>
        private IDataReader dr;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="SqlReader{T}" /> class.
        /// </summary>
        /// <param name="_dr">The dr.</param>
        public SqlReader(IDataReader _dr)
        {
            dr = _dr;
        }

        #endregion

        #region Methods








        /// <summary>
        /// Decks from schema.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="operColumns">The oper columns.</param>
        /// <param name="insAndDel">if set to <c>true</c> [ins and delete].</param>
        /// <returns>IFigures.</returns>
        public IFigures DeckFromSchema(DataTable schema, IDeck<MemberRubric> operColumns, bool insAndDel = false)
        {

            List<MemberRubric> columns = new List<MemberRubric>(schema.Rows.Cast<DataRow>().AsEnumerable().AsQueryable()
                                                .Select(c => new MemberRubric(new FieldRubric(Type.GetType(c["DataType"].ToString()),
                                                                        c["ColumnName"].ToString(),
                                                                        Convert.ToInt32(c["ColumnSize"]),
                                                                        Convert.ToInt32(c["ColumnOrdinal"]))
                                                {
                                                    RubricSize = Convert.ToInt32(c["ColumnSize"])
                                                })
                                                {
                                                    FieldId = Convert.ToInt32(c["ColumnOrdinal"]),
                                                    IsIdentity = Convert.ToBoolean(c["IsIdentity"]),
                                                    IsAutoincrement = Convert.ToBoolean(c["IsAutoincrement"]),
                                                    IsDBNull = Convert.ToBoolean(c["AllowDBNull"])

                                                }).ToList());

            List<MemberRubric> _columns = new List<MemberRubric>();

            if (insAndDel)
                for (int i = 0; i < (int)(columns.Count / 2); i++)
                    _columns.Add(columns[i]);
            else
                _columns.AddRange(columns);

            Figure rt = new Figure(_columns.ToArray(), "SchemaFigure");
            Figures tab = new Figures(rt, "Schema");
            IFigures deck = tab.Combine();

            List<DbTable> dbtabs = DbHand.Schema.DataDbTables.List;
            MemberRubric[] pKeys = columns.Where(c => dbtabs.SelectMany(t => t.GetKeyForDataTable
                                          .Select(d => d.RubricName))
                                          .Contains(c.RubricName) && operColumns
                                          .Select(o => o.RubricName)
                                          .Contains(c.RubricName)).ToArray();
            if (pKeys.Length > 0)
                deck.Rubrics.KeyRubrics = new MemberRubrics(pKeys);
            deck.Rubrics.Update();
            return deck;
        }






        /// <summary>
        /// Deletes the read.
        /// </summary>
        /// <param name="toDeleteCards">To delete cards.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        public IDeck<IDeck<IFigure>> DeleteRead(IFigures toDeleteCards)
        {
            IFigures deck = toDeleteCards;
            IDeck<IFigure> deletedList = new Album<IFigure>();
            IDeck<IFigure> brokenList = new Album<IFigure>();

            int i = 0;
            do
            {
                int columnsCount = deck.Rubrics.Count;

                if (i == 0 && deck.Rubrics.Count == 0)
                {
                    IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                    deck = tab;
                    columnsCount = deck.Rubrics.Count;
                }
                object[] itemArray = new object[columnsCount];
                int[] keyIndexes = deck.Rubrics.KeyRubrics.Ordinals;
                while (dr.Read())
                {
                    if ((columnsCount - 1) != dr.FieldCount)
                    {
                        IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                        deck = tab;
                        columnsCount = deck.Rubrics.Count;
                        itemArray = new object[columnsCount];
                        keyIndexes = deck.Rubrics.KeyRubrics.Ordinals;
                    }

                    dr.GetValues(itemArray);

                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().Default() : a).ToArray();

                    

                    deletedList.Add(row);
                }

                foreach (IFigure ir in toDeleteCards)
                    if (!deletedList.ContainsKey(ir))
                        brokenList.Add(ir);

            } while (dr.NextResult());

            IDeck<IDeck<IFigure>> iSet = new Album<IDeck<IFigure>>();

            iSet.Add("Failed", brokenList);

            iSet.Add("Deleted", deletedList);

            return iSet;
        }







        /// <summary>
        /// Injects the read.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="keyNames">The key names.</param>
        /// <returns>IFigures.</returns>
        public IFigures InjectRead(string tableName, IDeck<string> keyNames = null)
        {
            DataTable schema = dr.GetSchemaTable();
            List<MemberRubric> columns = new List<MemberRubric>(schema.Rows.Cast<DataRow>().AsEnumerable().AsQueryable()
                                                .Where(n => n["ColumnName"].ToString() != "SerialCode").Select(c =>
                                                new MemberRubric(new FieldRubric(Type.GetType(c["DataType"].ToString()),
                                                                        c["ColumnName"].ToString(),
                                                                        Convert.ToInt32(c["ColumnSize"]),
                                                                        Convert.ToInt32(c["ColumnOrdinal"]))
                                                {
                                                    RubricSize = Convert.ToInt32(c["ColumnSize"])
                                                })
                                                {
                                                    FieldId = Convert.ToInt32(c["ColumnOrdinal"]),
                                                    IsIdentity = Convert.ToBoolean(c["IsIdentity"]),
                                                    IsAutoincrement = Convert.ToBoolean(c["IsAutoincrement"]),
                                                    IsDBNull = Convert.ToBoolean(c["AllowDBNull"]),
                                                }
                                                ).ToList());



            bool takeDbKeys = false;
            if (keyNames != null)
                if (keyNames.Count > 0)
                    foreach (var k in keyNames)
                    {
                        columns.Where(c => c.Name == k)
                               .Select(ck => ck.IsKey = true)
                               .ToArray();
                    }
                else
                    takeDbKeys = true;
            else
                takeDbKeys = true;

            if (takeDbKeys &&
                 DbHand.Schema != null &&
                    DbHand.Schema.DataDbTables.List.Count > 0)
            {
                List<DbTable> dbtabs = DbHand.Schema.DataDbTables.List;
                MemberRubric[] pKeys = columns
                        .Where(c => dbtabs
                        .SelectMany(t => t.GetKeyForDataTable
                        .Select(d => d.RubricName))
                        .Contains(c.RubricName))
                        .ToArray();

                if (pKeys.Length > 0)
                {
                    pKeys.Select(pk => pk.IsKey = true);
                }
            }

            Figure rt = new Figure(columns.ToArray(), tableName);
            Figures deck = new Figures(rt, tableName + "_Figures");
            IFigures tab = deck.Combine();

            if (dr.Read())
            {
                int columnsCount = dr.FieldCount;
                object[] itemArray = new object[columnsCount];
                int[] keyOrder = tab.Rubrics.KeyRubrics.Ordinals;

                do
                {
                    IFigure figure = tab.NewFigure();

                    dr.GetValues(itemArray);

                    figure.ValueArray = itemArray.Select((a, y) => 
                                        itemArray[y] = (a == DBNull.Value) 
                                      ? a.GetType().Default() 
                                      : a).ToArray();

                    figure.UniqueKey = keyOrder.Select(i => itemArray[i])
                                               .ToArray().UniqueKey64();

                    tab.Put(figure);
                }
                while (dr.Read());
                itemArray = null;
            }
            dr.Dispose();
            return tab;
        }






        /// <summary>
        /// Inserts the read.
        /// </summary>
        /// <param name="toInsertCards">To insert cards.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        public IDeck<IDeck<IFigure>> InsertRead(IFigures toInsertCards)
        {
            IFigures deck = toInsertCards;
            IDeck<IFigure> insertedList = new Album<IFigure>();
            IDeck<IFigure> brokenList = new Album<IFigure>();

            int i = 0;
            do
            {
                int columnsCount = deck.Rubrics.Count;

                if (i == 0 && deck.Rubrics.Count == 0)
                {
                    IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                    deck = tab;
                    columnsCount = deck.Rubrics.Count;
                }
                object[] itemArray = new object[columnsCount];
                int[] keyIndexes = deck.Rubrics.KeyRubrics.Ordinals;
                while (dr.Read())
                {
                    if ((columnsCount - 1) != dr.FieldCount)
                    {
                        IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics);
                        deck = tab;
                        columnsCount = deck.Rubrics.Count;
                        itemArray = new object[columnsCount];
                        keyIndexes = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FieldId).ToArray();
                    }

                    dr.GetValues(itemArray);

                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().Default() : a).ToArray();

                    

                    insertedList.Add(row);
                }

                foreach (IFigure ir in toInsertCards)
                    if (!insertedList.ContainsKey(ir))
                        brokenList.Add(ir);

            } while (dr.NextResult());

            IDeck<IDeck<IFigure>> iSet = new Album<IDeck<IFigure>>();

            iSet.Add("Failed", brokenList);

            iSet.Add("Inserted", insertedList);

            return iSet;
        }






        /// <summary>
        /// Updates the read.
        /// </summary>
        /// <param name="toUpdateCards">To update cards.</param>
        /// <returns>IDeck&lt;IDeck&lt;IFigure&gt;&gt;.</returns>
        public IDeck<IDeck<IFigure>> UpdateRead(IFigures toUpdateCards)
        {
            IFigures deck = toUpdateCards;
            IDeck<IFigure> updatedList = new Album<IFigure>();
            IDeck<IFigure> toInsertList = new Album<IFigure>();

            int i = 0;
            do
            {
                int columnsCount = deck.Rubrics != null ? deck.Rubrics.Count : 0;

                if (i == 0 && columnsCount == 0)
                {
                    IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics, true);
                    deck = tab;
                    columnsCount = deck.Rubrics.Count;
                }
                object[] itemArray = new object[columnsCount];
                int[] keyOrder = deck.Rubrics.KeyRubrics.Ordinals;
                while (dr.Read())
                {
                    if ((columnsCount - 1) != (int)(dr.FieldCount / 2))
                    {
                        IFigures tab = DeckFromSchema(dr.GetSchemaTable(), deck.Rubrics.KeyRubrics, true);
                        deck = tab;
                        columnsCount = deck.Rubrics.Count;
                        itemArray = new object[columnsCount];
                        keyOrder = deck.Rubrics.KeyRubrics.AsValues().Select(k => k.FieldId).ToArray();
                    }

                    dr.GetValues(itemArray);

                    IFigure row = deck.NewFigure();

                    row.ValueArray = itemArray.Select((a, y) => itemArray[y] = (a == DBNull.Value) ? a.GetType().Default() : a).ToArray();

                    

                    updatedList.Add(row);
                }

                foreach (IFigure ir in toUpdateCards)
                    if (!updatedList.ContainsKey(ir))
                        toInsertList.Add(ir);

            } while (dr.NextResult());

            IDeck<IDeck<IFigure>> iSet = new Album<IDeck<IFigure>>();

            iSet.Add("Failed", toInsertList);

            iSet.Add("Updated", updatedList);

            return iSet;
        }

        #endregion
    }
}
