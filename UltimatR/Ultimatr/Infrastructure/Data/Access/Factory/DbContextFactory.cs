using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Instant.Sqlset;

namespace UltimatR
{
    public class DbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>, IDbContextFactory<TContext> where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            var config = new ServiceConfiguration();
            var configEndpoint = config.Endpoint(typeof(TContext).FullName);
            var options = RepositoryEndpointOptions.BuildOptions<TContext>(config.EndpointProvider(configEndpoint), 
                                                                           config.EndpointConnectionString(configEndpoint)).Options;
            return typeof(TContext).New<TContext>(options);
        }

        public TContext CreateDbContext()
        {
            if (RepositoryManager.TryGetEndpoint<TContext>(out var endpoint))
                return endpoint.CreateContext<TContext>();
            return null;
        }        
    }  
}