using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Client;

namespace UltimatR
{
    #region Interfaces

    public interface IRepositoryClient : IRepositoryContextPool, IUnique, IDisposable, IAsyncDisposable
    {
        #region Properties        

        DsContext Context { get; }

        Uri Route { get; }        

        #endregion

        #region Methods

        TContext GetContext<TContext>() where TContext : DsContext;        

        object CreateContext(Type contextType, Uri serviceRoot);
        TContext CreateContext<TContext>(Uri serviceRoot) where TContext : DsContext;

        void BuildMetadata();

        #endregion
    }

    public interface IRepositoryClient<TContext> : IRepositoryContextPool<TContext>,
                                                   IRepositoryClient where TContext : class
    {
        #region Properties

        new TContext Context { get; }

        #endregion

        TContext CreateContext(Uri serviceRoot);
    }

    #endregion
}
