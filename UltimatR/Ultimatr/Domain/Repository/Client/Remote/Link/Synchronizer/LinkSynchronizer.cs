//-----------------------------------------------------------------------
// <copyright file="LinkSynchronizer.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Series;
using System.Threading;

using Microsoft.OData.Client;

namespace UltimatR
{
    public class LinkSynchronizer : ILinkSynchronizer
    {
        const int WAIT_LINKER_TIMEOUT = 10000;
        const int WAIT_RESULT_TIMEOUT = 10000;
        readonly ManualResetEventSlim linkerAccess = new ManualResetEventSlim(true, 128);
        readonly IDeck<IRepository>   repositories = new Catalog<IRepository>();
        readonly ManualResetEventSlim resultAccess = new ManualResetEventSlim(true, 128);
        int linkers;

        public LinkSynchronizer()
        {
        }

        public void AcquireLinker()
        {
            Interlocked.Increment(ref linkers);
            resultAccess.Reset();
            if(!linkerAccess.Wait(WAIT_LINKER_TIMEOUT))
                throw new TimeoutException("Wait read timeout");
        }

        public void AcquireResult()
        {
            if(!resultAccess.Wait(WAIT_RESULT_TIMEOUT))
                throw new TimeoutException("Wait write Timeout");
            linkerAccess.Reset();
        }

        public void AddRepository(IRepository repository) { repositories.Put(repository); }
        public void OnLinked(object sender, LoadCompletedEventArgs args) { ReleaseLinker(); }

        public void ReleaseLinker()
        {
            if(0 == Interlocked.Decrement(ref linkers))
                resultAccess.Set();
        }

        public void ReleaseResult() { linkerAccess.Set(); }
    }
}

