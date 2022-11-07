
// <copyright file="SqlDbColumns.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Sqlset namespace.
/// </summary>
namespace System.Instant.Sqlset
{
    using System;
    using System.Collections.Generic;
    using System.Linq;




    /// <summary>
    /// Class DbColumn.
    /// </summary>
    public class DbColumn
    {
        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="DbColumn" /> class.
        /// </summary>
        public DbColumn()
        {
            isDBNull = false;
            isIdentity = false;
            isKey = false;
            isAutoincrement = false;
            MaxLength = -1;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName { get; set; }




        /// <summary>
        /// Gets or sets the size of the database column.
        /// </summary>
        /// <value>The size of the database column.</value>
        public int DbColumnSize { get; set; }




        /// <summary>
        /// Gets or sets the database ordinal.
        /// </summary>
        /// <value>The database ordinal.</value>
        public int DbOrdinal { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is autoincrement.
        /// </summary>
        /// <value><c>true</c> if this instance is autoincrement; otherwise, <c>false</c>.</value>
        public bool isAutoincrement { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is database null.
        /// </summary>
        /// <value><c>true</c> if this instance is database null; otherwise, <c>false</c>.</value>
        public bool isDBNull { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value><c>true</c> if this instance is identity; otherwise, <c>false</c>.</value>
        public bool isIdentity { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this instance is key.
        /// </summary>
        /// <value><c>true</c> if this instance is key; otherwise, <c>false</c>.</value>
        public bool isKey { get; set; }




        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public int MaxLength { get; set; }




        /// <summary>
        /// Gets or sets the rubrics.
        /// </summary>
        /// <value>The rubrics.</value>
        public List<MemberRubric> Rubrics { get; set; }




        /// <summary>
        /// Gets or sets the type of the rubric.
        /// </summary>
        /// <value>The type of the rubric.</value>
        public Type RubricType { get; set; }

        #endregion
    }

    /// <summary>
    /// Class DbColumns.
    /// </summary>
    public class DbColumns
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbColumns" /> class.
        /// </summary>
        public DbColumns()
        {
            list = new List<DbColumn>();
        }
        /// <summary>
        /// The list
        /// </summary>
        private List<DbColumn> list;
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>The list.</value>
        public List<DbColumn> List
        {
            get
            {
                return list;
            }
            set
            {
                list.AddRange(value.Where(c => !this.Have(c.ColumnName)).ToList());
            }
        }


        /// <summary>
        /// Adds the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        public void Add(DbColumn column) { if (!this.Have(column.ColumnName)) List.Add(column); }
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="_columns">The columns.</param>
        public void AddRange(List<DbColumn> _columns) { list.AddRange(_columns.Where(c => !this.Have(c.ColumnName)).ToList()); }
        /// <summary>
        /// Removes the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        public void Remove(DbColumn column) { list.Remove(column); }
        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index) { list.RemoveAt(index); }
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear() { List.Clear(); }
        /// <summary>
        /// Haves the specified column name.
        /// </summary>
        /// <param name="ColumnName">Name of the column.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Have(string ColumnName)
        {
            return list.Where(c => c.ColumnName == ColumnName).Any();
        }

        /// <summary>
        /// Gets the <see cref="DbColumn" /> with the specified column name.
        /// </summary>
        /// <param name="ColumnName">Name of the column.</param>
        /// <returns>DbColumn.</returns>
        public DbColumn this[string ColumnName]
        {
            get
            {
                return list.Where(c => ColumnName == c.ColumnName).First();
            }
        }
        /// <summary>
        /// Gets the <see cref="DbColumn" /> with the specified ordinal.
        /// </summary>
        /// <param name="Ordinal">The ordinal.</param>
        /// <returns>DbColumn.</returns>
        public DbColumn this[int Ordinal]
        {
            get
            {
                return list.Where(c => Ordinal == c.DbOrdinal).First();
            }
        }
        /// <summary>
        /// Gets the database column.
        /// </summary>
        /// <param name="ColumnName">Name of the column.</param>
        /// <returns>DbColumn.</returns>
        public DbColumn GetDbColumn(string ColumnName)
        {
            return list.Where(c => c.ColumnName == ColumnName).First();
        }
        /// <summary>
        /// Gets the database columns.
        /// </summary>
        /// <param name="ColumnNames">The column names.</param>
        /// <returns>DbColumn[].</returns>
        public DbColumn[] GetDbColumns(List<string> ColumnNames)
        {
            return list.Where(c => ColumnNames.Contains(c.ColumnName)).ToArray();
        }
        /// <summary>
        /// Gets the rubrics.
        /// </summary>
        /// <param name="ColumnNames">The column names.</param>
        /// <returns>List&lt;MemberRubric&gt;.</returns>
        public List<MemberRubric> GetRubrics(string ColumnNames)
        {
            return list.Where(c => ColumnNames == c.ColumnName).SelectMany(r => r.Rubrics).ToList();
        }
        /// <summary>
        /// Gets the rubrics.
        /// </summary>
        /// <param name="ColumnNames">The column names.</param>
        /// <returns>List&lt;MemberRubric&gt;.</returns>
        public List<MemberRubric> GetRubrics(List<string> ColumnNames)
        {
            return list.Where(c => ColumnNames.Contains(c.ColumnName)).SelectMany(r => r.Rubrics).ToList();
        }
    }
}
