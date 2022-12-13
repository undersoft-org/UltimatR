using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class UpsertDtoSetHandler<TStore, TEntity, TDto> : IRequestHandler<UpsertDtoSet<TStore, TEntity, TDto> , DtoCommandSet<TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;       
        protected readonly IUltimatr _uservice;

        public UpsertDtoSetHandler(IUltimatr uservice, IDataRepository<TStore, TEntity> repository)
        {
            _uservice = uservice;
            _repository = repository;
        }

        public virtual async Task<DtoCommandSet<TDto>> Handle(UpsertDtoSet<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<TEntity> entities;
                if (request.Conditions == null)
                    entities = _repository.PutBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate);
                else
                    entities = _repository.PutBy(request.ForOnly(d => d.IsValid, d => d.Data), request.Predicate, request.Conditions);

                await entities.ForEachAsync((e) => { request[e.Id].Entity = e; }).ConfigureAwait(false);

                _ = _uservice.Publish(new RenewedDtoSet<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Failure<Domainlog>(ex.Message, request.Select(r => r.ErrorMessages).ToArray(), ex);
            }
            return request;
        }
    }
}
