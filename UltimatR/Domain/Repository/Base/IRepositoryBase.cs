using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace UltimatR
{
    public interface IRepository : IDataContext
    {       
        Type ElementType { get;  }

        Expression Expression { get;  }        

        IQueryProvider Provider { get; }

        IDataMapper Mapper { get; }

        CancellationToken Cancellation { get; set; }

        IEnumerable<ILinkedObject> LinkedObjects { get; set; }

        EntityEntry LastEntry { get; }

        void ChangeDetecting(bool enable);

        void QueryTracking(bool enable);

        void LazyLoading(bool enable);

        void AutoTransaction(bool enable);

        void TrackingEvents(bool enable = true);

        void LoadLinked(object entity);

        Task LoadLinkedAsync(object entity);

        void LoadRelated(EntityEntry entry, RelatedType relatedType);

        void LoadRelatedAsync(EntityEntry entry, RelatedType relatedType, CancellationToken token);
    }
}