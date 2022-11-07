using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class GetEntityQuery<TStore, TEntity> : IRequest<IDeck<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        public SortExpression<TEntity> Sort { get; }
        public Expression<Func<TEntity, object>>[] Expanders { get; }

        public GetEntityQuery(params Expression<Func<TEntity, object>>[] expanders)
        {
            Expanders = expanders;
        }

        public GetEntityQuery(SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders)
        {
            Sort = sortTerms;
            Expanders = expanders;
        }
    }

    public class GetEntityQueryHandler<TStore, TEntity> : IRequestHandler<GetEntityQuery<TStore, TEntity>, IDeck<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;

        public GetEntityQueryHandler(IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<IDeck<TEntity>> Handle(GetEntityQuery<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return _repository.Get(0, 0, request.Sort, request.Expanders);
        }
    }
}
