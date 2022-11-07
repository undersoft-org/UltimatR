using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    #region Interfaces

    public interface IDataContextPool : IDeck<IDataContext>, IDataContextFactory
    {
        int PoolSize { get; set; }

        void CreatePool();

        IDataContext Rent();

        void Return();

        Task ReturnAsync(CancellationToken cancellationToken = default);
    }

    public interface IDataContextPool<TContext> : IDataContextPool, IDataContextFactory<TContext>
        where TContext : class { }

    #endregion
}