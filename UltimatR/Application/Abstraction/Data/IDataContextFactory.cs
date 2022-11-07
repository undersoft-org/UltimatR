using Microsoft.EntityFrameworkCore;
using System;

namespace UltimatR
{
    #region Interfaces

    public interface IDataContextFactory : IDataContext, IAsyncDisposable, IDisposable
    {
        object CreateContext();

        TContext CreateContext<TContext>() where TContext : class;
    }

    public interface IDataContextFactory<TContext> : IDataContextFactory, IDataContext<TContext>, IAsyncDisposable,
        IDisposable where TContext : class
    {
        new TContext CreateContext();
    }

    #endregion
}