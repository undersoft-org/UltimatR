using MediatR;
using System;
using System.Linq.Expressions;
using System.Series;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class FilterEntityQuery<TStore, TEntity> : IRequest<IDeck<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        [JsonIgnore] public Expression<Func<TEntity, bool>> Predicate { get; }
        [JsonIgnore] public SortExpression<TEntity> Sort { get; }
        [JsonIgnore] public Expression<Func<TEntity, object>>[] Expanders { get; }

        public FilterEntityQuery(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }

        public FilterEntityQuery(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders)
        {
            Predicate = predicate;
            Expanders = expanders;
        }

        public FilterEntityQuery(Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms,
            params Expression<Func<TEntity, object>>[] expanders)
        {
            Predicate = predicate;
            Sort = sortTerms;
            Expanders = expanders;
        }
    }


    public class FilterEntityQueryHandler<TStore, TEntity> :  IRequestHandler<FilterEntityQuery<TStore, TEntity>, IDeck<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;

        public FilterEntityQueryHandler(IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;                       
        }

        public Task<IDeck<TEntity>> Handle(FilterEntityQuery<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return _repository.Filter(0, 0, request.Predicate, request.Sort, request.Expanders);
        }
    }


}
