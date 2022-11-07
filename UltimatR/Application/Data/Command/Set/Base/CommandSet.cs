using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Series;

namespace UltimatR
{
    public class CommandSet<TDto> : Album<DataCommand<TDto>>, IRequest<CommandSet<TDto>>, ICommandSet<TDto> where TDto : Dto
    {
        public CommandMode CommandMode { get; set; }

        public PublishMode PublishMode { get; set; }

        protected CommandSet() : base(true)
        {
        }
        protected CommandSet(CommandMode commandMode) : base()
        {
            CommandMode = commandMode;
        }
        protected CommandSet(CommandMode commandMode, PublishMode publishPattern, DataCommand<TDto>[] dataCommands) : base(dataCommands)
        {
            CommandMode = commandMode;
            PublishMode = publishPattern;
        }

        public IEnumerable<DataCommand<TDto>> Commands { get => AsValues(); }

        public ValidationResult Result { get; set; } = new ValidationResult();

        public object Input => Commands.Select(c => c.Data);

        public object Output => Commands.ForEach(c => (c.Result.IsValid) ? c.Id as object : c.Result);

        IEnumerable<ICommand> ICommandSet.Commands { get => this.Cast<ICommand>(); }
    }
}