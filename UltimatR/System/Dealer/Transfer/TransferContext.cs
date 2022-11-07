
// <copyright file="TransferContext.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Extract;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Series;
    using System.Text;
    using System.Threading;




    /// <summary>
    /// Class TransferContext. This class cannot be inherited.
    /// Implements the <see cref="System.Deal.ITransferContext" />
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.Deal.ITransferContext" />
    /// <seealso cref="System.IDisposable" />
    public sealed class TransferContext : ITransferContext, IDisposable
    {
        #region Constants

        /// <summary>
        /// The buffer size
        /// </summary>
        private const int Buffer_Size = 4096;

        #endregion

        #region Fields

        /// <summary>
        /// The instream
        /// </summary>
        [NonSerialized] private readonly DealStream instream;
        /// <summary>
        /// The outstream
        /// </summary>
        [NonSerialized] private readonly DealStream outstream;
        /// <summary>
        /// The bin receive
        /// </summary>
        [NonSerialized] public byte[] binReceive = new byte[0];
        /// <summary>
        /// The bin receive address
        /// </summary>
        [NonSerialized] public IntPtr binReceiveAddress;
        /// <summary>
        /// The bin receive handler
        /// </summary>
        [NonSerialized] public IntPtr binReceiveHandler;
        /// <summary>
        /// The bin send
        /// </summary>
        [NonSerialized] public byte[] binSend = new byte[0];
        /// <summary>
        /// The bin send address
        /// </summary>
        [NonSerialized] public IntPtr binSendAddress;
        /// <summary>
        /// The bin send handler
        /// </summary>
        [NonSerialized] public IntPtr binSendHandler;
        /// <summary>
        /// The header buffer address
        /// </summary>
        [NonSerialized] public IntPtr headerBufferAddress;
        /// <summary>
        /// The header buffer handler
        /// </summary>
        [NonSerialized] public IntPtr headerBufferHandler;
        /// <summary>
        /// The HTTP headers
        /// </summary>
        [NonSerialized] public Hashtable httpHeaders = new Hashtable();
        /// <summary>
        /// The HTTP options
        /// </summary>
        [NonSerialized] public Hashtable httpOptions = new Hashtable();
        /// <summary>
        /// The message buffer address
        /// </summary>
        [NonSerialized] public IntPtr messageBufferAddress;
        /// <summary>
        /// The message buffer handler
        /// </summary>
        [NonSerialized] public IntPtr messageBufferHandler;
        /// <summary>
        /// The ms receive
        /// </summary>
        [NonSerialized] public MemoryStream msReceive;
        /// <summary>
        /// The ms send
        /// </summary>
        [NonSerialized] public MemoryStream msSend;
        /// <summary>
        /// The request builder
        /// </summary>
        [NonSerialized] public StringBuilder requestBuilder = new StringBuilder();
        /// <summary>
        /// The response builder
        /// </summary>
        [NonSerialized] public StringBuilder responseBuilder = new StringBuilder();
        /// <summary>
        /// The listener
        /// </summary>
        [NonSerialized] private Socket _listener;
        /// <summary>
        /// The block offset
        /// </summary>
        private int Block_Offset = 16;
        /// <summary>
        /// The disposed
        /// </summary>
        [NonSerialized] private bool disposed = false;
        /// <summary>
        /// The headerbuffer
        /// </summary>
        [NonSerialized] private byte[] headerbuffer = new byte[Buffer_Size];
        /// <summary>
        /// The HTTP method
        /// </summary>
        [NonSerialized] private String http_method;
        /// <summary>
        /// The HTTP protocol version
        /// </summary>
        [NonSerialized] private String http_protocol_version;
        /// <summary>
        /// The HTTP URL
        /// </summary>
        [NonSerialized] private String http_url;
        /// <summary>
        /// The HTTP extensions
        /// </summary>
        [NonSerialized]
        private Dictionary<string, string> httpExtensions = new Dictionary<string, string>()
        {
            { ".html", "text/html" },
            { ".css",  "text/css"  },
            { ".less", "text/css"  },
            { ".png",  "image/png" },
            { ".ico",  "image/ico" },
            { ".jpg",  "image/jpg" },
            { ".bmp",  "image/bmp" },
            { ".gif",  "image/gif" },
            { ".js",   "text/javascript" },
            { ".qjs",  "text/javascript" },
            { ".bjs",  "text/babel" },
            { ".json", "application/json" },
            { ".woff", "font/woff" },
            { ".woff2","font/woff2" },
            { ".ttf",  "font/ttf" },
            { ".svg",  "image/svg" }
        };
        /// <summary>
        /// The identifier
        /// </summary>
        private int id;
        /// <summary>
        /// The messagebuffer
        /// </summary>
        [NonSerialized] private byte[] messagebuffer = new byte[Buffer_Size];
        /// <summary>
        /// The resources
        /// </summary>
        [NonSerialized] private IDeck<byte[]> resources;
        /// <summary>
        /// The sb
        /// </summary>
        [NonSerialized] private StringBuilder sb = new StringBuilder();
        /// <summary>
        /// The tr
        /// </summary>
        [NonSerialized] private DealTransfer tr;

        #endregion

        #region Constructors







        /// <summary>
        /// Initializes a new instance of the <see cref="TransferContext" /> class.
        /// </summary>
        /// <param name="listener">The listener.</param>
        /// <param name="_id">The identifier.</param>
        /// <param name="withStream">if set to <c>true</c> [with stream].</param>
        public TransferContext(Socket listener, int _id = -1, bool withStream = false)
        {
            this._listener = listener;
            if (withStream)
            {
                this.instream = new DealStream(listener);
                this.outstream = new DealStream(listener);
            }

            GCHandle gc = GCHandle.Alloc(messagebuffer, GCHandleType.Pinned);
            messageBufferHandler = GCHandle.ToIntPtr(gc);
            messageBufferAddress = gc.AddrOfPinnedObject();
            gc = GCHandle.Alloc(headerbuffer, GCHandleType.Pinned);
            headerBufferHandler = GCHandle.ToIntPtr(gc);
            headerBufferAddress = gc.AddrOfPinnedObject();

            this.id = _id;
            this.Close = false;
            this.Denied = false;
            this.ObjectPosition = 0;
            this.ObjectsLeft = 0;
            this.DeserialBlockId = 0;
            this.BlockSize = 0;
            this.SendMessage = true;
            this.ReceiveMessage = true;
            this.disposed = true;

            HeaderSentNotice.Reset();
            HeaderReceivedNotice.Reset();
            MessageSentNotice.Reset();
            MessageReceivedNotice.Reset();
            BatchesReceivedNotice.Reset();
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the batches received notice.
        /// </summary>
        /// <value>The batches received notice.</value>
        public ManualResetEvent BatchesReceivedNotice { get; set; } = new ManualResetEvent(false);




        /// <summary>
        /// Gets or sets the block offset.
        /// </summary>
        /// <value>The block offset.</value>
        public int BlockOffset
        {
            get
            {
                return Block_Offset;
            }
            set
            {
                Block_Offset = value;
            }
        }




        /// <summary>
        /// Gets or sets the size of the block.
        /// </summary>
        /// <value>The size of the block.</value>
        public long BlockSize { get; set; }




        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        /// <value>The size of the buffer.</value>
        public int BufferSize
        {
            get
            {
                return Buffer_Size;
            }
        }




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ITransferContext" /> is close.
        /// </summary>
        /// <value><c>true</c> if close; otherwise, <c>false</c>.</value>
        public bool Close { get; set; }




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ITransferContext" /> is denied.
        /// </summary>
        /// <value><c>true</c> if denied; otherwise, <c>false</c>.</value>
        public bool Denied { get; set; }




        /// <summary>
        /// Gets the deserial block.
        /// </summary>
        /// <value>The deserial block.</value>
        public byte[] DeserialBlock
        {
            get
            {
                byte[] result = null;
                lock (binReceive)
                {
                    disposed = false;
                    BlockSize = 0;
                    result = binReceive;
                    binReceive = new byte[0];
                }
                return result;
            }
        }




        /// <summary>
        /// Gets or sets the deserial block identifier.
        /// </summary>
        /// <value>The deserial block identifier.</value>
        public int DeserialBlockId { get; set; }




        /// <summary>
        /// Gets the deserial block PTR.
        /// </summary>
        /// <value>The deserial block PTR.</value>
        public IntPtr DeserialBlockPtr => binReceiveAddress;




        /// <summary>
        /// Gets the echo.
        /// </summary>
        /// <value>The echo.</value>
        public string Echo
        {
            get
            {
                return this.sb.ToString();
            }
        }




        /// <summary>
        /// Gets the header buffer.
        /// </summary>
        /// <value>The header buffer.</value>
        public byte[] HeaderBuffer
        {
            get
            {
                return this.headerbuffer;
            }
        }




        /// <summary>
        /// Gets or sets the header received notice.
        /// </summary>
        /// <value>The header received notice.</value>
        public ManualResetEvent HeaderReceivedNotice { get; set; } = new ManualResetEvent(false);




        /// <summary>
        /// Gets or sets the header sent notice.
        /// </summary>
        /// <value>The header sent notice.</value>
        public ManualResetEvent HeaderSentNotice { get; set; } = new ManualResetEvent(false);




        /// <summary>
        /// Gets or sets the HTTP headers.
        /// </summary>
        /// <value>The HTTP headers.</value>
        public Hashtable HttpHeaders
        {
            get { return httpHeaders; }
            set { httpHeaders = value; }
        }




        /// <summary>
        /// Gets or sets the HTTP options.
        /// </summary>
        /// <value>The HTTP options.</value>
        public Hashtable HttpOptions
        {
            get { return httpOptions; }
            set { httpOptions = value; }
        }




        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }




        /// <summary>
        /// Gets or sets the listener.
        /// </summary>
        /// <value>The listener.</value>
        public Socket Listener
        {
            get
            {
                return this._listener;
            }
            set
            {
                this._listener = value;
            }
        }




        /// <summary>
        /// Gets the message buffer.
        /// </summary>
        /// <value>The message buffer.</value>
        public byte[] MessageBuffer
        {
            get
            {
                return this.messagebuffer;
            }
        }




        /// <summary>
        /// Gets or sets the message received notice.
        /// </summary>
        /// <value>The message received notice.</value>
        public ManualResetEvent MessageReceivedNotice { get; set; } = new ManualResetEvent(false);




        /// <summary>
        /// Gets or sets the message sent notice.
        /// </summary>
        /// <value>The message sent notice.</value>
        public ManualResetEvent MessageSentNotice { get; set; } = new ManualResetEvent(false);




        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        public ProtocolMethod Method { get; set; } = ProtocolMethod.NONE;




        /// <summary>
        /// Gets or sets the object position.
        /// </summary>
        /// <value>The object position.</value>
        public int ObjectPosition { get; set; }




        /// <summary>
        /// Gets or sets the objects left.
        /// </summary>
        /// <value>The objects left.</value>
        public int ObjectsLeft { get; set; }




        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public DealProtocol Protocol { get; set; } = DealProtocol.NONE;




        /// <summary>
        /// Gets or sets a value indicating whether [receive message].
        /// </summary>
        /// <value><c>true</c> if [receive message]; otherwise, <c>false</c>.</value>
        public bool ReceiveMessage { get; set; }




        /// <summary>
        /// Gets or sets the request builder.
        /// </summary>
        /// <value>The request builder.</value>
        public StringBuilder RequestBuilder
        {
            get { return requestBuilder; }
            set { requestBuilder = value; }
        }




        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public IDeck<byte[]> Resources
        {
            get { return resources; }
            set { resources = value; }
        }




        /// <summary>
        /// Gets or sets the response builder.
        /// </summary>
        /// <value>The response builder.</value>
        public StringBuilder ResponseBuilder
        {
            get { return responseBuilder; }
            set { responseBuilder = value; }
        }




        /// <summary>
        /// Gets or sets a value indicating whether [send message].
        /// </summary>
        /// <value><c>true</c> if [send message]; otherwise, <c>false</c>.</value>
        public bool SendMessage { get; set; }




        /// <summary>
        /// Gets or sets the serial block.
        /// </summary>
        /// <value>The serial block.</value>
        public byte[] SerialBlock
        {
            get
            {
                return binSend;
            }
            set
            {
                if (value != null)
                {
                    lock (binSend)
                    {
                        disposed = false;
                        binSend = value;
                        if (Protocol != DealProtocol.HTTP)
                        {
                            long size = binSend.Length - BlockOffset;
                            new byte[] { (byte) 'D',
                                         (byte) 'E',
                                         (byte) 'A',
                                         (byte) 'L' }.CopyTo(binSend, 0);
                            BitConverter.GetBytes(size).CopyTo(binSend, 4);
                            BitConverter.GetBytes(ObjectPosition).CopyTo(binSend, 12);
                        }
                        value = null;
                    }
                }
            }
        }




        /// <summary>
        /// Gets or sets the serial block identifier.
        /// </summary>
        /// <value>The serial block identifier.</value>
        public int SerialBlockId { get; set; }




        /// <summary>
        /// Gets the serial block PTR.
        /// </summary>
        /// <value>The serial block PTR.</value>
        public IntPtr SerialBlockPtr => binSendAddress;




        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <value>The site.</value>
        public ServiceSite Site
        {
            get
            {
                return Transfer.MyHeader.Context.IdentitySite;
            }
        }




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ITransferContext" /> is synchronic.
        /// </summary>
        /// <value><c>true</c> if synchronic; otherwise, <c>false</c>.</value>
        public bool Synchronic { get; set; }




        /// <summary>
        /// Gets or sets the transfer.
        /// </summary>
        /// <value>The transfer.</value>
        public DealTransfer Transfer
        {
            get
            {
                return this.tr;
            }
            set
            {
                if (value.Context == null)
                {
                    value.Context = this;
                    if (value.Identity != null)
                        value.MyHeader.BindContext(value.Context);
                }
                if (value.MyMessage.Content != null)
                {
                    if (value.MyMessage.Content.GetType() == typeof(object[][]))
                        value.MyHeader.Context.ObjectsCount = ((object[][])value.MyMessage.Content).Length;
                }
                this.tr = value;
            }
        }




        /// <summary>
        /// Gets or sets the send echo.
        /// </summary>
        /// <value>The send echo.</value>
        internal DealEvent SendEcho { get; set; }

        #endregion

        #region Methods





        /// <summary>
        /// Appends the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Append(string text)
        {
            this.sb.Append(text);
        }






        /// <summary>
        /// Dirs the search.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="jspfiles">The jspfiles.</param>
        public void DirSearch(string dir, List<string> jspfiles)
        {
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                    jspfiles.Add(f);
                foreach (string d in Directory.GetDirectories(dir))
                    DirSearch(d, jspfiles);
            }
            catch (Exception ex)
            {
            }
        }




        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                sb.Clear();
                msSend.Dispose();
                msReceive.Dispose();
                GCHandle gc;
                lock (binReceive)
                {
                    if (!binReceiveHandler.Equals(IntPtr.Zero))
                    {
                        gc = GCHandle.FromIntPtr(binReceiveHandler);
                        gc.Free();
                    }
                    binReceive = null;
                }
                lock (binSend)
                {
                    if (!binSendHandler.Equals(IntPtr.Zero))
                    {
                        gc = GCHandle.FromIntPtr(binSendHandler);
                        gc.Free();
                    }
                    binSend = null;
                }

                gc = GCHandle.FromIntPtr(messageBufferHandler);
                gc.Free();
                messagebuffer = null;
                gc = GCHandle.FromIntPtr(headerBufferHandler);
                gc.Free();
                headerbuffer = null;

                HeaderSentNotice.Dispose();
                HeaderReceivedNotice.Dispose();
                MessageSentNotice.Dispose();
                MessageReceivedNotice.Dispose();
                BatchesReceivedNotice.Dispose();

                disposed = true;
            }
        }





        /// <summary>
        /// Gets the type of the HTTP extension.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetHttpExtensionType()
        {
            string extension = http_url.Substring(http_url.LastIndexOf('.'));
            string result = null;
            httpExtensions.TryGetValue(extension, out result);
            return result = (result == null) ? "text/html" : result;
        }






        /// <summary>
        /// Gets the java script project.
        /// </summary>
        /// <param name="ms">The ms.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GetJavaScriptProject(MemoryStream ms)
        {
            string extension = http_url.Substring(http_url.LastIndexOf('.'));
            if (extension.Equals(".qjs") || extension.Equals(".bjs"))
            {
                string jspdir = "../../Web" + http_url.Substring(0, http_url.LastIndexOf('.'));
                List<string> jspfiles = new List<string>();
                DirSearch(jspdir, jspfiles);
                foreach (string jspfile in jspfiles)
                {
                    Stream fs = File.Open(jspfile, FileMode.Open);
                    fs.CopyTo(ms);
                    
                    fs.Close();
                }
                return true;
            }
            return false;
        }




        /// <summary>
        /// Handles the denied request.
        /// </summary>
        public void HandleDeniedRequest()
        {
            writeDenied();
            writeClose();
            binSend = ResponseBuilder.ToString().ToBytes(CharEncoding.UTF8);
        }





        /// <summary>
        /// Handles the get request.
        /// </summary>
        /// <param name="content_type">Type of the content.</param>
        public void HandleGetRequest(string content_type = "text/html")
        {
            if (http_url.Equals("/") ||
                http_url.Equals(""))
                http_url = "/Index.html";

            if (!Resources.TryGet(http_url, out binSend))
            {
                MemoryStream ms = new MemoryStream();
                if (GetJavaScriptProject(ms))
                {
                    content_type = GetHttpExtensionType();
                    writeSuccess(content_type);
                }
                else
                {
                    if (File.Exists("../../Web" + http_url))
                    {
                        content_type = GetHttpExtensionType();
                        Stream fs = File.Open("../../Web" + http_url, FileMode.Open);
                        fs.CopyTo(ms);
                        fs.Close();
                        writeSuccess(content_type);
                    }
                    else
                        writeFailure();
                }

                if (HttpHeaders.ContainsKey("Connection"))
                    writeClose(HttpHeaders["Connection"].ToString());
                else
                    writeClose();

                if (content_type.Contains("text") ||
                    content_type.Contains("json"))
                {
                    ResponseBuilder.Append((ms.ToArray().ToChars(CharEncoding.UTF8)));
                    binSend = ResponseBuilder.ToString().ToBytes(CharEncoding.UTF8);
                    Resources.Add(http_url, binSend);
                }
                else
                {
                    binSend = ResponseBuilder.ToString().ToBytes(CharEncoding.UTF8).Concat(ms.ToArray()).ToArray();
                    Resources.Add(http_url, binSend);
                }
                ms.Dispose();
            }
        }





        /// <summary>
        /// Handles the options request.
        /// </summary>
        /// <param name="content_type">Type of the content.</param>
        public void HandleOptionsRequest(string content_type = "text/html")
        {
            writeSuccess(content_type);
            writeOptions();
            writeClose();
            binSend = ResponseBuilder.ToString().ToBytes(CharEncoding.UTF8);
        }





        /// <summary>
        /// Handles the post request.
        /// </summary>
        /// <param name="content_type">Type of the content.</param>
        public void HandlePostRequest(string content_type = "text/html")
        {
            writeSuccess(content_type);
            writeOptions();
            writeClose();
            string requestBuilder = RequestBuilder.ToString();
            ResponseBuilder.AppendLine(requestBuilder);
            string responseString = ResponseBuilder.ToString();
            binSend = responseString.ToBytes(CharEncoding.UTF8);
        }






        /// <summary>
        /// HTTPs the header.
        /// </summary>
        /// <param name="received">The received.</param>
        /// <returns>MarkupType.</returns>
        public MarkupType HttpHeader(int received)
        {
            MarkupType noiseKind = MarkupType.None;

            lock (binReceive)
            {
                if (BlockSize == 0)
                {
                    BlockSize = 1;
                    msReceive = new MemoryStream();
                }

                msReceive.Write(HeaderBuffer, 0, received);

                if (received < BufferSize)
                {
                    BlockSize = 0;
                    msReceive.Position = 0;
                    ParseRequest(msReceive);
                    ReadHeaders(msReceive);
                    if (VerifyRequest())
                        binReceive = msReceive.ToArray().Skip(Convert.ToInt32(msReceive.Position)).ToArray();
                    else
                        Denied = true;
                }
            }
            return noiseKind;
        }





        /// <summary>
        /// Identifies the protocol.
        /// </summary>
        /// <returns>DealProtocol.</returns>
        public DealProtocol IdentifyProtocol()
        {
            StringBuilder sb = new StringBuilder();
            Protocol = DealProtocol.NONE;
            ProtocolMethod method = ProtocolMethod.NONE;
            for (int i = 0; i < HeaderBuffer.Length; i++)
            {
                MarkupType splitter = MarkupType.None;
                HeaderBuffer[i].IsSpliter(out splitter);
                if ((splitter != MarkupType.Empty) &&
                    (splitter != MarkupType.Space) &&
                    (splitter != MarkupType.Line))
                    sb.Append(HeaderBuffer[i].ToChars(CharEncoding.UTF8));
                if (sb.Length > 3)
                {
                    method = ProtocolMethod.NONE;
                    Enum.TryParse(sb.ToString().ToUpper(), out method);
                    if (method != ProtocolMethod.NONE)
                    {
                        switch (method)
                        {
                            case ProtocolMethod.DEAL:
                                Protocol = DealProtocol.DOTP;
                                break;
                            case ProtocolMethod.SYNC:
                                Protocol = DealProtocol.HTTP;
                                break;
                            case ProtocolMethod.GET:
                                Protocol = DealProtocol.HTTP;
                                break;
                            case ProtocolMethod.POST:
                                Protocol = DealProtocol.HTTP;
                                break;
                            case ProtocolMethod.OPTIONS:
                                Protocol = DealProtocol.HTTP;
                                break;
                            default:
                                Protocol = DealProtocol.NONE;
                                break;
                        }
                    }
                    Method = method;
                    if (Protocol != DealProtocol.NONE)
                    {
                        sb = null;
                        return Protocol;
                    }
                }
            }
            sb = null;
            return Protocol;
        }






        /// <summary>
        /// Incomings the header.
        /// </summary>
        /// <param name="received">The received.</param>
        /// <returns>MarkupType.</returns>
        public MarkupType IncomingHeader(int received)
        {
            disposed = false;
            MarkupType noiseKind = MarkupType.None;
            if (Protocol == DealProtocol.NONE)
                IdentifyProtocol();
            if (Protocol == DealProtocol.DOTP)
                return SyncHeader(received);
            else if (Protocol == DealProtocol.HTTP)
                return HttpHeader(received);
            return noiseKind;
        }






        /// <summary>
        /// Incomings the message.
        /// </summary>
        /// <param name="received">The received.</param>
        /// <returns>MarkupType.</returns>
        public unsafe MarkupType IncomingMessage(int received)
        {
            disposed = false;
            MarkupType noiseKind = MarkupType.None;
            if (Protocol == DealProtocol.DOTP)
                return SyncMessage(received);
            return noiseKind;
        }





        /// <summary>
        /// Parses the request.
        /// </summary>
        /// <param name="ms">The ms.</param>
        /// <exception cref="System.Logs.ILogSate.Exception">invalid http request line</exception>
        public void ParseRequest(Stream ms)
        {
            String request = streamReadLine(ms);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_version = tokens[2];
        }





        /// <summary>
        /// Reads the headers.
        /// </summary>
        /// <param name="ms">The ms.</param>
        /// <exception cref="System.Logs.ILogSate.Exception">invalid http header line: " + line</exception>
        public void ReadHeaders(Stream ms)
        {
            String line;
            while ((line = streamReadLine(ms)) != null)
            {
                if (line.Equals(""))
                    return;

                int separator = line.IndexOf(':');
                if (separator == -1)
                    throw new Exception("invalid http header line: " + line);

                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                    pos++; 

                string value = line.Substring(pos, line.Length - pos);
                HttpHeaders[name] = value;
            }
        }




        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            if (!disposed)
            {
                sb.Clear();
                sb = new StringBuilder();
                msSend.Dispose();
                msSend = new MemoryStream();
                msReceive.Dispose();
                msReceive = new MemoryStream();

                lock (binReceive)
                {
                    if (!binReceiveHandler.Equals(IntPtr.Zero))
                    {
                        GCHandle gc = GCHandle.FromIntPtr(binReceiveHandler);
                        gc.Free();
                        binReceive = new byte[0];
                    }
                }
                lock (binSend)
                    binSend = new byte[0];
            }
        }






        /// <summary>
        /// Synchronizes the header.
        /// </summary>
        /// <param name="received">The received.</param>
        /// <returns>MarkupType.</returns>
        public unsafe MarkupType SyncHeader(int received)
        {
            MarkupType noiseKind = MarkupType.None;

            lock (binReceive)
            {
                int offset = 0, length = received;
                bool inprogress = false;
                if (BlockSize == 0)
                {
                    BlockSize = *((int*)(headerBufferAddress + 4).ToPointer());
                    DeserialBlockId = *((int*)(headerBufferAddress + 12).ToPointer());

                    binReceive = new byte[BlockSize];
                    GCHandle gc = GCHandle.Alloc(binReceive, GCHandleType.Pinned);
                    binReceiveHandler = GCHandle.ToIntPtr(gc);
                    binReceiveAddress = gc.AddrOfPinnedObject();

                    offset = BlockOffset;
                    length -= BlockOffset;
                }

                if (BlockSize > 0)
                    inprogress = true;

                BlockSize -= length;

                if (BlockSize < 1)
                {
                    long endPosition = length;
                    noiseKind = HeaderBuffer.SeekMarkup(out endPosition, SeekDirection.Backward);
                }

                int destid = (int)(binReceive.Length - (BlockSize + length));

                if (inprogress)
                {
                    Extractor.CopyBlock(binReceiveAddress, destid, headerBufferAddress, offset, length);
                }
            }
            return noiseKind;
        }






        /// <summary>
        /// Synchronizes the message.
        /// </summary>
        /// <param name="received">The received.</param>
        /// <returns>MarkupType.</returns>
        public unsafe MarkupType SyncMessage(int received)
        {
            MarkupType noiseKind = MarkupType.None;

            lock (binReceive)
            {
                int offset = 0, length = received;
                bool inprogress = false;

                if (BlockSize == 0)
                {
                    BlockSize = *((int*)(messageBufferAddress + 4).ToPointer());
                    DeserialBlockId = *((int*)(messageBufferAddress + 12).ToPointer());

                    binReceive = new byte[BlockSize];
                    GCHandle gc = GCHandle.Alloc(binReceive, GCHandleType.Pinned);
                    binReceiveHandler = GCHandle.ToIntPtr(gc);
                    binReceiveAddress = gc.AddrOfPinnedObject();

                    offset = BlockOffset;
                    length -= BlockOffset;
                }

                if (BlockSize > 0)
                    inprogress = true;

                BlockSize -= length;

                if (BlockSize < 1)
                {
                    long endPosition = length;
                    noiseKind = MessageBuffer.SeekMarkup(out endPosition, SeekDirection.Backward);
                }

                int destid = (int)(binReceive.Length - (BlockSize + length));
                if (inprogress)
                {
                    Extractor.CopyBlock(binReceiveAddress, destid, messageBufferAddress, offset, length);
                }
            }
            return noiseKind;
        }





        /// <summary>
        /// Verifies the request.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool VerifyRequest()
        {
            bool verified = false;
            string ip = tr.MyHeader.Context.RemoteEndPoint.Address.ToString();

            if (HttpHeaders.ContainsKey("DealerToken"))
                if (HttpHeaders["DealerToken"].ToString() != "")
                {
                    string token = HttpHeaders["DealerToken"].ToString();
                    MemberIdentity di = null;
                    if (DealServer.Security.Register(token, out di, ip))
                    {
                        verified = true;
                        HttpOptions["DealerToken"] = di.Token;
                        HttpOptions["DealerUserId"] = di.UserId;
                        HttpOptions["DealerDeptId"] = di.DeptId;
                    }
                }

            if (!verified)
                if (HttpHeaders.ContainsKey("Authorization"))
                    if (HttpHeaders["Authorization"].ToString() != "")
                    {
                        string[] codes = HttpHeaders["Authorization"].ToString().Split(' ');
                        string decode64 = "";
                        string name = "";
                        string key = "";
                        if (codes.Length > 1)
                        {
                            decode64 = Encoding.UTF8.GetString(Convert.FromBase64String(codes[1]));
                            string[] namekey = decode64.Split(':');
                            name = namekey[0];
                            key = namekey[1];

                            MemberIdentity di = null;
                            if (DealServer.Security.Register(name, key, out di, ip))
                            {
                                verified = true;
                                HttpOptions["DealerToken"] = di.Token;
                                HttpOptions["DealerUserId"] = di.UserId;
                                HttpOptions["DealerDeptId"] = di.DeptId;
                            }
                        }
                    }

            return verified;
        }






        /// <summary>
        /// Streams the read line.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>System.String.</returns>
        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            StringBuilder data = new StringBuilder();
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data.Append(((byte)next_char).ToChars(CharEncoding.UTF8));
            }
            return data.ToString();
        }





        /// <summary>
        /// Writes the close.
        /// </summary>
        /// <param name="state">The state.</param>
        private void writeClose(string state = "close")
        {
            ResponseBuilder.AppendLine("Connection: " + state);
            ResponseBuilder.AppendLine("");
        }




        /// <summary>
        /// Writes the denied.
        /// </summary>
        private void writeDenied()
        {
            ResponseBuilder.AppendLine("HTTP/1.1 401.7 Access denied");
        }




        /// <summary>
        /// Writes the failure.
        /// </summary>
        private void writeFailure()
        {
            ResponseBuilder.AppendLine("HTTP/1.1 404 File not found");
        }




        /// <summary>
        /// Writes the options.
        /// </summary>
        private void writeOptions()
        {
            if (HttpOptions.Count > 0)
                foreach (DictionaryEntry option in HttpOptions)
                    ResponseBuilder.AppendLine(string.Format("{0}: {1}", option.Key, option.Value));

            ResponseBuilder.AppendLine("Accept: application/json");
            ResponseBuilder.AppendLine("Access-Control-Allow-Headers: content-type");
            ResponseBuilder.AppendLine("Access-Control-Allow-Origin: " + HttpHeaders["Origin"].ToString());
        }





        /// <summary>
        /// Writes the success.
        /// </summary>
        /// <param name="content_type">Type of the content.</param>
        private void writeSuccess(string content_type = "text/html")
        {
            ResponseBuilder.AppendLine("HTTP/1.1 200 OK");
            ResponseBuilder.AppendLine("Content-Type: " + content_type);
        }

        #endregion
    }
}
