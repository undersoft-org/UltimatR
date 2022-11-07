
// <copyright file="SqlIdentity.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    #region Enums

    /// <summary>
    /// Enum SqlProvider
    /// </summary>
    public enum SqlProvider
    {
        /// <summary>
        /// The ms SQL
        /// </summary>
        MsSql,
        /// <summary>
        /// My SQL
        /// </summary>
        MySql,
        /// <summary>
        /// The postgres
        /// </summary>
        Postgres,
        /// <summary>
        /// The oracle
        /// </summary>
        Oracle,
        /// <summary>
        /// The SQL lite
        /// </summary>
        SqlLite
    }

    #endregion




    /// <summary>
    /// Class SqlIdentity.
    /// </summary>
    [Serializable]
    public class SqlIdentity
    {
        #region Fields

        /// <summary>
        /// The authentication identifier
        /// </summary>
        public string AuthId;
        /// <summary>
        /// The database
        /// </summary>
        public string Database;
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id;
        /// <summary>
        /// The name
        /// </summary>
        public string Name;
        /// <summary>
        /// The password
        /// </summary>
        public string Password;
        /// <summary>
        /// The port
        /// </summary>
        public int Port;
        /// <summary>
        /// The provider
        /// </summary>
        public SqlProvider Provider;
        /// <summary>
        /// The security
        /// </summary>
        public bool Security;
        /// <summary>
        /// The server
        /// </summary>
        public string Server;
        /// <summary>
        /// The user identifier
        /// </summary>
        public string UserId;

        #endregion

        #region Constructors




        /// <summary>
        /// Initializes a new instance of the <see cref="SqlIdentity" /> class.
        /// </summary>
        public SqlIdentity()
        {
        }




        /// <summary>
        /// Initializes a new instance of the <see cref="SqlIdentity" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlIdentity(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Properties




        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get
            {

                string cn = string.Format("server={0}{1};Persist Security Info={2};password={3};User ID={4};database={5}",
                                           Server,
                                           (Port != 0) ? ":" + Port.ToString() : "",
                                           Security.ToString(),
                                           Password,
                                           UserId,
                                           Database);
                return cn;
            }
            set
            {
                string cn = value;
                string[] opts = cn.Split(';');
                foreach (string opt in opts)
                {
                    string name = opt.Split('=')[0].ToLower();
                    string val = opt.Split('=')[1];
                    switch (name)
                    {
                        case "server":
                            Server = val;
                            break;
                        case "persist security info":
                            Security = Boolean.Parse(val);
                            break;
                        case "password":
                            Password = val;
                            break;
                        case "user id":
                            UserId = val;
                            break;
                        case "database":
                            Database = val;
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
