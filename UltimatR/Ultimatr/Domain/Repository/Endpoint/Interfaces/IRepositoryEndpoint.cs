//-----------------------------------------------------------------------
// <copyright file="IRepositoryEndpoint.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.OData.ModelBuilder;
using System;
using System.Linq;

namespace UltimatR
{
    public interface IRepositoryEndpoint<TStore, TEntity> : IRepositoryEndpoint where TEntity : Entity
    {
        EntitySetConfiguration<TEntity> EntitySet();

        IQueryable<TEntity> FromSql(string sql, params object[] parameters);

        DbSet<TEntity> GetDbSet();
    }

    public interface IRepositoryEndpoint : IRepositoryContextPool
    {
        IDataContext CreateContext(DbContextOptions options);

        IDataContext CreateContext(Type contextType, DbContextOptions options);

        TModel CreateServiceModel<TModel>();

        TModel GetServiceModel<TModel>();

        object ServiceEntitySet<TEntity>() where TEntity : class, IIdentifiable;

        object ServiceEntitySet(Type entityType);

        IDataContext Context { get; }

        DbContextOptions Options { get; }
    }

    public interface IRepositoryEndpoint<TContext> : IRepositoryContextPool<TContext>, IDesignTimeDbContextFactory<TContext>, IDbContextFactory<TContext>, IRepositoryEndpoint
        where TContext : DbContext
    {
        TContext CreateContext(DbContextOptions<TContext> options);

        new DbContextOptions<TContext> Options { get; }
    }
}
