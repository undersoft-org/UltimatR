using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class RenewDtoHandler<TStore, TEntity, TDto> : IRequestHandler<RenewDto<TStore, TEntity, TDto> , DtoCommand<TDto>> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IUltimatr _umaker;

        public RenewDtoHandler(IUltimatr umaker, IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _umaker = umaker;
        }

        public async Task<DtoCommand<TDto>> Handle(RenewDto<TStore, TEntity, TDto>  request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                if (!request.Result.IsValid)
                return request;

            try
            {                
                if (request.Conditions != null)
                    request.Entity = await _repository.PutBy(request.Data, request.Predicate, request.Conditions);
                else
                    request.Entity = await _repository.PutBy(request.Data, request.Predicate);

                if (request.Entity == null) throw new Exception($"{ GetType().Name } " +
                              $"for entity { typeof(TEntity).Name } unable renew entry");              

                _ = _umaker.Publish(new RenewedDto<TStore, TEntity, TDto>(request)).ConfigureAwait(false); ;
            }
            catch (Exception ex)
            {
                request.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                this.Failure<Applog>(ex.Message, request.ErrorMessages, ex);
            }

            return request;
            }, cancellationToken);
        }
    }
}
