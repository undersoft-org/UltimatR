using FluentValidation.Results;
using MediatR;
using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class ChangeDtoHandler<TStore, TEntity, TDto> : IRequestHandler<ChangeDto<TStore, TEntity, TDto>, DtoCommand<TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IUltimatr _ultimatr;

        public ChangeDtoHandler(IUltimatr ultimatr, IDataRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public virtual async Task<DtoCommand<TDto>> Handle(ChangeDto<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            if (!request.Result.IsValid)
                return request;
            try
            {              
                if (request.Keys != null)
                    request.Entity = await _repository.PatchBy(request.Data, request.Keys).ConfigureAwait(false);
                else
                    request.Entity = await _repository.PatchBy(request.Data, request.Predicate).ConfigureAwait(false);

                if (request.Entity == null) throw new Exception($"{ GetType().Name } for entity " +
                                                                $"{ typeof(TEntity).Name } unable patch entry");                
                
                _ = _ultimatr.Publish(new ChangedDto<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                request.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                this.Failure<Applog>(ex.Message, request.ErrorMessages, ex);
            }

            return request;
        }
    }
}
