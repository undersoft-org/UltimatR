using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace UltimatR
{
    public interface IServiceConfiguration : IConfiguration
    {
        string Version { get; }

        IConfigurationSection Client(string name);
        IConfigurationSection DataCacheLifeTime();
        int ClientPoolSize(IConfigurationSection endpoint);
        string ClientConnectionString(IConfigurationSection client);
        string ClientConnectionString(string name);
        ClientProvider ClientProvider(IConfigurationSection client);
        ClientProvider ClientProvider(string name);
        IEnumerable<IConfigurationSection> Clients();
        string Description { get; }
        string DsoControllerRoute(string name);
        IConfigurationSection Endpoint(string name);
        int EndpointPoolSize(IConfigurationSection endpoint);
        string EndpointConnectionString(IConfigurationSection endpoint);
        string EndpointConnectionString(string name);
        EndpointProvider EndpointProvider(IConfigurationSection endpoint);
        EndpointProvider EndpointProvider(string name);
        IEnumerable<IConfigurationSection> Endpoints();
        string Title { get; }
        IConfigurationSection Repository();
        IConfigurationSection IdentityServer();
        string IdentityServerAddress();
        string IdentityServerApiName();
        IEnumerable<IConfigurationSection> IdentityServerScopes();

    }
}