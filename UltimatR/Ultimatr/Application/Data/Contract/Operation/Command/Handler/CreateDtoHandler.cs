using FluentValidation.Results;
using MediatR;
using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class CreateDtoHandler<TStore, TEntity, TDto>  : IRequestHandler<CreateDto<TStore, TEntity, TDto> , DtoCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;       
        protected readonly IUltimatr _ultimatr;

        public CreateDtoHandler(IUltimatr ultimatr, IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _ultimatr = ultimatr;
        }

        public async Task<DtoCommand<TDto>> Handle(CreateDto<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            if (!request.Result.IsValid) 
                return request;
            try
            {
                request.Entity= await _repository.AddBy(request.Data, request.Predicate).ConfigureAwait(false);

                if (request.Entity == null) throw new Exception($"{ GetType().Name } " +
                                                                $"for entity { typeof(TEntity).Name } " +
                                                                $"unable create entry");              

                _ = _ultimatr.Publish(new CreatedDto<TStore, TEntity, TDto>(request)).ConfigureAwait(false); ;
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
