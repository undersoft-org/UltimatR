using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Series;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace UltimatR
{
    public class FilterDataQuery<TStore, TEntity, TDto> : Query<TStore, TEntity, IDeck<TDto>>
        where TEntity : Entity where TStore : IDataStore
    {
        public FilterDataQuery(Expression<Func<TEntity, bool>> predicate) : base(predicate) { }
        public FilterDataQuery(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders) : base(predicate, expanders) { }
        public FilterDataQuery(Expression<Func<TEntity, bool>> predicate, SortExpression<TEntity> sortTerms,
                                params Expression<Func<TEntity, object>>[] expanders) : base(predicate, sortTerms, expanders) { }
    }

    public class FilterDataQueryHandler<TStore, TEntity, TDto> : IRequestHandler<FilterDataQuery<TStore, TEntity, TDto>, IDeck<TDto>>
        where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;

        public FilterDataQueryHandler(IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<IDeck<TDto>> Handle(FilterDataQuery<TStore, TEntity, TDto> request,
            CancellationToken cancellationToken)
        {
            if(request.Predicate == null)
                return _repository.Filter<TDto>(0, 0, request.Sort, request.Expanders);
            return _repository.Filter<TDto>(0, 0, request.Predicate, request.Sort, request.Expanders);
        }
    }
}
