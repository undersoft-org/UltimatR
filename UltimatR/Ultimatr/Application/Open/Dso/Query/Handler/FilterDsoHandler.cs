using MediatR;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class FilterDsoHandler<TStore, TEntity> :  IRequestHandler<FilterDso<TStore, TEntity>, IDeck<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;

        public FilterDsoHandler(IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;                       
        }

        public Task<IDeck<TEntity>> Handle(FilterDso<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return _repository.Filter(0, 0, request.Predicate, request.Sort, request.Expanders);
        }
    }


}
