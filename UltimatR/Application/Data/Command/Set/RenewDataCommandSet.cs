using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class RenewDataCommandSet<TStore, TEntity, TDto>  : CommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public RenewDataCommandSet(PublishMode publishPattern, TDto[] inputs, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Renew, publishPattern, inputs.Select(input => new RenewDataCommand<TStore, TEntity, TDto>(publishPattern, input, predicate)).ToArray())
        {
            Predicate = predicate;
        }
        public RenewDataCommandSet(PublishMode publishPattern, TDto[] inputs, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions)
           : base(CommandMode.Renew, publishPattern, inputs.Select(input => new RenewDataCommand<TStore, TEntity, TDto>(publishPattern, input, predicate, conditions)).ToArray())
        {
            Predicate = predicate;
            Conditions = conditions;
        }
    }

    public class RenewDataCommandSetHandler<TStore, TEntity, TDto> : IRequestHandler<RenewDataCommandSet<TStore, TEntity, TDto> , CommandSet<TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;       
        protected readonly IUltimatr _uservice;

        public RenewDataCommandSetHandler(IUltimatr uservice, IHostRepository<TStore, TEntity> repository)
        {
            _uservice = uservice;
            _repository = repository;
        }

        public virtual async Task<CommandSet<TDto>> Handle(RenewDataCommandSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<TEntity> entities;
                if (request.Conditions == null)
                    entities = _repository.PutBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate);
                else
                    entities = _repository.PutBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate, request.Conditions);

                await entities.ForEachAsync((e) => { request[e.Id].Entity = e; }).ConfigureAwait(false);

                _ = _uservice.Publish(new RenewedDataEventSet<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Failure<Domainlog>(ex.Message, request.Select(r => r.ErrorMessages).ToArray(), ex);
            }
            return request;
        }
    }
}
