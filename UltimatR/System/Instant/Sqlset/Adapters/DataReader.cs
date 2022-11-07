
// <copyright file="DataReader.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Sqlset namespace.
/// </summary>
namespace System.Instant.Sqlset
{
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Uniques;

    /// <summary>
    /// Class DataReader.
    /// Implements the <see cref="System.Data.IDataReader" />
    /// </summary>
    /// <seealso cref="System.Data.IDataReader" />
    public class DataReader : IDataReader
    {

        /// <summary>
        /// The m f open
        /// </summary>
        private bool m_fOpen = true;

        /// <summary>
        /// The m resultset
        /// </summary>
        private IRubrics m_resultset;
        /// <summary>
        /// The m values
        /// </summary>
        private object[][] m_values;
        /// <summary>
        /// The y size
        /// </summary>
        private int y_size;
        /// <summary>
        /// The x size
        /// </summary>
        private int x_size;
        /// <summary>
        /// The m startpos
        /// </summary>
        private static int m_STARTPOS = -1;
        /// <summary>
        /// The m n position
        /// </summary>
        private int m_nPos = m_STARTPOS;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReader" /> class.
        /// </summary>
        /// <param name="resultset">The resultset.</param>
        public DataReader(IFigures resultset)
        {
            m_resultset = resultset.Rubrics;
            x_size = resultset.Rubrics.Count;
            m_values = resultset.Select(p =>
                                    new object[] { p.SerialCode }
                                            .Concat(p.ValueArray).ToArray()).ToArray();
            y_size = m_values.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReader" /> class.
        /// </summary>
        /// <param name="resultset">The resultset.</param>
        public DataReader(FigureCard[] resultset)
        {
            if (resultset.Length > 0)
            {
                m_resultset = resultset.First().Figures.Rubrics;
                x_size = m_resultset.Count;
                m_values = resultset.Select(p =>
                                   new object[] { p.SerialCode }
                                           .Concat(p.ValueArray).ToArray()).ToArray();
                y_size = m_values.Length;
            }
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        public bool IsClosed
        {

            get { return !m_fOpen; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        /// <value>The records affected.</value>
        public int RecordsAffected
        {

            get { return -1; }
        }

        /// <summary>
        /// Closes the <see cref="T:System.Data.IDataReader" /> Object.
        /// </summary>
        public void Close()
        {
            m_fOpen = false;
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch SQL statements.
        /// </summary>
        /// <returns><see langword="true" /> if there are more rows; otherwise, <see langword="false" />.</returns>
        public bool NextResult()
        {

            return false;
        }

        /// <summary>
        /// Advances the <see cref="T:System.Data.IDataReader" /> to the next record.
        /// </summary>
        /// <returns><see langword="true" /> if there are more rows; otherwise, <see langword="false" />.</returns>
        public bool Read()
        {

            if (++m_nPos >= y_size)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Returns a <see cref="T:System.Data.DataTable" /> that describes the column metadata of the <see cref="T:System.Data.IDataReader" />.
        /// </summary>
        /// <returns>A <see cref="T:System.Data.DataTable" /> that describes the column metadata.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public DataTable GetSchemaTable()
        {
            
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>The field count.</value>
        public int FieldCount
        {

            get { return x_size; }
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        public string GetName(int i)
        {
            return m_resultset[i].RubricName;
        }

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The data type information for the specified field.</returns>
        public string GetDataTypeName(int i)
        {
            return m_resultset[i].RubricType.Name;
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.</returns>
        public Type GetFieldType(int i)
        {
            
            return m_resultset[i].RubricType;
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Object" /> which will contain the field value upon return.</returns>
        public Object GetValue(int i)
        {
            object o = m_values[m_nPos][i];
            if (o is Usid)
                return ((Usid)o).UniqueKey;
            else if (o is Ussn)
                return ((Ussn)o).GetBytes();
            else if (o is DateTime)
                if ((DateTime)o == DateTime.MinValue)
                    o = DateTime.FromBinary(599581440000000000);
            return o;
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="values">An array of <see cref="T:System.Object" /> to copy the attribute fields into.</param>
        /// <returns>The number of instances of <see cref="T:System.Object" /> in the array.</returns>
        public int GetValues(object[] values)
        {
            int i = 0, j = 0;
            for (; i < values.Length && j < x_size; i++, j++)
            {
                object o = m_values[m_nPos][j];
                if (o is Usid)
                    values[i] = ((Usid)o).UniqueKey;
                else if (o is Ussn)
                    values[i] = ((Ussn)o).GetBytes();
                else
                    values[i] = o;
            }
            return i;
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Could not find specified column in results</exception>
        public int GetOrdinal(string name)
        {
            
            for (int i = 0; i < x_size; i++)
            {
                if (0 == _cultureAwareCompare(name, m_resultset[i].RubricName))
                {
                    return i;
                }
            }

            
            throw new IndexOutOfRangeException("Could not find specified column in results");
        }

        /// <summary>
        /// Gets the <see cref="System.Object" /> with the specified i.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>System.Object.</returns>
        public object this[int i]
        {
            get
            {
                object o = m_values[m_nPos][i];
                if (o is Usid)
                    return ((Usid)o).UniqueKey;
                else if (o is Ussn)
                    return ((Ussn)o).GetBytes();
                else
                    return o;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object" /> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        public object this[string name]
        {
            get
            {
                object o = this[GetOrdinal(name)];
                if (o is Usid)
                    return ((Usid)o).UniqueKey;
                else if (o is Ussn)
                    return ((Ussn)o).GetBytes();
                else
                    return o;
            }
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public bool GetBoolean(int i)
        {
            return (bool)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
        public byte GetByte(int i)
        {
            return (byte)m_values[m_nPos][i];
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for <paramref name="buffer" /> to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        /// <exception cref="System.NotSupportedException">GetBytes not supported.</exception>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException("GetBytes not supported.");
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The character value of the specified column.</returns>
        public char GetChar(int i)
        {
            return (char)m_values[m_nPos][i];
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldoffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for <paramref name="buffer" /> to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        /// <exception cref="System.NotSupportedException">GetChars not supported.</exception>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException("GetChars not supported.");
        }

        /// <summary>
        /// Returns the GUID value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The GUID value of the specified field.</returns>
        public Guid GetGuid(int i)
        {
            return (Guid)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        public Int16 GetInt16(int i)
        {
            return (Int16)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 32-bit signed integer value of the specified field.</returns>
        public Int32 GetInt32(int i)
        {
            return (Int32)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 64-bit signed integer value of the specified field.</returns>
        public Int64 GetInt64(int i)
        {
            return (Int64)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The single-precision floating point number of the specified field.</returns>
        public float GetFloat(int i)
        {

            return (float)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The double-precision floating point number of the specified field.</returns>
        public double GetDouble(int i)
        {

            return (double)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        public string GetString(int i)
        {

            return (string)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The fixed-position numeric value of the specified field.</returns>
        public Decimal GetDecimal(int i)
        {

            return (decimal)m_values[m_nPos][i];
        }

        /// <summary>
        /// Gets the date and time data value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The date and time data value of the specified field.</returns>
        public DateTime GetDateTime(int i)
        {

            return (DateTime)m_values[m_nPos][i];
        }

        /// <summary>
        /// Returns an <see cref="T:System.Data.IDataReader" /> for the specified column ordinal.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Data.IDataReader" /> for the specified column ordinal.</returns>
        /// <exception cref="System.NotSupportedException">GetData not supported.</exception>
        public IDataReader GetData(int i)
        {

            throw new NotSupportedException("GetData not supported.");
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns><see langword="true" /> if the specified field is set to null; otherwise, <see langword="false" />.</returns>
        public bool IsDBNull(int i)
        {
            return m_values[m_nPos][i] == DBNull.Value;
        }

        /// <summary>
        /// Cultures the aware compare.
        /// </summary>
        /// <param name="strA">The string a.</param>
        /// <param name="strB">The string b.</param>
        /// <returns>System.Int32.</returns>
        private int _cultureAwareCompare(string strA, string strB)
        {
            return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <exception cref="System.SystemException">An exception of type " + e.GetType() +
        ///                                               " was encountered while closing the TemplateDataReader.</exception>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    m_values = null;
                    m_resultset = null;
                    this.Close();
                }
                catch (Exception e)
                {
                    throw new SystemException("An exception of type " + e.GetType() +
                                              " was encountered while closing the TemplateDataReader.");
                }
            }
        }

    }
}
