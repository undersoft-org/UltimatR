﻿using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace UltimatR
{
    public interface ILinkedObject<TStore, TOrigin> : ILinkedObject<TOrigin> where TOrigin : Entity where TStore : IDataStore
    {   
    }

    public interface ILinkedObject<TOrigin> : ILinkedObject where TOrigin : Entity 
    {
    }

    public interface ILinkedObject : IRepository, IDsoRelation 
    {
        bool IsLinked { get; set; }

        IRepository Host { get; set; }

        void Load(object origin);

        Task LoadAsync(object origin);

        void LinkTrigger(object sender, EntityEntryEventArgs e);

        ILinkSynchronizer Synchronizer { get; }
    }
}
