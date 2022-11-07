using UltimatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Series;

namespace System.Instant.Tests
{
    public class AgreementVersion : Entity
    {
        public long TypeId { get; set; }
        
        public int VersionNumber { get; set; }

        public int OriginId { get; set; }

        public virtual AgreementType Type { get; set; }
        public virtual DboSet<Agreement> Agreements { get; set; }
        public virtual DboSet<AgreementText> Texts { get; set; } 
    }

    public class AgreementVersions : KeyedCollection<long, AgreementVersion>
    {
        protected override long GetKeyForItem(AgreementVersion item)
        {
            return (item.Id == 0) ? (long)item.AutoId() : item.Id;
        }
    }
}
