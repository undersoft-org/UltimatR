using MediatR;
using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class FindEntityQuery<TStore, TEntity> : IRequest<TEntity> where TEntity : Entity where TStore : IDataStore
    {
        [JsonIgnore] public object[] Keys { get; }
        [JsonIgnore] public Expression<Func<TEntity, bool>> Predicate { get; }
        [JsonIgnore] public Expression<Func<TEntity, object>>[] Expanders { get; }

        public FindEntityQuery(params object[] keys)
        {
            Keys = keys;
        }

        public FindEntityQuery(object[] keys, params Expression<Func<TEntity, object>>[] expanders)
        {
            Keys = keys;
            Expanders = expanders;
        }

        public FindEntityQuery(Expression<Func<TEntity, bool>> predicate)  
        {
            Predicate = predicate;
        }

        public FindEntityQuery(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] expanders)
        {
            Predicate = predicate;
            Expanders = expanders;
        }
    }

    public class FindEntityQueryHandler<TStore, TEntity> : IRequestHandler<FindEntityQuery<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;

        public FindEntityQueryHandler(IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<TEntity> Handle(FindEntityQuery<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            if (request.Predicate == null)
                return _repository.Find(request.Keys, request.Expanders);
            return _repository.Find(request.Predicate, false, request.Expanders);
        }
    }

}
