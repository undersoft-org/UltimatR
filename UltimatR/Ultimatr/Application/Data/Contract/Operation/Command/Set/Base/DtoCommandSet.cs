using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Series;

namespace UltimatR
{
    public class DtoCommandSet<TDto> : Album<DtoCommand<TDto>>, IRequest<DtoCommandSet<TDto>>, ICommandSet<TDto> where TDto : Dto
    {
        public CommandMode CommandMode { get; set; }

        public PublishMode PublishMode { get; set; }

        protected DtoCommandSet() : base(true)
        {
        }
        protected DtoCommandSet(CommandMode commandMode) : base()
        {
            CommandMode = commandMode;
        }
        protected DtoCommandSet(CommandMode commandMode, PublishMode publishPattern, DtoCommand<TDto>[] DtoCommands) : base(DtoCommands)
        {
            CommandMode = commandMode;
            PublishMode = publishPattern;
        }

        public IEnumerable<DtoCommand<TDto>> Commands { get => AsValues(); }

        public ValidationResult Result { get; set; } = new ValidationResult();

        public object Input => Commands.Select(c => c.Data);

        public object Output => Commands.ForEach(c => (c.Result.IsValid) ? c.Id as object : c.Result);

        IEnumerable<ICommand> ICommandSet.Commands { get => this.Cast<ICommand>(); }
    }
}