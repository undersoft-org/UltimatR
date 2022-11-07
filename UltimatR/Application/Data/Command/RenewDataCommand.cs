using System;
using System.Linq.Expressions;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UltimatR
{
    public class RenewDataCommand<TStore, TEntity, TDto>  : DataCommand<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public RenewDataCommand(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Renew, publishPattern, input)
        {
            Predicate = predicate;
        }
        public RenewDataCommand(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
            : base(CommandMode.Renew, publishPattern, input)
        {
            Predicate = predicate;
            Conditions = conditions;
        }
    }

    public class RenewDataCommandHandler<TStore, TEntity, TDto> : IRequestHandler<RenewDataCommand<TStore, TEntity, TDto> , DataCommand<TDto>> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IUltimatr _umaker;

        public RenewDataCommandHandler(IUltimatr umaker, IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _umaker = umaker;
        }

        public async Task<DataCommand<TDto>> Handle(RenewDataCommand<TStore, TEntity, TDto>  request, CancellationToken cancellationToken)
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

                _ = _umaker.Publish(new RenewedDataEvent<TStore, TEntity, TDto>(request)).ConfigureAwait(false); ;
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
