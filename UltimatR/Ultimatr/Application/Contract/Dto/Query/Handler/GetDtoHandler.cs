using MediatR;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class GetDtoHandler<TStore, TEntity, TDto> : IRequestHandler<GetDto<TStore, TEntity, TDto>, IDeck<TDto>> 
        where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;

        public GetDtoHandler(IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IDeck<TDto>> Handle(GetDto<TStore, TEntity, TDto> request,
                                                CancellationToken cancellationToken)
        {
            return await _repository.Get<TDto>(0, 0, request.Sort, request.Expanders);
        }
    }
}
