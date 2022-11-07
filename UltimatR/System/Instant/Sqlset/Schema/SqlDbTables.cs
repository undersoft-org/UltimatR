
// <copyright file="SqlDbTables.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Sqlset namespace.
/// </summary>
namespace System.Instant.Sqlset
{
    using System.Collections.Generic;
    using System.Linq;




    /// <summary>
    /// Class DbTable.
    /// </summary>
    public class DbTable
    {
        #region Fields

        /// <summary>
        /// The database primary key
        /// </summary>
        private DbColumn[] dbPrimaryKey;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="DbTable" /> class.
        /// </summary>
        public DbTable()
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="DbTable" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public DbTable(string tableName)
        {
            TableName = tableName;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the data database columns.
        /// </summary>
        /// <value>The data database columns.</value>
        public DbColumns DataDbColumns { get; set; }




        /// <summary>
        /// Gets or sets the database primary key.
        /// </summary>
        /// <value>The database primary key.</value>
        public DbColumn[] DbPrimaryKey
        {
            get
            {
                return dbPrimaryKey;
            }
            set
            {
                dbPrimaryKey = value;
            }
        }




        /// <summary>
        /// Gets the get columns for data table.
        /// </summary>
        /// <value>The get columns for data table.</value>
        public List<MemberRubric> GetColumnsForDataTable
        {
            get
            {
                return DataDbColumns.List.Select(c =>
                    new MemberRubric(new FieldRubric(c.RubricType, c.ColumnName, c.DbColumnSize, c.DbOrdinal) { RubricSize = c.DbColumnSize })
                    {
                        FieldId = c.DbOrdinal - 1,
                        IsAutoincrement = c.isAutoincrement,
                        IsDBNull = c.isDBNull,
                        IsIdentity = c.isIdentity
                    }).ToList();
            }
        }




        /// <summary>
        /// Gets the get key for data table.
        /// </summary>
        /// <value>The get key for data table.</value>
        public MemberRubric[] GetKeyForDataTable
        {
            get
            {
                return DbPrimaryKey.Select(c =>
                                                 new MemberRubric(new FieldRubric(c.RubricType, c.ColumnName, c.DbColumnSize, c.DbOrdinal) { RubricSize = c.DbColumnSize })
                                                 {
                                                     FieldId = c.DbOrdinal - 1,
                                                     IsAutoincrement = c.isAutoincrement,
                                                     IsDBNull = c.isDBNull,
                                                     IsIdentity = c.isIdentity
                                                 }).ToArray();
            }
        }




        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName { get; set; }

        #endregion
    }

    /// <summary>
    /// Class DbTables.
    /// </summary>
    public class DbTables
    {
        /// <summary>
        /// The holder
        /// </summary>
        private object holder = new object();
        /// <summary>
        /// Gets the holder.
        /// </summary>
        /// <value>The holder.</value>
        public object Holder
        {
            get
            {
                return holder;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTables" /> class.
        /// </summary>
        public DbTables()
        {
            tables = new List<DbTable>();
        }
        /// <summary>
        /// The tables
        /// </summary>
        private List<DbTable> tables;
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>The list.</value>
        public List<DbTable> List
        {
            get
            {
                return
                    tables;
            }
            set
            {
                tables.AddRange(value.Where(c => !this.Have(c.TableName)).ToList());
            }
        }

        /// <summary>
        /// Adds the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        public void Add(DbTable table) { if (!this.Have(table.TableName)) { tables.Add(table); } }
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="_tables">The tables.</param>
        public void AddRange(List<DbTable> _tables) { tables.AddRange(_tables.Where(c => !this.Have(c.TableName)).ToList()); }
        /// <summary>
        /// Removes the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        public void Remove(DbTable table) { tables.Remove(table); }
        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index) { tables.RemoveAt(index); }
        /// <summary>
        /// Haves the specified table name.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Have(string TableName)
        {
            return tables.Where(t => t.TableName == TableName).Any();
        }
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear() { tables.Clear(); }

        /// <summary>
        /// Gets the <see cref="DbTable" /> with the specified table name.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns>DbTable.</returns>
        public DbTable this[string TableName]
        {
            get
            {
                return tables.Where(c => TableName == c.TableName).First();
            }
        }
        /// <summary>
        /// Gets the <see cref="DbTable" /> with the specified table index.
        /// </summary>
        /// <param name="TableIndex">Index of the table.</param>
        /// <returns>DbTable.</returns>
        public DbTable this[int TableIndex]
        {
            get
            {
                return tables[TableIndex];
            }
        }
        /// <summary>
        /// Gets the database table.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns>DbTable.</returns>
        public DbTable GetDbTable(string TableName)
        {
            return tables.Where(c => TableName == c.TableName).First();
        }
        /// <summary>
        /// Gets the database tables.
        /// </summary>
        /// <param name="TableNames">The table names.</param>
        /// <returns>List&lt;DbTable&gt;.</returns>
        public List<DbTable> GetDbTables(List<string> TableNames)
        {
            return tables.Where(c => TableNames.Contains(c.TableName)).ToList();
        }

    }
}
