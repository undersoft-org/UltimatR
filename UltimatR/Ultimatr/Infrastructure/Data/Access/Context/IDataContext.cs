using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UltimatR
{
    #region Interfaces

    public interface IDataContext : IResettableService, IDisposable, IAsyncDisposable
    {
        IModel Model { get; }

        IQueryable<TEntity> DataSet<TEntity>() where TEntity : class, IIdentifiable;

        object ServiceEntitySet<TEntity>() where TEntity : class, IIdentifiable;

        object ServiceEntitySet(Type entityType);

        TModel CreateServiceModel<TModel>();

        TModel GetServiceModel<TModel>();

        Task<int> Save(bool asTransaction, CancellationToken token = default(CancellationToken));
    }

    public interface IDataContext<TStore> : IDataContext where TStore : IDataStore
    {
    }  

    #endregion
}