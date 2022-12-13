using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class UpdateDtoHandler<TStore, TEntity, TDto> : IRequestHandler<UpdateDto<TStore, TEntity, TDto> , DtoCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IUltimatr _ultimatr;

        public UpdateDtoHandler(IUltimatr ultimatr, IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _ultimatr = ultimatr;
        }

        public async Task<DtoCommand<TDto>> Handle(UpdateDto<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            if (!request.Result.IsValid) return request;

            try
            {                
                if (request.Predicate == null)
                    request.Entity = await _repository.SetBy(request.Data, request.Keys);
                else if (request.Conditions == null)
                    request.Entity = await _repository.SetBy(request.Data, request.Predicate);
                else
                    request.Entity = await _repository.SetBy(request.Data, request.Predicate, request.Conditions);

                if (request.Entity == null) throw new Exception($"{ this.GetType().Name } for entity " +
                                                                $"{ typeof(TEntity).Name } unable update entry");
            
                _ = _ultimatr.Publish(new UpdatedDto<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
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
