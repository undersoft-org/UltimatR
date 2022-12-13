//-----------------------------------------------------------------------
// <copyright file="LinkedResult.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UltimatR
{
    public class LinkedResultAttribute : TypeFilterAttribute
    {
        public LinkedResultAttribute() : base(typeof(LinkedResult)) { Order = 1; }

        class LinkedResult : IResultFilter
        {
            readonly ILinkSynchronizer synchronizer;

            public LinkedResult(IUltimatr ultimatr) { synchronizer = ultimatr.GetService<ILinkSynchronizer>(); }

            public void OnResultExecuted(ResultExecutedContext context)
            {
                synchronizer.AcquireResult();
                IActionResult result = context.Result;
                synchronizer.ReleaseResult();
            }

            public void OnResultExecuting(ResultExecutingContext context)
            {
                IActionResult preresult = context.Result;
            }
        }
    }
}

