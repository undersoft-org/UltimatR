﻿using FluentValidation.Results;
using MediatR;
using System;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class ChangedDsoHandler<TStore, TEntity> : INotificationHandler<ChangedDso<TStore, TEntity> > where TEntity : Entity where TStore : IDataStore
    {
        protected readonly IDataRepository<TEntity> _repository;
        protected readonly IDataRepository<Event> _eventStore;

        public ChangedDsoHandler(IDataRepository<IReportStore, TEntity> repository,
                                         IDataRepository<IEventStore, Event> eventStore)
        {
            _repository = repository;
            _eventStore = eventStore;
        }

        public virtual Task Handle(ChangedDso<TStore, TEntity> request, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (_eventStore.Add(request) == null)
                        throw new Exception($"{ GetType().Name } " +
                               $"for entity { typeof(TEntity).Name } unable add event");

                    if (request.Command.PublishMode == PublishMode.PropagateCommand)
                    {
                        TEntity result = null;
                        if (request.Delta != null)
                            result = await _repository.Patch(request.Delta, request.Predicate);
                        else
                            result = await _repository.Patch(request.Command.Data, request.Predicate);

                        if (result == null)
                            throw new Exception($"{ this.GetType().Name } " +
                                 $"for entity { typeof(TEntity).Name } unable change report");

                        request.PublishStatus = PublishStatus.Complete;
                    }
                }
                catch (Exception ex)
                {
                    request.Command.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                    this.Failure<Domainlog>(ex.Message, request.Command.ErrorMessages, ex);
                    request.PublishStatus = PublishStatus.Error;
                }
            }, cancellationToken);
        }
    }
}
