using System;
using Microsoft.EntityFrameworkCore;

namespace UltimatR
{
    public class DbContext<TStore> : DbContextStore where TStore : IDataStore
    {
        protected virtual Type StoreType { get; }

        public DbContext(DbContextOptions options) : base(options)
        {
            StoreType = typeof(TStore);
        }
    }

    public class DbContextStore : DbContext, IDbContextStore
    {
        public virtual IUltimatr Ultimatr { get; }

        public DbContextStore(DbContextOptions options, IUltimatr ultimatr = null) : base(options)
        {
            Ultimatr = ultimatr;
        }
    }

    public interface IDbContextStore
    {
        IUltimatr Ultimatr { get; }
    }


}