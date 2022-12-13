using System;
using System.Linq;
using System.Logs;
using System.Threading;
using Microsoft.OData.Edm;
using System.Threading.Tasks;
using Microsoft.OData.ModelBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel;

namespace UltimatR
{
    public class DataContext<TStore> : DataContext, IDataContext<TStore> where TStore : IDataStore
    {
        protected virtual Type StoreType { get; }       

        public DataContext(DbContextOptions options) : base(options)
        {
            StoreType = typeof(TStore);
        }
    }

    public class DataContext : DbContext, IDataContext, IResettableService
    {
        protected ODataConventionModelBuilder odataBuilder;
        protected IEdmModel edmModel;
        public virtual IUltimatr ultimatr { get; }

        public DataContext(DbContextOptions options, IUltimatr ultimatr = null) : base(options)
        {
            odataBuilder = new ODataConventionModelBuilder();
            this.ultimatr = ultimatr;
        }

        public interface IDbContextStore
        {
            IUltimatr ultimatr { get; }
        }

        public IQueryable<TEntity> DataSet<TEntity>() where TEntity : class, IIdentifiable
        {
            return this.Set<TEntity>();
        }

        public object ServiceEntitySet<TEntity>() where TEntity : class, IIdentifiable
        {
            return odataBuilder.EntitySet<TEntity>(typeof(TEntity).Name);
        }
        
        public object ServiceEntitySet(Type entityType)
        {
            var entitySetName = entityType.Name;
            if (entityType.IsGenericType && entityType.IsAssignableTo(typeof(Identifier)))
                entitySetName = entityType.GetGenericArguments().FirstOrDefault().Name + "Identifier";

            var etc = odataBuilder.AddEntityType(entityType);
            etc.Name = entitySetName;
            var ets = odataBuilder.AddEntitySet(entitySetName, etc);
            ets.EntityType.HasKey(entityType.GetProperty("Id"));
            return ets;
        }

        public override IModel Model
        {
            get
            {
                return base.Model;
            }
        }

        public TModel CreateServiceModel<TModel>()
        {
            var entityTypes = this.Model.GetEntityTypes();
            odataBuilder = new ODataConventionModelBuilder();

            foreach (var entityType in entityTypes)
            {
                var type = entityType.ClrType;
                var setType = ((EntitySetConfiguration)ServiceEntitySet(type)).EntityType;
                setType.HasKey(type.GetProperty("Id"));
            }

            return (TModel)odataBuilder.GetEdmModel();
        }

        public TModel GetServiceModel<TModel>()
        {
            var model = edmModel ??= odataBuilder.GetEdmModel();
            odataBuilder.ValidateModel(model);
            return (TModel)model;
        }

        public virtual Task<int> Save(bool asTransaction, CancellationToken token = default(CancellationToken))
        {
            return saveEndpoint(asTransaction, token);
        }

        private async Task<int> saveEndpoint(bool asTransaction, CancellationToken token = default(CancellationToken))
        {
            if (this.ChangeTracker.HasChanges())
            {
                if (asTransaction)
                    return await saveAsTransaction(token);
                else
                    return await saveChanges(token);
            }
            return 0;
        }

        private async Task<int> saveAsTransaction(CancellationToken token = default(CancellationToken))
        {
            await using var tr = await this.Database.BeginTransactionAsync(token);
            try
            {
                var changes = await this.SaveChangesAsync(token);

                await tr.CommitAsync(token);

                return changes;
            }
            catch (DbUpdateException e)
            {
                if (e is DbUpdateConcurrencyException)
                    tr.Warning<Datalog>($"Concurrency update exception data changed by: {e.Source}, " +
                                        $"entries involved in detail data object", e.Entries, e);
                else
                    tr.Failure<Datalog>(
                        $"Fail on update database transaction Id:{tr.TransactionId}, using context:{this.GetType().Name}," +
                        $" context Id:{this.ContextId}, TimeStamp:{DateTime.Now.ToString()}");

                await tr.RollbackAsync(token);

                tr.Warning<Datalog>($"Transaction Id:{tr.TransactionId} Rolling Back !!");
            }
            return -1;
        }

        private async Task<int> saveChanges(CancellationToken token = default(CancellationToken))
        {
            try
            {
                return await this.SaveChangesAsync(token);
            }
            catch (DbUpdateException e)
            {
                if (e is DbUpdateConcurrencyException)
                    this.Warning<Datalog>($"Concurrency update exception data changed by: {e.Source}, " +
                                             $"entries involved in detail data object", e.Entries, e);
                else
                    this.Failure<Datalog>(
                        $"Fail on update database, using context:{this.GetType().Name}, " +
                        $"context Id: {this.ContextId}, TimeStamp: {DateTime.Now.ToString()}");
            }

            return -1;
        }
    }

}