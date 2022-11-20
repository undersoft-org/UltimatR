using MediatR;
using System;
using System.Text.Json.Serialization;

namespace UltimatR
{
    public class DtoCommand<TDto> : Command, IRequest<DtoCommand<TDto>>, IUnique where TDto : Dto
    {
        [JsonIgnore] public override TDto Data => base.Data as TDto;     

        protected DtoCommand()
        {        
        }
        protected DtoCommand(CommandMode commandMode, TDto dataObject)
        {
            CommandMode = commandMode;
            base.Data = dataObject;
        }
        protected DtoCommand(CommandMode commandMode, PublishMode publishMode, TDto dataObject) 
            : base(dataObject, commandMode, publishMode)
        {
        }
        protected DtoCommand(CommandMode commandMode, PublishMode publishMode, TDto dataObject, params object[] keys) 
            : base(dataObject, commandMode, publishMode, keys)
        {
        }
        protected DtoCommand(CommandMode commandMode, PublishMode publishMode, params object[] keys)
           : base(commandMode, publishMode, keys)
        {
        }

        public byte[] GetBytes()
        {
            return Data.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return Data.GetUniqueBytes();
        }
        
        public bool Equals(IUnique other)
        {
            return Data.Equals(other);
        }

        public int CompareTo(IUnique other)
        {
            return Data.CompareTo(other);
        }

        public override long Id { get => Data.Id; set => Data.Id = value; }

        public ulong UniqueKey { get => (ulong)Data.Id; set => Data.Id=(long)value; }

        public ulong UniqueSeed { get => Data.UniqueSeed; set => Data.UniqueSeed=value; }
    }
}