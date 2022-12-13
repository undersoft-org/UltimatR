using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class FindDsoHandler<TStore, TEntity> : IRequestHandler<FindDso<TStore, TEntity>, TEntity> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;

        public FindDsoHandler(IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<TEntity> Handle(FindDso<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            if (request.Predicate == null)
                return _repository.Find(request.Keys, request.Expanders);
            return _repository.Find(request.Predicate, false, request.Expanders);
        }
    }

}
