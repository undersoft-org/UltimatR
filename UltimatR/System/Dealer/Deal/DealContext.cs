/// <summary>
/// The Deal namespace.
/// </summary>
namespace System.Deal
{
    using System.Net;




    /// <summary>
    /// Class DealContext.
    /// </summary>
    [Serializable]
    public class DealContext
    {
        #region Fields

        /// <summary>
        /// The event class
        /// </summary>
        public string EventClass;
        /// <summary>
        /// The event method
        /// </summary>
        public string EventMethod;
        /// <summary>
        /// The local end point
        /// </summary>
        [NonSerialized] public IPEndPoint LocalEndPoint;
        /// <summary>
        /// The remote end point
        /// </summary>
        [NonSerialized] public IPEndPoint RemoteEndPoint;
        /// <summary>
        /// The content type
        /// </summary>
        [NonSerialized] private Type contentType;

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the complexity.
        /// </summary>
        /// <value>The complexity.</value>
        public DealComplexity Complexity { get; set; } = DealComplexity.Standard;




        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public Type ContentType
        {
            get
            {
                if (contentType == null && ContentTypeName != null)
                    ContentType = Assemblies.FindType(ContentTypeName);
                return contentType;
            }
            set
            {
                if (value != null)
                {
                    ContentTypeName = value.FullName;
                    contentType = value;
                }
            }
        }




        /// <summary>
        /// Gets or sets the name of the content type.
        /// </summary>
        /// <value>The name of the content type.</value>
        public string ContentTypeName { get; set; }




        /// <summary>
        /// Gets or sets the echo.
        /// </summary>
        /// <value>The echo.</value>
        public string Echo { get; set; }




        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public int Errors { get; set; }




        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        /// <value>The identity.</value>
        public MemberIdentity Identity { get; set; }




        /// <summary>
        /// Gets or sets the identity site.
        /// </summary>
        /// <value>The identity site.</value>
        public ServiceSite IdentitySite { get; set; } = ServiceSite.Client;




        /// <summary>
        /// Gets or sets the objects count.
        /// </summary>
        /// <value>The objects count.</value>
        public int ObjectsCount { get; set; } = 0;




        /// <summary>
        /// Gets or sets a value indicating whether [receive message].
        /// </summary>
        /// <value><c>true</c> if [receive message]; otherwise, <c>false</c>.</value>
        public bool ReceiveMessage { get; set; } = true;




        /// <summary>
        /// Gets or sets a value indicating whether [send message].
        /// </summary>
        /// <value><c>true</c> if [send message]; otherwise, <c>false</c>.</value>
        public bool SendMessage { get; set; } = true;




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DealContext" /> is synchronic.
        /// </summary>
        /// <value><c>true</c> if synchronic; otherwise, <c>false</c>.</value>
        public bool Synchronic { get; set; } = false;

        #endregion
    }
}
