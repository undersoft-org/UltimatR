using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace UltimatR
{
    public class FindDtoHandler<TStore, TEntity, TDto> : IRequestHandler<FindDto<TStore, TEntity, TDto>, TDto>  
        where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;

        public FindDtoHandler(IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<TDto> Handle(FindDto<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            if(request.Keys != null)
                return _repository.Find<TDto>(request.Keys, request.Expanders);
            return _repository.Find<TDto>(request.Predicate, false, request.Expanders);
        }
    }
}
