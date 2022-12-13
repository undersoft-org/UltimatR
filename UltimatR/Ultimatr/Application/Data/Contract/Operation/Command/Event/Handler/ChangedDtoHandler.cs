//-----------------------------------------------------------------------
// <copyright file="ChangedDtoEvent.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using FluentValidation.Results;
using MediatR;
using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class ChangedDtoHandler<TStore, TEntity, TCommand> : INotificationHandler<ChangedDto<TStore, TEntity, TCommand>>
        where TEntity : Entity
        where TCommand : Dto
        where TStore : IDataStore
    {
        protected readonly IDataRepository<Event> _eventStore;
        protected readonly IDataRepository<TEntity> _repository;

        public ChangedDtoHandler()
        {
        }

        public ChangedDtoHandler(
            IDataRepository<IReportStore, TEntity> repository,
            IDataRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(
            ChangedDto<TStore, TEntity, TCommand> request,
            CancellationToken cancellationToken)
        {
            return Task.Run(
                async () =>
                {
                    try
                    {
                        if(_eventStore.Add(request) == null)
                            throw new Exception(
                                $"{($"{GetType().Name} for entity ")}{($"{typeof(TEntity).Name} unable add event")}");

                        if(request.Command.PublishMode == PublishMode.PropagateCommand)
                        {
                            TEntity entity;
                            if(request.Command.Keys != null)
                                entity = await _repository.PatchBy(request.Command.Data, request.Command.Keys);
                        else
                                entity = await _repository.PatchBy(request.Command.Data, request.Predicate);

                            if(entity == null)
                                throw new Exception(
                                    $"{($"{GetType().Name} for entity ")}{($"{typeof(TEntity).Name} unable change report")}");

                            request.PublishStatus = PublishStatus.Complete;
                        }
                    } catch(Exception ex)
                    {
                        request.Command.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                        this.Failure<Domainlog>(ex.Message, request.Command.ErrorMessages, ex);
                        request.PublishStatus = PublishStatus.Error;
                    }
                },
                cancellationToken);
        }
    }
}
