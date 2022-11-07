using UltimatR;
using System.Collections.ObjectModel;

namespace System.Instant.Tests
{
    public class AgreementText: Entity
    {
        public ulong TypeId { get; set; }  
        public ulong VersionId { get; set; }  
        public int VersionNumber { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
        public virtual AgreementVersion Version { get; set; }
    }
    
    public class AgreementContents : Collection<AgreementText>
    {
    }
}