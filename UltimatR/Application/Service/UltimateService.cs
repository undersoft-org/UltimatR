using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Client;
using System;
using System.Linq;
using System.Logs;
using System.Threading;
using System.Threading.Tasks;
using System.Uniques;

namespace UltimatR
{
    public class UltimateService : ServiceManager, IUltimateService, IMediator
    {
        protected IMediator mediator;
        private new bool disposedValue;

        public UltimateService() : base() { }

        public Task<R> Run<T, R>(Func<T, Task<R>> function) where T : class
        {
            return Task.Run(async () =>
            {
                return await function.Invoke(GetService<T>());
            });
        }
        public Task    Run<T>(Func<T, Task> function) where T : class
        {
            return Task.Run(async () =>
            {
                await function.Invoke(GetService<T>());
            });
        }
        public Task    Run<T>(string methodname, params object[] parameters)
        {
            var deputy = new Deputy(GetOrNewService<T>(), methodname);
            return deputy.ExecuteAsync(parameters);
        }
        public Task<R> Run<T, R>(string methodname, params object[] parameters) where T : class
        {
            var deputy = new Deputy(GetOrNewService<T>(), methodname);
            return deputy.ExecuteAsync<R>(parameters);
        }

        public Task<R> Serve<T, R>(Func<T, Task<R>> function) where T : class
        {
            return Task.Run(async () =>
            {
                using (var us = new Ultimatr())
                {
                    return await function.Invoke(us.GetService<T>());
                }
            });
        }
        public Task    Serve<T>(Func<T, Task> function) where T : class
        {
            return Task.Run(async () =>
            {
                using (var us = new Ultimatr())
                {
                    await function.Invoke(us.GetService<T>());
                }
            });
        }
        public Task    Serve<T>(string methodname, params object[] parameters)
        {
            return Task.Run(() =>
            {
                using (var us = new UltimateService())
                {
                    var deputy = new Deputy(us.GetOrNewService<T>(), methodname);
                    return deputy.Execute(parameters);
                }
            });
        }
        public Task<R> Serve<T, R>(string methodname, params object[] parameters) where T : class
        {
            return Task.Run(async () =>
            {
                using (var us = new UltimateService())
                {
                    var deputy = new Deputy(us.GetOrNewService<T>(), methodname);
                    return await deputy.ExecuteAsync<R>(parameters);
                }
            });
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return await Mediator.Send(request, cancellationToken);
        }
        public async Task<object>    Send(object request, CancellationToken cancellationToken = default)
        {
            return await Mediator.Send(request, cancellationToken);
        }

