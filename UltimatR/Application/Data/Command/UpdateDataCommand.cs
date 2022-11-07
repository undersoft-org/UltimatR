using System;
using System.Linq.Expressions;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UltimatR
{
    public class UpdateDataCommand<TStore, TEntity, TDto>  : DataCommand<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>>[] Conditions { get; }

        public UpdateDataCommand(PublishMode publishPattern, TDto input, params object[] keys)
           : base(CommandMode.Update, publishPattern, input, keys)
        {
        }
        
        public UpdateDataCommand(PublishMode publishPattern, TDto input, Func<TEntity, 
                                Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Update, publishPattern, input)
        {
            Predicate = predicate;
        }
        
        public UpdateDataCommand(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate, 
                                params Func<TEntity, Expression<Func<TEntity, bool>>>[] conditions) 
            : base(CommandMode.Update, publishPattern, input)
        {
            Predicate = predicate;
            Conditions = conditions;
        }
    }

    public class UpdateDataCommandHandler<TStore, TEntity, TDto> : IRequestHandler<UpdateDataCommand<TStore, TEntity, TDto> , DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IUltimatr _ultimatr;

        public UpdateDataCommandHandler(IUltimatr ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _ultimatr = ultimatr;
        }

        public async Task<DataCommand<TDto>> Handle(UpdateDataCommand<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            if (!request.Result.IsValid) return request;

            try
            {                
                if (request.Predicate == null)
                    request.Entity = await _repository.SetBy(request.Data, request.Keys);
                else if (request.Conditions == null)
                    request.Entity = await _repository.SetBy(request.Data, request.Predicate);
                else
                    request.Entity = await _repository.SetBy(request.Data, request.Predicate, request.Conditions);

                if (request.Entity == null) throw new Exception($"{ this.GetType().Name } for entity " +
                                                                $"{ typeof(TEntity).Name } unable update entry");
            
                _ = _ultimatr.Publish(new UpdatedDataEvent<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                request.Result.Errors.Add(new ValidationFailure(string.Empty, ex.Message));
                this.Failure<Applog>(ex.Message, request.ErrorMessages, ex);
            }

            return request;
        }
    }
}
