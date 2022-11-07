using System;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using RTools_NTS.Util;

namespace UltimatR
{
    public class RepositoryLink<TStore, TOrigin, TTarget> : TeleRepository<TStore, TTarget>, IRepositoryLink<TStore, TOrigin, TTarget> where TOrigin : Entity where TTarget : Entity where TStore : IDataStore
    {
        private ILinkSynchronizer synchronizer;
        private IDsoRelation<TOrigin, TTarget> relation;

        public RepositoryLink(IDataContextPool<DsContext<TStore>> pool, 
                              IDataCache<TStore, TTarget> cache,                   
                              IDsoRelation<TOrigin, TTarget> relation,
                              ILinkSynchronizer synchronizer)
                              : base(pool, cache)
        {
            this.relation = relation;
            this.synchronizer = synchronizer;
        }

        public override Towards Towards => relation.Towards;

        public IRepository Host { get; set; }

        public ILinkSynchronizer Synchronizer => synchronizer;

        public Expression<Func<TOrigin, object>> OriginKey
        {
            get => relation.OriginKey;
            set => relation.OriginKey=value;
        }
        public Expression<Func<TTarget, object>> TargetKey
        {
            get => relation.TargetKey;
            set => relation.TargetKey=value;
        }
      
        public Func<TOrigin, Expression<Func<TTarget, bool>>> Predicate
        {
            get => relation.Predicate;
            set => relation.Predicate=value;
        }

        public MemberRubric LinkedMember => relation.LinkedMember;

        public override int LinkedCount { get; set; } 

        public bool IsLinked { get; set; }

        public void Load(object origin)
        {
           Load(origin, dsContext);
        }

        public async Task LoadAsync(object origin)
        {
            await Task.Run(() => { Load(origin, dsContext); }, Cancellation);
        }

        public void Load<T>(IEnumerable<T> origins, DsContext context) where T : class
        {
            origins.DoEach((o) => Load(o, context));
        }

        public void Load(object origin, DsContext context)
        {
            var _entity = (IEntity)origin;
            var rubricId = LinkedMember.RubricId;

            var predicate = CreatePredicate(origin);
            if (predicate != null)
            {
                IDso<TTarget> dso;
                switch (Towards)
                {
                    case Towards.ToSingle:
                        var query = context.CreateQuery<TTarget>(typeof(TTarget).Name);
                        synchronizer.AcquireLinker();
                        _entity[rubricId] = query.FirstOrDefault(predicate);
                        synchronizer.ReleaseLinker();
                        break;
                    case Towards.ToSet:
                        dso = typeof(DsoToSet<TTarget>).New<DsoToSet<TTarget>>(context);
                        dso.LoadCompleted += synchronizer.OnLinked;
                        _entity[rubricId] = dso;
                        synchronizer.AcquireLinker();
                        dso.LoadAsync(predicate);
                        break;
                    case Towards.SetToSet:
                        dso = typeof(DsoSetToSet<TTarget>).New<DsoSetToSet<TTarget>>(context);
                        dso.LoadCompleted += synchronizer.OnLinked;
                        _entity[rubricId] = dso;
                        synchronizer.AcquireLinker();
                        dso.LoadAsync(predicate);
                        break;
                    default:
                        break;
                }
            }
        }

        public async ValueTask LoadAsync(object origin, DsContext context, CancellationToken token)
        {
            await Task.Run(() => { Load(origin, context); }, token);
        }

        public Expression<Func<TTarget, bool>> CreatePredicate(object entity)
        {
           return relation.CreatePredicate(entity);
        }

        public override Task<int> Save(bool asTransaction, CancellationToken token = default)
        {
            return ContextLease.Save(asTransaction, token);
        }

    }
}
