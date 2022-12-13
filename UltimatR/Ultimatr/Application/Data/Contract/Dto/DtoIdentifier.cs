using System.Text.Json.Serialization;

namespace UltimatR
{
    public class DtoIdentifier<TDto> : DtoIdentifier where TDto : Dto
    {
        public override ulong EntityId { get; set; }

        [JsonIgnore] public virtual TDto Entity { get; set; }
    }

    public class DtoIdentifier : Dto
    {
        public virtual ulong EntityId { get; set; }

        public IdKind Kind { get; set; }

        public string Name { get; set; }
        
        public string Value { get; set; }

        public long Key { get; set; }
    }
}