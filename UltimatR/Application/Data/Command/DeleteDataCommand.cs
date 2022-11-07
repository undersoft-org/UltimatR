using FluentValidation.Results;
using MediatR;
using System;
using System.Linq.Expressions;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class DeleteDataCommand<TStore, TEntity, TDto>  : DataCommand<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity,  Expression<Func<TEntity, bool>>> Predicate { get; }

        public DeleteDataCommand(PublishMode publishPattern, TDto input) 
            : base(CommandMode.Delete, publishPattern, input)
        {
        }
        public DeleteDataCommand(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
            : base(CommandMode.Delete, publishPattern, input)
        {
            Predicate = predicate;
        }
        public DeleteDataCommand(PublishMode publishPattern, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) : base(CommandMode.Delete, publishPattern)
        {
            Predicate = predicate;
        }
        public DeleteDataCommand(PublishMode publishPattern, params object[] keys) 
            : base(CommandMode.Delete, publishPattern, keys)
        {
        }
    }

    public class DeleteDataCommandHandler<TStore, TEntity, TDto> : IRequestHandler<DeleteDataCommand<TStore, TEntity, TDto> , DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IUltimatr _umaker;

        public DeleteDataCommandHandler(IUltimatr umaker, IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _umaker = umaker;
        }

        public async Task<DataCommand<TDto>> Handle(DeleteDataCommand<TStore, TEntity, TDto>  request, CancellationToken cancellationToken)
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

                _ = _umaker.Publish(new DeletedDataEvent<TStore, TEntity, TDto>(request)).ConfigureAwait(false); ;
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
