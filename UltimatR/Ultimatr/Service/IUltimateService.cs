﻿using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public interface IUltimateService : IServiceManager, IRepositoryManager
    {
        //void LazyLoading(bool enable);
        //void AutoDetectChanges(bool enable);
        //void AutoTransaction(bool enable);

        Task Publish(object notification, CancellationToken cancellationToken = default);
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;

        Task<R> Run<T, R>(Func<T, Task<R>> function) where T : class;
        Task<R> Run<T, R>(string methodname, params object[] parameters) where T : class;

        Task Run<T>(Func<T, Task> function) where T : class;
        Task Run<T>(string methodname, params object[] parameters);
        Task Save(bool asTransaction = false);

        Task<object> Send(object request, CancellationToken cancellationToken = default);
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

        Task<R> Serve<T, R>(Func<T, Task<R>> function) where T : class;
        Task<R> Serve<T, R>(string methodname, params object[] parameters) where T : class;

        Task Serve<T>(Func<T, Task> function) where T : class;
        Task Serve<T>(string methodname, params object[] parameters);
      
    }
}