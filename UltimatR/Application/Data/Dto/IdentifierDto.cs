using System.Text.Json.Serialization;

namespace UltimatR
{
    public class IdentifierDto<TDto> : IdentifierDto where TDto : Dto
    {
        public override ulong EntityId { get; set; }

        [JsonIgnore] public virtual TDto Entity { get; set; }
    }

    public class IdentifierDto : Dto
    {
        public virtual ulong EntityId { get; set; }

        public IdKind Kind { get; set; }

        public string Name { get; set; }
        
        public string Value { get; set; }

        public long Key { get; set; }
    }
}