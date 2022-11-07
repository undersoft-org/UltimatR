﻿using System;
using System.Instant;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace UltimatR
{
    [Serializable]
    public class FilterItem
    {    
        #region Properties

        public string Property { get; set; }
            
        public string Operand { get; set; }

        public object Value { get; set; }

        public string Type { get; set; }

        public string Logic { get; set; } = "And";

        #endregion
     
    }
}
