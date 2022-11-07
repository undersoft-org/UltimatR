using FluentValidation.Results;
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
    public class ChangeDataCommandSet<TStore, TEntity, TDto>  : CommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {       
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get; }

        public ChangeDataCommandSet(PublishMode publishPattern, TDto[] inputs) 
            : base(CommandMode.Change, publishPattern, inputs.Select(c => new ChangeDataCommand<TStore, TEntity, TDto>(publishPattern, c, c.Id)).ToArray())
        {
        }
        public ChangeDataCommandSet(PublishMode publishPattern, TDto[] inputs, Func<TDto, Expression<Func<TEntity, bool>>> predicate)
           : base(CommandMode.Change, publishPattern, inputs.Select(c => new ChangeDataCommand<TStore, TEntity, TDto>(publishPattern, c, predicate)).ToArray())
        {
            Predicate = predicate;
        }
    }

    public class ChangeDataCommandSetHandler<TStore, TEntity, TDto> : IRequestHandler<ChangeDataCommandSet<TStore, TEntity, TDto> , CommandSet<TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;       
        protected readonly IUltimatr _uservice;

        public ChangeDataCommandSetHandler(IUltimatr uservice, IHostRepository<TStore, TEntity> repository)
        {
            _uservice = uservice;
            _repository = repository;
        }

        public virtual async Task<CommandSet<TDto>> Handle(ChangeDataCommandSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<TEntity> entities;
                if (request.Predicate == null)
                    entities = _repository.PatchBy(request.ForOnly(d => d.IsValid, d => d.Data));
                else
                    entities = _repository.PatchBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate);      

                await entities.ForEachAsync((e) => { request[e.Id].Entity = e; }).ConfigureAwait(false);

                _ = _uservice.Publish(new ChangedDataEventSet<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                 this.Failure<Domainlog>(ex.Message, request.Select(r => r.Output).ToArray(), ex);
            }
            return request;
        }
    }
}
