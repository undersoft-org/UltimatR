using MediatR;
using SharpCompress.Common;
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
    public class UpdateDataCommandSet<TStore, TEntity, TDto>  : CommandSet<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpdateDataCommandSet(PublishMode publishPattern, TDto[] inputs)
          : base(CommandMode.Update, publishPattern, inputs.Select(input => new UpdateDataCommand<TStore, TEntity, TDto>(publishPattern, input)).ToArray())
        {            
        }
        
        public UpdateDataCommandSet(PublishMode publishPattern, TDto[] inputs, Func<TDto, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Update, publishPattern, inputs.Select(input => new UpdateDataCommand<TStore, TEntity, TDto>(publishPattern, input, predicate)).ToArray())
        {
            Predicate = predicate;
        }
       
        public UpdateDataCommandSet(PublishMode publishPattern, TDto[] inputs, Func<TDto, Expression<Func<TEntity, bool>>> predicate, 
                                    params Func<TDto, Expression<Func<TEntity, bool>>>[] conditions)
            : base(CommandMode.Update, publishPattern, inputs.Select(input => new UpdateDataCommand<TStore, TEntity, TDto>(publishPattern, input, predicate, conditions)).ToArray())
        {
            Predicate = predicate;
            Conditions = conditions;
        }
    }

    public class UpdateDataCommandSetHandler<TStore, TEntity, TDto> : IRequestHandler<UpdateDataCommandSet<TStore, TEntity, TDto> , CommandSet<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IUltimatr _ultimatr;

        public UpdateDataCommandSetHandler(IUltimatr ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _ultimatr = ultimatr;
        }

        public async Task<CommandSet<TDto>> Handle(UpdateDataCommandSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<TEntity> entities = null;
                if (request.Predicate == null)
                    entities = _repository.SetBy(request.ForOnly(d => d.IsValid, d => d.Data));
                else if (request.Conditions == null)
                    entities = _repository.SetBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate);
                else
                    entities = _repository.SetBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate, request.Conditions);

                await entities.ForEachAsync((e) => { request[e.Id].Entity = e; }).ConfigureAwait(false);

                _ = _ultimatr.Publish(new UpdatedDataEventSet<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Failure<Domainlog>(ex.Message, request.Select(r => r.ErrorMessages).ToArray(), ex);
            }
            return request;
        }
    }
}