        public async Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            await Serve<IMediator>(async (m) => await m.Publish(notification, cancellationToken));
        }
        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            await Serve<IMediator>(async (m) => await m.Publish(notification, cancellationToken));
        }

        protected IMediator Mediator => mediator ??= GetService<IMediator>();

        public Lazy<R> Deserve<T, R>(Func<T, R> function) where T : class where R : class
        {
            return new Lazy<R>(function.Invoke(GetService<T>()));
        }

        public void AutoDetectChanges(bool enable)
        {
            foreach (var endpoint in Endpoints)
            {
                endpoint.Context.ChangeTracker.AutoDetectChangesEnabled = enable;
            }
        }

        public void LazyLoading(bool enable)
        {
            foreach (var endpoint in Endpoints)
            {
                endpoint.Context.ChangeTracker.LazyLoadingEnabled = enable;
            }
        }

        public void AutoTransaction(bool enable)
        {
            foreach (var endpoint in Endpoints)
            {
                endpoint.Context.Database.AutoTransactionsEnabled = enable;
            }
        }

        public async Task Save(bool asTransaction = false)
        {
            await SaveEndpoints(true);
            await SaveClients(true);
        }

        public async Task<int> SaveEndpoints(bool asTransaction = false)
        {
            var changes = 0;
            for (int i = 0; i < Endpoints.Count; i++)
                changes += await SaveEndpoint(Endpoints[i], asTransaction);
            return changes;
        }

        public async Task<int> SaveClients(bool asTransaction = false)
        {
            var changes = 0;
            for (int i = 0; i < Clients.Count; i++)
                changes += await SaveClient(Clients[i], asTransaction);
            return changes;
        }

        public async Task<int> SaveEndpoint(IRepositoryEndpoint endpoint, bool asTransaction = false)
        {
            var  changes = 0;
            var context = endpoint.Context;
            if (context.ChangeTracker.HasChanges())
            {
                if (asTransaction)
                {
                    return await saveAsTransaction(context, changes);
                }
                else
                {
                    return await saveChanges(context, changes);
                }
            }        
            return 0;
        }

        public async Task<int> SaveClient(IRepositoryClient client, bool asTransaction = false)
        {
            var changes = 0;
            var context = client.Context;       
            if (context.Entities.Any())
            {
                if (asTransaction)
                {
                    return await saveAsTransaction(context, changes);
                }
                else
                {
                    return await saveChanges(context, changes);
                }
            }          
            return 0;
        }

        private async Task<int> saveAsTransaction(DbContext context, int changes)
        {
            await using (var tr = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    changes += await context.SaveChangesAsync();

                    await tr.CommitAsync();                    
                }
                catch (DbUpdateException e)
                {
                    if (e is DbUpdateConcurrencyException)
                        tr.Warning<Datalog>($"Concurrency update exception data changed by: { e.Source }, " +
                                            $"entries involved in detail data object", e.Entries, e);

                    tr.Failure<Datalog>(
                        $"Fail on update database transaction Id:{tr.TransactionId}, using context:{context.GetType().Name}," +
                        $" context Id:{context.ContextId}, TimeStamp:{DateTime.Now.ToString()}, changes made count:{changes} ");
                        
                    await tr.RollbackAsync();

                    tr.Warning<Datalog>($"Transaction Id:{tr.TransactionId} Rolling Back !!");
                }
                return changes;
            }           
        }

        private async Task<int> saveChanges(DbContext context, int changes)
        {
            try
            {
                changes += await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e is DbUpdateConcurrencyException)
                    context.Warning<Datalog>($"Concurrency update exception data changed by: { e.Source }, " +
                                             $"entries involved in detail data object", e.Entries, e);

                context.Failure<Datalog>(
                    $"Fail on update database, using context:{context.GetType().Name}, " +
                    $"context Id: {context.ContextId}, TimeStamp: {DateTime.Now.ToString()}, " +
                    $"changes made count: {changes} ");
            }

            return changes;
        }

        private async Task<int> saveAsTransaction(DsContext context, int changes)
        {
            try
            {
                changes += (await context.SaveChangesAsync(SaveChangesOptions.BatchWithSingleChangeset)).Count();
            }
            catch (Exception e)
            {
                context.Failure<Datalog>(
                    $"Fail on update dataservice as singlechangeset, using context:{context.GetType().Name}, " +
                    $"TimeStamp: {DateTime.Now.ToString()}, " +
                    $"changes made count: {changes} ");
            }

            return changes;
        }

        private async Task<int> saveChanges(DsContext context, int changes)
        {
            try
            {
                changes += (await context.SaveChangesAsync(SaveChangesOptions.BatchWithIndependentOperations | SaveChangesOptions.ContinueOnError)).Count();
            }
            catch (Exception e)
            {
                context.Failure<Datalog>(
                    $"Fail on update dataservice as independent operations, using context:{context.GetType().Name}, " +
                    $"TimeStamp: {DateTime.Now.ToString()}, " +
                    $"changes made count: {changes} ");
            }

            return changes;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {                                        
                   base.Dispose(true);                    
                }               
                disposedValue = true;
            }
        }

        public override async ValueTask DisposeAsyncCore()
        {
            await base.DisposeAsyncCore().ConfigureAwait(false);

        }
    }
}
