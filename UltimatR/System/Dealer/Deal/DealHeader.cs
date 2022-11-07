
// <copyright file="DealHeader.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// Class BinaryHeader.
    /// </summary>
    public static class BinaryHeader
    {
        #region Methods







        /// <summary>
        /// Gets the binary.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>DealHeader.</returns>
        public static DealHeader GetBinary(this DealHeader bank, ISerialBuffer buffer)
        {
            try
            {
                MemoryStream ms = new MemoryStream(buffer.DeserialBlock);
                BinaryFormatter binform = new BinaryFormatter();
                DealHeader _bank = (DealHeader)binform.Deserialize(ms);
                ms.Dispose();
                return _bank;
            }
            catch (Exception ex)
            {
                return null;
            }
        }







        /// <summary>
        /// Gets the binary.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="fromstream">The fromstream.</param>
        /// <returns>DealHeader.</returns>
        public static DealHeader GetBinary(this DealHeader bank, Stream fromstream)
        {
            try
            {
                BinaryFormatter binform = new BinaryFormatter();
                return (DealHeader)binform.Deserialize(fromstream);
            }
            catch (Exception ex)
            {
                return null;
            }
        }







        /// <summary>
        /// Sets the binary.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>System.Int32.</returns>
        public static int SetBinary(this DealHeader bank, ISerialBuffer buffer)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(new byte[buffer.BlockOffset], 0, buffer.BlockOffset);
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(ms, bank);
            buffer.SerialBlock = ms.ToArray();
            ms.Dispose();
            return buffer.SerialBlock.Length;
        }







        /// <summary>
        /// Sets the binary.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="tostream">The tostream.</param>
        /// <returns>System.Int32.</returns>
        public static int SetBinary(this DealHeader bank, Stream tostream)
        {
            if (tostream == null) tostream = new MemoryStream();
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(tostream, bank);
            return (int)tostream.Length;
        }

        #endregion
    }

    /// <summary>
    /// Class DealHeader.
    /// Implements the <see cref="System.ISerialFormatter" />
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.ISerialFormatter" />
    /// <seealso cref="System.IDisposable" />
    [Serializable]
    public class DealHeader : ISerialFormatter, IDisposable
    {
        #region Fields

        /// <summary>
        /// The transaction
        /// </summary>
        [NonSerialized]
        private DealTransfer transaction;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader" /> class.
        /// </summary>
        public DealHeader()
        {
            Context = new DealContext();
            SerialCount = 0; DeserialCount = 0;
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader" /> class.
        /// </summary>
        /// <param name="_transaction">The transaction.</param>
        public DealHeader(DealTransfer _transaction)
        {
            Context = new DealContext();
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader" /> class.
        /// </summary>
        /// <param name="_transaction">The transaction.</param>
        /// <param name="context">The context.</param>
        public DealHeader(DealTransfer _transaction, ITransferContext context)
        {
            Context = new DealContext();
            Context.LocalEndPoint = (IPEndPoint)context.Listener.LocalEndPoint;
            Context.RemoteEndPoint = (IPEndPoint)context.Listener.RemoteEndPoint;
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader" /> class.
        /// </summary>
        /// <param name="_transaction">The transaction.</param>
        /// <param name="context">The context.</param>
        /// <param name="identity">The identity.</param>
        public DealHeader(DealTransfer _transaction, ITransferContext context, MemberIdentity identity)
        {
            Context = new DealContext();
            Context.LocalEndPoint = (IPEndPoint)context.Listener.LocalEndPoint;
            Context.RemoteEndPoint = (IPEndPoint)context.Listener.RemoteEndPoint;
            Context.Identity = identity;
            Context.IdentitySite = identity.Site;
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="DealHeader" /> class.
        /// </summary>
        /// <param name="_transaction">The transaction.</param>
        /// <param name="identity">The identity.</param>
        public DealHeader(DealTransfer _transaction, MemberIdentity identity)
        {
            Context = new DealContext();
            Context.Identity = identity;
            Context.IdentitySite = identity.Site;
            transaction = _transaction;
            SerialCount = 0; DeserialCount = 0;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public object Content { get; set; }




        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public DealContext Context { get; set; }




        /// <summary>
        /// Gets or sets the deserial count.
        /// </summary>
        /// <value>The deserial count.</value>
        public int DeserialCount { get; set; }




        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>The items count.</value>
        public int ItemsCount
        {
            get { return Context.ObjectsCount; }
        }




        /// <summary>
        /// Gets or sets the progress count.
        /// </summary>
        /// <value>The progress count.</value>
        public int ProgressCount { get; set; }




        /// <summary>
        /// Gets or sets the serial count.
        /// </summary>
        /// <value>The serial count.</value>
        public int SerialCount { get; set; }

        #endregion

        #region Methods





        /// <summary>
        /// Binds the context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void BindContext(ITransferContext context)
        {
            Context.LocalEndPoint = (IPEndPoint)context.Listener.LocalEndPoint;
            Context.RemoteEndPoint = (IPEndPoint)context.Listener.RemoteEndPoint;
        }







        /// <summary>
        /// Deserializes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Object.</returns>
        public object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.GetBinary(buffer);
            else if (serialFormat == SerialFormat.Json)
                return this.GetJson(buffer);
            else
                return null;
        }







        /// <summary>
        /// Deserializes the specified fromstream.
        /// </summary>
        /// <param name="fromstream">The fromstream.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Object.</returns>
        public object Deserialize(Stream fromstream, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.GetBinary(fromstream);
            else if (serialFormat == SerialFormat.Json)
                return this.GetJson(fromstream);
            else
                return null;
        }




        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Content = null;
        }





        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetHeader()
        {
            return this;
        }





        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public object[] GetMessage()
        {
            return null;
        }









        /// <summary>
        /// Serializes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        public int Serialize(ISerialBuffer buffer, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.SetBinary(buffer);
            else if (serialFormat == SerialFormat.Json)
                return this.SetJson(buffer);
            else
                return -1;
        }









        /// <summary>
        /// Serializes the specified tostream.
        /// </summary>
        /// <param name="tostream">The tostream.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        public int Serialize(Stream tostream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.SetBinary(tostream);
            else if (serialFormat == SerialFormat.Json)
                return this.SetJson(tostream);
            else
                return -1;
        }

        #endregion
    }

    /// <summary>
    /// Class JsonHeader.
    /// </summary>
    public static class JsonHeader
    {
        #region Methods







        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>DealHeader.</returns>
        public static DealHeader GetJson(this DealHeader thdr, ISerialBuffer buffer)
        {
            try
            {
                DealHeader trs = null;

                byte[] _fromarray = buffer.DeserialBlock;
                StringBuilder sb = new StringBuilder();

                sb.Append(_fromarray.ToChars(CharEncoding.UTF8));
                trs = thdr.GetJsonObject(sb.ToString())["DealHeader"];

                _fromarray = null;
                sb = null;
                return trs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }







        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="fromstream">The fromstream.</param>
        /// <returns>DealHeader.</returns>
        public static DealHeader GetJson(this DealHeader thdr, Stream fromstream)
        {
            try
            {
                fromstream.Position = 0;
                byte[] array = new byte[4096];
                int read = 0;
                StringBuilder sb = new StringBuilder();
                while ((read = fromstream.Read(array, 0, array.Length)) > 0)
                {
                    sb.Append(array.Select(b => (char)b).ToArray());
                }
                DealHeader trs = thdr.GetJsonObject(sb.ToString())["DealHeader"];
                sb = null;
                fromstream.Dispose();
                return trs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }







        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="jsonstring">The jsonstring.</param>
        /// <returns>DealHeader.</returns>
        public static DealHeader GetJson(this DealHeader thdr, string jsonstring)
        {
            try
            {
                DealHeader trs = thdr.GetJsonObject(jsonstring)["DealHeader"];
                return trs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }







        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="stringbuilder">The stringbuilder.</param>
        /// <returns>DealHeader.</returns>
        public static DealHeader GetJson(this DealHeader thdr, StringBuilder stringbuilder)
        {
            try
            {
                StringBuilder sb = stringbuilder;
                DealHeader trs = thdr.GetJsonObject(sb.ToString())["DealHeader"];
                return trs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }







        /// <summary>
        /// Gets the json bag.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="JsonString">The json string.</param>
        /// <param name="_bag">The bag.</param>
        public static void GetJsonBag(this DealHeader thdr, string JsonString, IDictionary<string, object> _bag)
        {
            _bag.Add(JsonParser.FromJson(JsonString));
        }







        /// <summary>
        /// Gets the json object.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="_bag">The bag.</param>
        /// <returns>Dictionary&lt;System.String, DealHeader&gt;.</returns>
        public static Dictionary<string, DealHeader> GetJsonObject(this DealHeader thdr, IDictionary<string, object> _bag)
        {
            Dictionary<string, DealHeader> result = new Dictionary<string, DealHeader>();
            IDictionary<string, object> bags = _bag;
            foreach (KeyValuePair<string, object> bag in bags)
            {
                object inst = new object();
                IEnumerable<PropertyInfo> map = JsonParser.PrepareInstance(out inst, typeof(DealHeader));
                JsonParser.DeserializeType(map, (IDictionary<string, object>)bag.Value, inst);
                DealHeader deck = (DealHeader)inst;
                result.Add(bag.Key, deck);
            }
            return result;
        }







        /// <summary>
        /// Gets the json object.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="JsonString">The json string.</param>
        /// <returns>Dictionary&lt;System.String, DealHeader&gt;.</returns>
        public static Dictionary<string, DealHeader> GetJsonObject(this DealHeader thdr, string JsonString)
        {
            Dictionary<string, DealHeader> result = new Dictionary<string, DealHeader>();
            Dictionary<string, object> bags = new Dictionary<string, object>();
            thdr.GetJsonBag(JsonString, bags);

            foreach (KeyValuePair<string, object> bag in bags)
            {
                object inst = new object();
                IEnumerable<PropertyInfo> map = JsonParser.PrepareInstance(out inst, typeof(DealHeader));
                JsonParser.DeserializeType(map, (IDictionary<string, object>)bag.Value, inst);
                DealHeader deck = (DealHeader)inst;
                result.Add(bag.Key, deck);
            }
            return result;
        }








        /// <summary>
        /// Sets the json.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Int32.</returns>
        public static int SetJson(this DealHeader thdr, ISerialBuffer buffer, int offset = 0)
        {
            if (offset > 0)
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(thdr.SetJsonString());
                byte[] serialBytes = new byte[jsonBytes.Length + offset];
                jsonBytes.CopyTo(serialBytes, offset);
                buffer.SerialBlock = serialBytes;
                jsonBytes = null;
            }
            else
                buffer.SerialBlock = Encoding.UTF8.GetBytes(thdr.SetJsonString());

            return buffer.SerialBlock.Length;
        }







        /// <summary>
        /// Sets the json.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="tostream">The tostream.</param>
        /// <returns>System.Int32.</returns>
        public static int SetJson(this DealHeader thdr, Stream tostream)
        {
            BinaryWriter binwriter = new BinaryWriter(tostream);
            binwriter.Write(thdr.SetJsonString());
            return (int)tostream.Length;
        }







        /// <summary>
        /// Sets the json.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="stringbuilder">The stringbuilder.</param>
        /// <returns>System.Int32.</returns>
        public static int SetJson(this DealHeader thdr, StringBuilder stringbuilder)
        {
            stringbuilder.AppendLine(thdr.SetJsonString());
            return stringbuilder.Length;
        }






        /// <summary>
        /// Sets the json bag.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <returns>IDictionary&lt;System.String, System.Object&gt;.</returns>
        public static IDictionary<string, object> SetJsonBag(this DealHeader thdr)
        {
            return new Dictionary<string, object>() { { "DealHeader", JsonParserProperties.GetJsonProperties(typeof(DealHeader), thdr.Context.Complexity)
                                                                       .Select(k => new KeyValuePair<string, object>(k.Name, k.GetValue(thdr, null)))
                                                                       .ToDictionary(k => k.Key, v => v.Value) } };
        }






        /// <summary>
        /// Sets the json string.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <returns>System.String.</returns>
        public static string SetJsonString(this DealHeader thdr)
        {
            IDictionary<string, object> toJson = thdr.SetJsonBag();
            return JsonParser.ToJson(toJson, thdr.Context.Complexity);
        }







        /// <summary>
        /// Sets the json string.
        /// </summary>
        /// <param name="thdr">The THDR.</param>
        /// <param name="jsonbag">The jsonbag.</param>
        /// <returns>System.String.</returns>
        public static string SetJsonString(this DealHeader thdr, IDictionary<string, object> jsonbag)
        {
            return JsonParser.ToJson(jsonbag, thdr.Context.Complexity);
        }

        #endregion
    }
}
