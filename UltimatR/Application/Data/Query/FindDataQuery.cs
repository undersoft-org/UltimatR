using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace UltimatR
{
    public class FindDataQuery<TStore, TEntity, TDto> : Query<TStore, TEntity, TDto>
        where TEntity : Entity where TStore : IDataStore
    {
        public FindDataQuery(params object[] keys) : base(keys) { }
        public FindDataQuery(object[] keys, params Expression<Func<TEntity, object>>[] expanders) : base(keys, expanders) { }
        public FindDataQuery(Expression<Func<TEntity, bool>> predicate) : base(predicate) { }
        public FindDataQuery(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders) : base(predicate, expanders) { }
    }

    public class FindDataQueryHandler<TStore, TEntity, TDto> : IRequestHandler<FindDataQuery<TStore, TEntity, TDto>, TDto>  
        where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;

        public FindDataQueryHandler(IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<TDto> Handle(FindDataQuery<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            if(request.Keys != null)
                return _repository.Find<TDto>(request.Keys, request.Expanders);
            return _repository.Find<TDto>(request.Predicate, false, request.Expanders);
        }
    }
}
