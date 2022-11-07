using FluentValidation.Results;
using MediatR;
using System;
using System.Linq.Expressions;
using System.Logs;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace UltimatR
{
    public class ChangeDataCommand<TStore, TEntity, TDto>  : DataCommand<TDto> where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {       
        [JsonIgnore] public Func<TDto, Expression<Func<TEntity, bool>>> Predicate { get; }

        public ChangeDataCommand(PublishMode publishMode, TDto input, params object[] keys) 
            : base(CommandMode.Change, publishMode, input, keys)
        {
        }
        public ChangeDataCommand(PublishMode publishMode, TDto input, Func<TDto, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Change, publishMode, input)
        {
            Predicate = predicate;
        }
    }

    public class ChangeDataCommandHandler<TStore, TEntity, TDto> : IRequestHandler<ChangeDataCommand<TStore, TEntity, TDto>, DataCommand<TDto>>
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;
        protected readonly IUltimatr _ultimatr;

        public ChangeDataCommandHandler(IUltimatr ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _ultimatr = ultimatr;
            _repository = repository;
        }

        public virtual async Task<DataCommand<TDto>> Handle(ChangeDataCommand<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            if (!request.Result.IsValid)
                return request;
            try
            {              
                if (request.Keys != null)
                    request.Entity = await _repository.PatchBy(request.Data, request.Keys).ConfigureAwait(false);
                else
                    request.Entity = await _repository.PatchBy(request.Data, request.Predicate).ConfigureAwait(false);

                if (request.Entity == null) throw new Exception($"{ GetType().Name } for entity " +
                                                                $"{ typeof(TEntity).Name } unable patch entry");                
                
                _ = _ultimatr.Publish(new ChangedDataEvent<TStore, TEntity, TDto>(request)).ConfigureAwait(false);
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
