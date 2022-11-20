//-----------------------------------------------------------------------
// <copyright file="RepositoryEndpointOptions.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace UltimatR
{
    public static class RepositoryEndpointOptions
    {
        static IServiceRegistry AddEntityServicesForDb(EndpointProvider provider)
        {
            IServiceManager sm = ServiceManager.GetManager();
            if(!DbRegistry.Providers.ContainsKey((int)provider))
            {
                switch(provider)
                {
                    case EndpointProvider.SqlServer:
                        sm.Registry.AddEntityFrameworkSqlServer();
                        break;
                    case EndpointProvider.AzureSql:
                        sm.Registry.AddEntityFrameworkSqlServer();
                        break;
                    case EndpointProvider.PostgreSql:
                        sm.Registry.AddEntityFrameworkNpgsql();
                        break;
                    case EndpointProvider.SqlLite:
                        sm.Registry.AddEntityFrameworkSqlite();
                        break;
                    case EndpointProvider.MariaDb:
                        sm.Registry.AddEntityFrameworkMySql();
                        break;
                    case EndpointProvider.MySql:
                        sm.Registry.AddEntityFrameworkMySql();
                        break;
                    case EndpointProvider.Oracle:
                        sm.Registry.AddEntityFrameworkOracle();
                        break;
                    case EndpointProvider.CosmosDb:
                        sm.Registry.AddEntityFrameworkCosmos();
                        break;
                    case EndpointProvider.MemoryDb:
                        sm.Registry.AddEntityFrameworkInMemoryDatabase();
                        break;
                    default:
                        break;
                }
                sm.Registry.AddEntityFrameworkProxies();
                DbRegistry.Providers.Add((int)provider, provider);
            }
            return sm.Registry;
        }

        public static DbContextOptionsBuilder<TContext> BuildOptions<TContext>(
            EndpointProvider provider,
            string connectionString)
            where TContext : DbContext
        {
            return (DbContextOptionsBuilder<TContext>)BuildOptions(
                new DbContextOptionsBuilder<TContext>(),
                provider,
                connectionString)
                .ConfigureWarnings(w => w.Ignore(CoreEventId.DetachedLazyLoadingWarning));
        }

        public static DbContextOptionsBuilder BuildOptions(EndpointProvider provider, string connectionString)
        {
            return BuildOptions(new DbContextOptionsBuilder(), provider, connectionString)
                .ConfigureWarnings(w => w.Ignore(CoreEventId.DetachedLazyLoadingWarning));
        }

        public static DbContextOptionsBuilder BuildOptions(
            DbContextOptionsBuilder builder,
            EndpointProvider provider,
            string connectionString)
        {
            switch(provider)
            {
                case EndpointProvider.SqlServer:
                    return builder
                        .UseSqlServer(connectionString)
                        .UseLazyLoadingProxies();
                case EndpointProvider.AzureSql:
                    return builder
                        .UseSqlServer(connectionString)
                        .UseLazyLoadingProxies();

                case EndpointProvider.PostgreSql:
                    return builder
                        .UseNpgsql(connectionString)
                        .UseLazyLoadingProxies();

                case EndpointProvider.SqlLite:
                    return builder
                        .UseSqlite(connectionString)
                        .UseLazyLoadingProxies();

                case EndpointProvider.MariaDb:
                    return builder
                        .UseMySql(
                            ServerVersion
                            .AutoDetect(connectionString))
                        .UseLazyLoadingProxies();

                case EndpointProvider.MySql:
                    return builder
                        .UseMySql(
                            ServerVersion
                            .AutoDetect(connectionString))
                        .UseLazyLoadingProxies();

                case EndpointProvider.Oracle:
                    return builder
                        .UseOracle(connectionString)
                        .UseLazyLoadingProxies();

                case EndpointProvider.CosmosDb:
                    return builder
                        .UseCosmos(
                            connectionString.Split('#')[0],
                            connectionString.Split('#')[1],
                            connectionString.Split('#')[2])
                        .UseLazyLoadingProxies();

                case EndpointProvider.MemoryDb:
                    return builder.UseInternalServiceProvider(new ServiceManager())
                        .UseInMemoryDatabase(connectionString)
                        .UseLazyLoadingProxies()
                        .ConfigureWarnings(
                            w => w.Ignore(
                                InMemoryEventId
                                .TransactionIgnoredWarning));
                default:
                    break;
            }

            return builder;
        }
    }

    public enum EndpointProvider
    {
        None,
        SqlServer,
        MemoryDb,
        AzureSql,
        PostgreSql,
        SqlLite,
        MySql,
        MariaDb,
        Oracle,
        CosmosDb,
        MongoDb,
        FileSystem
    }

    public enum ClientProvider
    {
        None,
        OData,
        RabbitMQ,
        gRPC
    }
}
