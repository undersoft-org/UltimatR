using UltimatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Series;

namespace System.Instant.Tests
{
    public class OriginAgreementType : Entity
    {
        public long TypeId { get; set; }

        public virtual AgreementType Type { get; set; }
    }

    public class OriginAgreementTypes : KeyedCollection<long, OriginAgreementType>
    {
        protected override long GetKeyForItem(OriginAgreementType item)
        {
            return (item.Id == 0) ? (long)item.AutoId() : item.Id;
        }
    }
}
