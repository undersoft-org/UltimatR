using System.Series;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace UltimatR
{
    public class FilterDtoHandler<TStore, TEntity, TDto> : IRequestHandler<FilterDto<TStore, TEntity, TDto>, IDeck<TDto>>
        where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;

        public FilterDtoHandler(IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<IDeck<TDto>> Handle(FilterDto<TStore, TEntity, TDto> request,
            CancellationToken cancellationToken)
        {
            if(request.Predicate == null)
                return _repository.Filter<TDto>(0, 0, request.Sort, request.Expanders);
            return _repository.Filter<TDto>(0, 0, request.Predicate, request.Sort, request.Expanders);
        }
    }
}
