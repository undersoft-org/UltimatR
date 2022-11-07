using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Series;
using UltimatR;

namespace System.Instant.Tests
{
    public class Agreement : Entity
    {
        public AgreementKind Kind { get; set; }
        [Key]
        public Guid UserId { get; set; }
        public long TypeId { get; set; }
        public long VersionId { get; set; }
        public long FormatId { get; set; }
        public string Language { get; set; } = "pl";
        public string Comments { get; set; } = "Comments";
        public string Email { get; set; } = "fdfds";
        public string PhoneNumber { get; set; } = "3453453";

        public virtual DboSet<AgreementFormat> Formats { get; set; } = null;
        public virtual DboSet<AgreementVersion> Versions { get; set; } = null;
        public virtual AgreementType Type { get; set; } = null;
    }

    public class Agreements : KeyedCollection<long, Agreement>
    {
        protected override long GetKeyForItem(Agreement item)
        {
            return (item.Id == 0) ? (long)item.AutoId() : item.Id;
        }
    }
}
