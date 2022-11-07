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
    public class DeleteDataCommandSet<TStore, TEntity, TDto>  : CommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {        
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public DeleteDataCommandSet(PublishMode publishPattern, TDto[] inputs) 
            : base(CommandMode.Delete, publishPattern, inputs.Select(input => new DeleteDataCommand<TStore, TEntity, TDto>(publishPattern, input)).ToArray())
        {
        }
        public DeleteDataCommandSet(PublishMode publishPattern, TDto[] inputs, Func<TEntity, Expression<Func<TEntity, bool>>> predicate)
           : base(CommandMode.Delete, publishPattern, inputs.Select(input => new DeleteDataCommand<TStore, TEntity, TDto>(publishPattern, input, predicate)).ToArray())
        {
            Predicate = predicate;
        }
    }

    public class DeleteDataCommandSetHandler<TStore, TEntity, TDto> : IRequestHandler<DeleteDataCommandSet<TStore, TEntity, TDto> , CommandSet<TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;       
        protected readonly IUltimatr _uservice;

        public DeleteDataCommandSetHandler(IUltimatr uservice, IHostRepository<TStore, TEntity> repository)
        {
            _uservice = uservice;
            _repository = repository;
        }

        public virtual async Task<CommandSet<TDto>> Handle(DeleteDataCommandSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<TEntity> entities;             
                if (request.Predicate == null)
                    entities = _repository.DeleteBy(request.ForOnly(d => d.IsValid, d => d.Data));
                else
                    entities = _repository.DeleteBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate);

                await entities.ForEachAsync((e) => { request[e.Id].Entity = e; }).ConfigureAwait(false);

                _ = _uservice.Publish(new DeletedDataEventSet<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Failure<Domainlog>(ex.Message, request.Select(r => r.ErrorMessages).ToArray(), ex);
            }
            return request;
        }
    }
}
