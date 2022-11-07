using System;
using System.Instant.Sqlset;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace System.Instant.Sqlset
{
    public class SqlContext : Sqlbase
    {
        public SqlContext(SqlIdentity identity)  : base(identity) { }
        public SqlContext(string connectionString) : base(connectionString) { }
        public SqlContext(IConfiguration configuration, string connectionName) : base(configuration.GetConnectionString(connectionName)) { }
        public SqlContext(IConfiguration configuration) : base(configuration.GetSection("ConnectionString")?
                                                                            .GetChildren()?
                                                                            .FirstOrDefault()?.Value) { }


    }
}
