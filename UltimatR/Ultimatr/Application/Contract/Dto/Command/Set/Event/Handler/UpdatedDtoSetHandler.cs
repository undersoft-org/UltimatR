//-----------------------------------------------------------------------
// <copyright file="UpdatedDtoEventSet.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MediatR;
using System;
using System.Linq;
using System.Logs;
using System.Series;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class UpdatedDtoSetHandler<TStore, TEntity, TDto> : INotificationHandler<UpdatedDtoSet<TStore, TEntity, TDto>>
        where TEntity : Entity
        where TDto : Dto
        where TStore : IDataStore
    {
        protected readonly IDataRepository<Event> _eventStore;
        protected readonly IDataRepository<TEntity> _repository;

        public UpdatedDtoSetHandler()
        {
        }

        public UpdatedDtoSetHandler(
            IDataRepository<IReportStore, TEntity> repository,
            IDataRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(
            UpdatedDtoSet<TStore, TEntity, TDto> request,
            CancellationToken cancellationToken)
        {
            return Task.Run(
                async () =>
                {
                    try
                    {
                        request.ForOnly(
                            d => !d.Command.IsValid,
                            d =>
                            {
                                request.Remove(d);
                            });

                        await _eventStore.AddAsync(request);

                        if(request.PublishMode == PublishMode.PropagateCommand)
                        {
                            IDeck<TEntity> entities;
                            if(request.Predicate == null)
                                entities = _repository.SetBy(request.Select(d => d.Command.Data)).ToDeck();
                        else if(request.Conditions == null)
                                entities = _repository.SetBy(request.Select(d => d.Command.Data), request.Predicate)
                                    .ToDeck();
                        else
                                entities = _repository.SetBy(
                                    request.Select(d => d.Command.Data),
                                    request.Predicate,
                                    request.Conditions)
                                    .ToDeck();

                            request.ForEach(
                                (r) =>
                                {
                                    _ = entities.ContainsKey(r.AggregateId)
                                        ? (r.PublishStatus = PublishStatus.Complete)
                                        : (r.PublishStatus = PublishStatus.Uncomplete);
                                });
                        }
                    } catch(Exception ex)
                    {
                        this.Failure<Domainlog>(ex.Message, request.Select(r => r.Command.ErrorMessages).ToArray(), ex);
                        request.ForEach((r) => r.PublishStatus = PublishStatus.Error);
                    }
                },
                cancellationToken);
        }
    }
}
