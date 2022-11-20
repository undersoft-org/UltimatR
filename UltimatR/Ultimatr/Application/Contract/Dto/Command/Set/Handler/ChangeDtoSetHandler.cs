using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class ChangeDtoSetHandler<TStore, TEntity, TDto> : IRequestHandler<ChangeDtoSet<TStore, TEntity, TDto> , DtoCommandSet<TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;       
        protected readonly IUltimatr _uservice;

        public ChangeDtoSetHandler(IUltimatr uservice, IDataRepository<TStore, TEntity> repository)
        {
            _uservice = uservice;
            _repository = repository;
        }

        public virtual async Task<DtoCommandSet<TDto>> Handle(ChangeDtoSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<TEntity> entities;
                if (request.Predicate == null)
                    entities = _repository.PatchBy(request.ForOnly(d => d.IsValid, d => d.Data));
                else
                    entities = _repository.PatchBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate);      

                await entities.ForEachAsync((e) => { request[e.Id].Entity = e; }).ConfigureAwait(false);

                _ = _uservice.Publish(new ChangedDtoSet<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                 this.Failure<Domainlog>(ex.Message, request.Select(r => r.Output).ToArray(), ex);
            }
            return request;
        }
    }
}
