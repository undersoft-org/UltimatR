
// <copyright file="DealStream.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System;
    using System.IO;
    using System.Net.Sockets;




    /// <summary>
    /// Class DealStream. This class cannot be inherited.
    /// Implements the <see cref="System.IO.Stream" />
    /// </summary>
    /// <seealso cref="System.IO.Stream" />
    public sealed class EthStream : Stream
    {
        #region Constants

        /// <summary>
        /// The read limit
        /// </summary>
        internal const int readLimit = 4194304;
        /// <summary>
        /// The write limit
        /// </summary>
        internal const int writeLimit = 65536;

        #endregion

        #region Fields


        /// <summary>
        /// The socket
        /// </summary>
        private Socket socket;
        /// <summary>
        /// The timeout
        /// </summary>
        private int timeout = 0;

        #endregion

        #region Constructors





        /// <summary>
        /// Initializes a new instance of the <see cref="EthStream" /> class.
        /// </summary>
        /// <param name="socketToStream">The socket to stream.</param>
        /// <exception cref="System.ArgumentNullException">socket</exception>
        public EthStream(Socket socketToStream)
        {
            socket = socketToStream;
            if (socket == null)
                throw new ArgumentNullException("socket");
        }

        #endregion

        #region Properties





        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public override bool CanRead
        {
            get { return true; }
        }




        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value><c>true</c> if this instance can seek; otherwise, <c>false</c>.</value>
        public override bool CanSeek
        {
            get { return false; }
        }




        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
        public override bool CanWrite
        {
            get { return true; }
        }




        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value>The length.</value>
        /// <exception cref="System.NotSupportedException">dont't ever use in net kind of streams</exception>
        public override long Length
        {
            get { throw new NotSupportedException("dont't ever use in net kind of streams"); }
        }





        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value>The position.</value>
        /// <exception cref="System.NotSupportedException">dont't ever use in net kind of streams</exception>
        public override long Position
        {
            get { throw new NotSupportedException("dont't ever use in net kind of streams"); }
            set { throw new NotSupportedException("dont't ever use in net kind of streams"); }
        }

        #endregion

        #region Methods










        /// <summary>
        /// Begins the read.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="contextObject">The context object.</param>
        /// <returns>IAsyncResult.</returns>
        /// <exception cref="System.NotSupportedException">reach read Limit 4MB</exception>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback asyncCallback, object contextObject)
        {
            if (size >= readLimit) { throw new NotSupportedException("reach read Limit 4MB"); }
            IAsyncResult result = socket.BeginReceive(buffer, offset, size, SocketFlags.None, asyncCallback, contextObject);
            return result;
        }











        /// <summary>
        /// Begins the write.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="contextObject">The context object.</param>
        /// <returns>IAsyncResult.</returns>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback asyncCallback, object contextObject)
        {
            IAsyncResult result = socket.BeginSend(buffer, offset, size, SocketFlags.None, asyncCallback, contextObject);
            return result;
        }






        /// <summary>
        /// Waits for the pending asynchronous read to complete. (Consider using <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)
        /// </summary>
        /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
        /// <returns>The number of bytes read from the stream, between zero (0) and the number of bytes you requested. Streams return zero (0) only at the end of the stream, otherwise, they should block until at least one byte is available.</returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }





        /// <summary>
        /// Ends an asynchronous write operation. (Consider using <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)
        /// </summary>
        /// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            socket.EndSend(asyncResult);
        }




        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="System.NotSupportedException">don't use its a empty method for future use maybe</exception>
        public override void Flush()
        {
            throw new NotSupportedException("don't use its a empty method for future use maybe");
        }








        /// <summary>
        /// Reads the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotSupportedException">reach read Limit 64K</exception>
        /// <exception cref="System.Logs.ILogSate.Exception"></exception>
        public override int Read(byte[] buffer, int offset, int size)
        {
            if (timeout <= 0)
            {
                if (size >= readLimit) { throw new NotSupportedException("reach read Limit 64K"); }
                return socket.Receive(buffer, offset, Math.Min(size, readLimit), SocketFlags.None);
            }
            else
            {
                if (size >= readLimit) { throw new NotSupportedException("reach read Limit 64K"); }
                IAsyncResult ar = socket.BeginReceive(buffer, offset, size, SocketFlags.None, null, null);
                if (timeout > 0 && !ar.IsCompleted)
                {
                    ar.AsyncWaitHandle.WaitOne(timeout, false);
                    if (!ar.IsCompleted)
                        throw new Exception();

                }
                return socket.EndReceive(ar);
            }
        }







        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception cref="System.NotSupportedException">dont't ever use in socket kind of streams</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("dont't ever use in socket kind of streams");
        }





        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="System.NotSupportedException">dont't ever use in net kind of streams</exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("dont't ever use in net kind of streams");
        }








        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        public override void Write(byte[] buffer, int offset, int size)
        {
            int tempSize = size;
            while (tempSize > 0)
            {
                size = Math.Min(tempSize, writeLimit);
                socket.Send(buffer, offset, size, SocketFlags.None);
                tempSize -= size;
                offset += size;
            }
        }






        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposeIt"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposeIt)
        {
            try
            {
                if (disposeIt)
                    socket.Close();
            }
            finally
            {
                base.Dispose(disposeIt);
            }
        }

        #endregion
    }
}
