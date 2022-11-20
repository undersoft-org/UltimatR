using MediatR;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class GetDsoHandler<TStore, TEntity> : IRequestHandler<GetDso<TStore, TEntity>, IDeck<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;

        public GetDsoHandler(IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<IDeck<TEntity>> Handle(GetDso<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return _repository.Get(0, 0, request.Sort, request.Expanders);
        }
    }
}
