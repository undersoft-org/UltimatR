using FluentValidation.Results;
using MediatR;
using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class DeleteDtoHandler<TStore, TEntity, TDto> : IRequestHandler<DeleteDto<TStore, TEntity, TDto> , DtoCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IUltimatr _umaker;

        public DeleteDtoHandler(IUltimatr umaker, IDataRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _umaker = umaker;
        }

        public async Task<DtoCommand<TDto>> Handle(DeleteDto<TStore, TEntity, TDto>  request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                if (!request.Result.IsValid)
                return request;

            try
            {                
                if (request.Keys != null)
                    request.Entity = await _repository.Delete(request.Keys);
                else if (request.Data == null && request.Predicate != null)
                    request.Entity = await _repository.Delete(request.Predicate);
                else
                    request.Entity = await _repository.DeleteBy(request.Data, request.Predicate);

                if (request.Entity == null) throw new Exception($"{ this.GetType().Name } for entity" +
                                                                $" { typeof(TEntity).Name } unable delete entry");             

                _ = _umaker.Publish(new DeletedDto<TStore, TEntity, TDto>(request)).ConfigureAwait(false); ;
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
