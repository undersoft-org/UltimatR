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
    public class CreateDataCommand<TStore, TEntity, TDto>  : DataCommand<TDto> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        [JsonIgnore] public Func<TEntity, Expression<Func<TEntity, bool>>> Predicate { get; }

        public CreateDataCommand(PublishMode publishPattern, TDto input) 
            : base(CommandMode.Create, publishPattern, input)
        {
            input.AutoId();
        }
        public CreateDataCommand(PublishMode publishPattern, TDto input, object key)
          : base(CommandMode.Create, publishPattern, input)
        {
            input.SetId(key);
        }
        public CreateDataCommand(PublishMode publishPattern, TDto input, Func<TEntity, Expression<Func<TEntity, bool>>> predicate) 
            : base(CommandMode.Create, publishPattern, input)
        {
            input.AutoId();
            Predicate = predicate;
        }
    }

    public class CreateDataCommandHandler<TStore, TEntity, TDto>  : IRequestHandler<CreateDataCommand<TStore, TEntity, TDto> , DataCommand<TDto>> 
        where TEntity : Entity where TDto : Dto where TStore : IDataStore
    {
        protected readonly IHostRepository<TEntity> _repository;       
        protected readonly IUltimatr _ultimatr;

        public CreateDataCommandHandler(IUltimatr ultimatr, IHostRepository<TStore, TEntity> repository)
        {
            _repository = repository;
            _ultimatr = ultimatr;
        }

        public async Task<DataCommand<TDto>> Handle(CreateDataCommand<TStore, TEntity, TDto> request, CancellationToken cancellationToken)
        {
            if (!request.Result.IsValid) 
                return request;
            try
            {
                request.Entity= await _repository.AddBy(request.Data, request.Predicate).ConfigureAwait(false);

                if (request.Entity == null) throw new Exception($"{ GetType().Name } " +
                                                                $"for entity { typeof(TEntity).Name } " +
                                                                $"unable create entry");              

                _ = _ultimatr.Publish(new CreatedDataEvent<TStore, TEntity, TDto>(request)).ConfigureAwait(false); ;
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
