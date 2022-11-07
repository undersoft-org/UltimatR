
// <copyright file="MemberIdentity.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    #region Enums

    /// <summary>
    /// Enum ServiceSite
    /// </summary>
    [Serializable]
    public enum ServiceSite
    {
        /// <summary>
        /// The client
        /// </summary>
        Client,
        /// <summary>
        /// The server
        /// </summary>
        Server
    }
    /// <summary>
    /// Enum DataSite
    /// </summary>
    public enum DataSite
    {
        /// <summary>
        /// The none
        /// </summary>
        None,
        /// <summary>
        /// The client
        /// </summary>
        Client,
        /// <summary>
        /// The endpoint
        /// </summary>
        Endpoint
    }
    /// <summary>
    /// Enum IdentityType
    /// </summary>
    public enum IdentityType
    {
        /// <summary>
        /// The user
        /// </summary>
        User,
        /// <summary>
        /// The server
        /// </summary>
        Server,
        /// <summary>
        /// The service
        /// </summary>
        Service
    }

    #endregion




    /// <summary>
    /// Class MemberIdentity.
    /// </summary>
    [Serializable]
    public class MemberIdentity
    {
        #region Fields

        /// <summary>
        /// The site
        /// </summary>
        public ServiceSite Site;
        /// <summary>
        /// The type
        /// </summary>
        public IdentityType Type;

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MemberIdentity" /> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }




        /// <summary>
        /// Gets or sets the authentication identifier.
        /// </summary>
        /// <value>The authentication identifier.</value>
        public string AuthId { get; set; }




        /// <summary>
        /// Gets or sets the data place.
        /// </summary>
        /// <value>The data place.</value>
        public string DataPlace { get; set; }




        /// <summary>
        /// Gets or sets the dept identifier.
        /// </summary>
        /// <value>The dept identifier.</value>
        public string DeptId { get; set; }




        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host { get; set; }




        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }




        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>The ip.</value>
        public string Ip { get; set; }




        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }




        /// <summary>
        /// Gets or sets the last action.
        /// </summary>
        /// <value>The last action.</value>
        public DateTime LastAction { get; set; }




        /// <summary>
        /// Gets or sets the life time.
        /// </summary>
        /// <value>The life time.</value>
        public DateTime LifeTime { get; set; }




        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>The limit.</value>
        public int Limit { get; set; }




        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }




        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; }




        /// <summary>
        /// Gets or sets the register time.
        /// </summary>
        /// <value>The register time.</value>
        public DateTime RegisterTime { get; set; }




        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public string Salt { get; set; }




        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public int Scale { get; set; }




        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }




        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }

        #endregion
    }
}
