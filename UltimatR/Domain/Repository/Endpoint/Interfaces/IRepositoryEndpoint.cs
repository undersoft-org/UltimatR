using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using System.Linq;

namespace UltimatR
{
    #region Interfaces

    public interface IRepositoryEndpoint<TStore, TEntity> : IRepositoryEndpoint where TEntity : Entity
    {
        DbSet<TEntity> GetDbSet();
            
        EntitySetConfiguration<TEntity> EntitySet();

        IQueryable<TEntity> FromSql(string sql, params object[] parameters);
    }
    
    public interface IRepositoryEndpoint : IDataContextPool
    {
        #region Properties           

        DbContext Context { get; }

        DbContextOptions Options { get; }

        #endregion

        #region Methods        

        DbContext CreateContext(DbContextOptions options);
        DbContext CreateContext(Type contextType, DbContextOptions options);

        EntitySetConfiguration<TEntity> EntitySet<TEntity>() where TEntity : Entity;
        EntitySetConfiguration EntitySet(Type entityType);

        IEdmModel CreateEdmModel();

        IEdmModel GetEdmModel();

        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters)
            where TEntity : class;

        int ExecuteSql(string sql, params object[] parameters);

        #endregion
    }

    public interface IRepositoryEndpoint<TContext> : IDataContextPool<TContext>,
                                                     IDesignTimeDbContextFactory<TContext>, 
                                                     IDbContextFactory<TContext>, 
                                                     IRepositoryEndpoint where TContext : DbContext
    {
        #region Properties

        new DbContextOptions<TContext> Options { get; }

        #endregion

        #region Methods        

        TContext CreateContext(DbContextOptions<TContext> options);

        #endregion
    }

    #endregion
}
