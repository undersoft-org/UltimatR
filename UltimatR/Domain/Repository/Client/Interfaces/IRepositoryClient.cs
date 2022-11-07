using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Client;

namespace UltimatR
{
    #region Interfaces

    public interface IRepositoryClient : IDataContextPool, IUnique, IDisposable, IAsyncDisposable
    {
        #region Properties        

        DsContext Context { get; }

        Uri Route { get; }        

        #endregion

        #region Methods

        TContext GetContext<TContext>() where TContext : DsContext;        

        DsContext CreateContext(Uri serviceRoot);
        DsContext CreateContext(Type contextType, Uri serviceRoot);

        TContext CreateContext<TContext>(Uri serviceRoot) where TContext : DsContext;

        void GetClientEdm();        

        #endregion
    }

    public interface IRepositoryClient<TContext> : IDataContextPool<TContext>,
                                                   IRepositoryClient where TContext : class
    {
        #region Properties

        new TContext Context { get; }

        #endregion

        new TContext CreateContext(Uri serviceRoot);
    }

    #endregion
}
