
// <copyright file="DealMessage.cs" company="UltimatR.Core">
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
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;




    /// <summary>
    /// Class BinaryMessage.
    /// </summary>
    public static class BinaryMessage
    {
        #region Methods







        /// <summary>
        /// Gets the binary.
        /// </summary>
        /// <param name="bank">The bank.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>DealMessage.</returns>
        public static DealMessage GetBinary(this DealMessage bank, ISerialBuffer buffer)
        {
            try
            {
                MemoryStream ms = new MemoryStream(buffer.DeserialBlock);
                BinaryFormatter binform = new BinaryFormatter();
                DealMessage _bank = (DealMessage)binform.Deserialize(ms);
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
        /// <returns>DealMessage.</returns>
        public static DealMessage GetBinary(this DealMessage bank, Stream fromstream)
        {
            try
            {
                BinaryFormatter binform = new BinaryFormatter();
                return (DealMessage)binform.Deserialize(fromstream);
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
        public static int SetBinary(this DealMessage bank, ISerialBuffer buffer)
        {
            int offset = buffer.BlockOffset;
            MemoryStream ms = new MemoryStream();
            ms.Write(new byte[offset], 0, offset);
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
        public static int SetBinary(this DealMessage bank, Stream tostream)
        {
            if (tostream == null) tostream = new MemoryStream();
            BinaryFormatter binform = new BinaryFormatter();
            binform.Serialize(tostream, bank);
            return (int)tostream.Length;
        }

        #endregion
    }




    /// <summary>
    /// Class DealMessage.
    /// Implements the <see cref="System.ISerialFormatter" />
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.ISerialFormatter" />
    /// <seealso cref="System.IDisposable" />
    [Serializable]
    public class DealMessage : ISerialFormatter, IDisposable
    {
        #region Fields

        /// <summary>
        /// The content
        /// </summary>
        private object content;
        /// <summary>
        /// The direction
        /// </summary>
        private DirectionType direction;
        /// <summary>
        /// The transaction
        /// </summary>
        [NonSerialized]
        private DealTransfer transaction;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="DealMessage" /> class.
        /// </summary>
        public DealMessage()
        {
            content = new object();
            SerialCount = 0;
            DeserialCount = 0;
            direction = DirectionType.Receive;
        }






        /// <summary>
        /// Initializes a new instance of the <see cref="DealMessage" /> class.
        /// </summary>
        /// <param name="_transaction">The transaction.</param>
        /// <param name="_direction">The direction.</param>
        /// <param name="message">The message.</param>
        public DealMessage(DealTransfer _transaction, DirectionType _direction, object message = null)
        {
            transaction = _transaction;
            direction = _direction;

            if (message != null)
                Content = message;
            else
                content = new object();

            SerialCount = 0;
            DeserialCount = 0;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public object Content
        {
            get { return content; }
            set { transaction.Manager.MessageContent(ref content, value, direction); }
        }




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
            get { return (content != null) ? ((ISerialFormatter[])content).Sum(t => t.ItemsCount) : 0; }
        }




        /// <summary>
        /// Gets or sets the notice.
        /// </summary>
        /// <value>The notice.</value>
        public string Notice { get; set; }




        /// <summary>
        /// Gets the objects count.
        /// </summary>
        /// <value>The objects count.</value>
        public int ObjectsCount
        {
            get { return (content != null) ? ((ISerialFormatter[])content).Length : 0; }
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
                return -1;
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
                return -1;
        }




        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            content = null;
        }





        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetHeader()
        {
            if (direction == DirectionType.Send)
                return transaction.MyHeader.Content;
            else
                return transaction.HeaderReceived.Content;
        }





        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>System.Object[].</returns>
        public object[] GetMessage()
        {
            if (content != null)
                return (ISerialFormatter[])content;
            return null;
        }









        /// <summary>
        /// Serializes the specified buffor.
        /// </summary>
        /// <param name="buffor">The buffor.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="serialFormat">The serial format.</param>
        /// <returns>System.Int32.</returns>
        public int Serialize(ISerialBuffer buffor, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            if (serialFormat == SerialFormat.Binary)
                return this.SetBinary(buffor);
            else if (serialFormat == SerialFormat.Json)
                return this.SetJson(buffor);
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
    /// Class JsonMessage.
    /// </summary>
    public static class JsonMessage
    {
        #region Methods







        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>DealMessage.</returns>
        public static DealMessage GetJson(this DealMessage tmsg, ISerialBuffer buffer)
        {
            try
            {
                DealMessage trs = null;

                byte[] _fromarray = buffer.DeserialBlock;
                StringBuilder sb = new StringBuilder();

                sb.Append(_fromarray.ToChars(CharEncoding.UTF8));
                trs = tmsg.GetJsonObject(sb.ToString())["DealMessage"];

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
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="fromstream">The fromstream.</param>
        /// <returns>DealMessage.</returns>
        public static DealMessage GetJson(this DealMessage tmsg, Stream fromstream)
        {
            try
            {
                fromstream.Position = 0;
                byte[] array = new byte[4096];
                int read = 0;
                StringBuilder sb = new StringBuilder();
                while ((read = fromstream.Read(array, 0, array.Length)) > 0)
                {
                    sb.Append(array.Cast<char>());
                }
                DealMessage trs = tmsg.GetJsonObject(sb.ToString())["DealMessage"];
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
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="jsonstring">The jsonstring.</param>
        /// <returns>DealMessage.</returns>
        public static DealMessage GetJson(this DealMessage tmsg, string jsonstring)
        {
            try
            {
                DealMessage trs = tmsg.GetJsonObject(jsonstring)["DealMessage"];
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
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="stringbuilder">The stringbuilder.</param>
        /// <returns>DealMessage.</returns>
        public static DealMessage GetJson(this DealMessage tmsg, StringBuilder stringbuilder)
        {
            try
            {
                StringBuilder sb = stringbuilder;
                DealMessage trs = tmsg.GetJsonObject(sb.ToString())["DealMessage"];
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
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="JsonString">The json string.</param>
        /// <param name="_bag">The bag.</param>
        public static void GetJsonBag(this DealMessage tmsg, string JsonString, IDictionary<string, object> _bag)
        {
            _bag.Add(JsonParser.FromJson(JsonString));
        }







        /// <summary>
        /// Gets the json object.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="_bag">The bag.</param>
        /// <returns>Dictionary&lt;System.String, DealMessage&gt;.</returns>
        public static Dictionary<string, DealMessage> GetJsonObject(this DealMessage tmsg, IDictionary<string, object> _bag)
        {
            Dictionary<string, DealMessage> result = new Dictionary<string, DealMessage>();
            IDictionary<string, object> bags = _bag;
            foreach (KeyValuePair<string, object> bag in bags)
            {
                object inst = new object();
                IEnumerable<PropertyInfo> map = JsonParser.PrepareInstance(out inst, typeof(DealMessage));
                JsonParser.DeserializeType(map, (IDictionary<string, object>)bag.Value, inst);
                DealMessage deck = (DealMessage)inst;
                result.Add(bag.Key, deck);
            }
            return result;
        }







        /// <summary>
        /// Gets the json object.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="JsonString">The json string.</param>
        /// <returns>Dictionary&lt;System.String, DealMessage&gt;.</returns>
        public static Dictionary<string, DealMessage> GetJsonObject(this DealMessage tmsg, string JsonString)
        {
            Dictionary<string, DealMessage> result = new Dictionary<string, DealMessage>();
            Dictionary<string, object> bags = new Dictionary<string, object>();
            tmsg.GetJsonBag(JsonString, bags);

            foreach (KeyValuePair<string, object> bag in bags)
            {
                object inst = new object();
                IEnumerable<PropertyInfo> map = JsonParser.PrepareInstance(out inst, typeof(DealMessage));
                JsonParser.DeserializeType(map, (IDictionary<string, object>)bag.Value, inst);
                DealMessage deck = (DealMessage)inst;
                result.Add(bag.Key, deck);
            }
            return result;
        }







        /// <summary>
        /// Sets the json.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>System.Int32.</returns>
        public static int SetJson(this DealMessage tmsg, ISerialBuffer buffer)
        {
            buffer.SerialBlock = Encoding.UTF8.GetBytes(tmsg.SetJsonString());
            return buffer.SerialBlock.Length;
        }







        /// <summary>
        /// Sets the json.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="tostream">The tostream.</param>
        /// <returns>System.Int32.</returns>
        public static int SetJson(this DealMessage tmsg, Stream tostream)
        {
            BinaryWriter binwriter = new BinaryWriter(tostream);
            binwriter.Write(tmsg.SetJsonString());
            return (int)tostream.Length;
        }







        /// <summary>
        /// Sets the json.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="stringbuilder">The stringbuilder.</param>
        /// <returns>System.Int32.</returns>
        public static int SetJson(this DealMessage tmsg, StringBuilder stringbuilder)
        {
            stringbuilder.AppendLine(tmsg.SetJsonString());
            return stringbuilder.Length;
        }






        /// <summary>
        /// Sets the json bag.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, object> SetJsonBag(this DealMessage tmsg)
        {
            return new Dictionary<string, object>() { { "DealMessage", JsonParserProperties.GetJsonProperties(typeof(DealMessage))
                                                                       .Select(k => new KeyValuePair<string, object>(k.Name, k.GetValue(tmsg, null)))
                                                                       .ToDictionary(k => k.Key, v => v.Value) } };
        }






        /// <summary>
        /// Sets the json string.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <returns>System.String.</returns>
        public static string SetJsonString(this DealMessage tmsg)
        {
            IDictionary<string, object> toJson = tmsg.SetJsonBag();
            return JsonParser.ToJson(toJson);
        }







        /// <summary>
        /// Sets the json string.
        /// </summary>
        /// <param name="tmsg">The TMSG.</param>
        /// <param name="jsonbag">The jsonbag.</param>
        /// <returns>System.String.</returns>
        public static string SetJsonString(this DealMessage tmsg, IDictionary<string, object> jsonbag)
        {
            return JsonParser.ToJson(jsonbag);
        }

        #endregion
    }
}
