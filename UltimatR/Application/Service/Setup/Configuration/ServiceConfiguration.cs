using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace UltimatR
{
    public static class ConfigutrationExtensions
    {
        public static IServiceConfiguration BuildConfiguration(this IConfiguration config)
        {
            return new ServiceConfiguration(config);
        }       
    }

    public class ServiceConfiguration : IServiceConfiguration
    {
        private IConfiguration config;

        public string this[string key] { get => config[key]; set => config[key] = value; }

        public ServiceConfiguration()
        {
            config = ConfigurationHelper.BuildConfiguration();
        }
        public ServiceConfiguration(IConfiguration config)
        {           
            this.config = config;
        }

        public string Version     => config["Version"];
        public string Title       => config["Title"];
        public string Description => config["Description"];

        public string DsRoutePrefix(string name)
        {
            return config.GetSection("DsRoutePrefix")[name];
        }

        public IConfigurationSection UltimateService()
        {
            return config.GetSection("UltimateService");
        }

        public IEnumerable<IConfigurationSection> Endpoints()
        {
            return config.GetSection("UltimateService").GetSection("Endpoints").GetChildren();
        }
        public IEnumerable<IConfigurationSection> Clients()
        {
            return UltimateService().GetSection("Clients").GetChildren();
        }

        public IConfigurationSection Endpoint(string name)
        {
            return UltimateService()?.GetSection("Endpoints")?.GetSection(name);
        }

        public string EndpointConnectionString(string name)
        {
            return Endpoint(name)["ConnectionString"];
        }
        public string ClientConnectionString(string name)
        {
            return Client(name)["ConnectionString"];
        }

        public string EndpointConnectionString(IConfigurationSection endpoint)
        {
            string connStr = Environment.GetEnvironmentVariable(endpoint.Key);
            if(!String.IsNullOrEmpty(connStr))
            {
                Console.WriteLine($"Connection string for {endpoint.Key} from environment variable");
            }
            var result=connStr ?? endpoint["ConnectionString"];
            return result;
            
            //return endpoint["ConnectionString"];
        }
        public string ClientConnectionString(IConfigurationSection client)
        {
            return client["ConnectionString"];
        }

        public IConfigurationSection Client(string name)
        {
            return UltimateService()?.GetSection("Clients")?.GetSection(name);
        }

        public EndpointProvider EndpointProvider(string name)
        {
            Enum.TryParse(Endpoint(name)["EndpointProvider"], out EndpointProvider result);
            return result;
        }
        public ClientProvider ClientProvider(string name)
        {
            Enum.TryParse(Client(name)["ClientProvider"], out ClientProvider result);
            return result;
        }

        public EndpointProvider EndpointProvider(IConfigurationSection endpoint)
        {
            Enum.TryParse(endpoint["EndpointProvider"], out EndpointProvider result);
            return result;
        }
        public ClientProvider ClientProvider(IConfigurationSection client)
        {
            Enum.TryParse(client["ClientProvider"], out ClientProvider result);
            return result;
        }

        public int EndpointPoolSize(IConfigurationSection endpoint)
        {
            return endpoint.GetValue<int>("PoolSize");            
        }
        public int ClientPoolSize(IConfigurationSection client)
        {
            return client.GetValue<int>("PoolSize");
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return config.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return config.GetReloadToken();
        }

        public IConfigurationSection GetSection(string key)
        {
            return config.GetSection(key);
        }
        public IConfigurationSection IdentityServer()
        {
            return config.GetSection("IdentityServer");
        }
        public string IdentityServerAddress()
        {
            return IdentityServer().GetValue<string>("Address");
        }
        public string IdentityServerApiName()
        {
            return IdentityServer().GetValue<string>("ApiName");
        }
        public IEnumerable<IConfigurationSection> IdentityServerScopes()
        {
            return IdentityServer()?.GetSection("Scopes")?.GetChildren();
        }
    }
}
