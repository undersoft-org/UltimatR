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
    public class CreateDataCommandSet<TStore, TEntity, TDto>  : CommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {       
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreateDataCommandSet(PublishMode publishPattern, TDto[] inputs) 
            : base(CommandMode.Create, publishPattern, inputs.Select(input => new CreateDataCommand<TStore, TEntity, TDto>(publishPattern, input)).ToArray())
        {
        }
        public CreateDataCommandSet(PublishMode publishPattern, TDto[] inputs, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
           : base(CommandMode.Create, publishPattern, inputs.Select(input => new CreateDataCommand<TStore, TEntity, TDto>(publishPattern, input, predicate)).ToArray())
        {
            Predicate = predicate;
        }
    }

    public class CreateDataCommandSetHandler<TStore, TEntity, TDto> : IRequestHandler<CreateDataCommandSet<TStore, TEntity, TDto> , CommandSet<TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;       
        protected readonly IUltimatr _uservice;

        public CreateDataCommandSetHandler(IUltimatr uservice, IHostRepository<TStore, TEntity> repository)
        {
            _uservice = uservice;
            _repository = repository;
        }

        public virtual async Task<CommandSet<TDto>> Handle(CreateDataCommandSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<TEntity> entities;
                if (request.Predicate == null)
                    entities = await _repository.AddBy(request.ForOnly(d => d.IsValid, d => d.Data));
                else
                    entities = await _repository.AddBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate);

                await entities.ForEachAsync((e, x) => { request[x].Entity = e; }).ConfigureAwait(false);

                _ = _uservice.Publish(new CreatedDataEventSet<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Failure<Domainlog>(ex.Message, request.Select(r => r.ErrorMessages).ToArray(), ex);
            }
            return request;
        }
    }
}
