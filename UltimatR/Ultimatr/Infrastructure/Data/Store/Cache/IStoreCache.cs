﻿//-----------------------------------------------------------------------
// <copyright file="IStoreCache.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace UltimatR
{
    public interface IStoreCache<TStore> : IDataCache
    {
        IDataMapper Mapper { get; set; }
    }
}