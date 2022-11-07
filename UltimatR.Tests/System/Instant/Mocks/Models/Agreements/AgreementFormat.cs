using UltimatR;
using System;
using System.Collections.Generic;

namespace System.Instant.Tests
{
    public class AgreementFormat : Entity
    {
        public string Name { get; set; }

        public virtual Agreements Agreements { get; } = new Agreements();
    }


}
