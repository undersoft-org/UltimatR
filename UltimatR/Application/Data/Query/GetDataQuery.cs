using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Logs;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class GetDataQuery<TStore, TEntity, TDto> : Query<TStore, TEntity, IDeck<TDto>>
        where TEntity : Entity where TStore : IDataStore
    {
        public GetDataQuery(params Expression<Func<TEntity, object>>[] expanders) : base(expanders) { }

        public GetDataQuery(SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders) : base(sortTerms, expanders) { }
    }

    public class GetDataQueryHandler<TStore, TEntity, TDto> : IRequestHandler<GetDataQuery<TStore, TEntity, TDto>, IDeck<TDto>> 
        where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;

        public GetDataQueryHandler(IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IDeck<TDto>> Handle(GetDataQuery<TStore, TEntity, TDto> request,
                                                CancellationToken cancellationToken)
        {
            return await _repository.Get<TDto>(0, 0, request.Sort, request.Expanders);
        }
    }
}
