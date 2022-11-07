using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UltimatR
{
    public class LinkedResultAttribute : TypeFilterAttribute
    {
        public LinkedResultAttribute() : base(typeof(LinkedResult))
        {
            Order = 1;
        }

        private class LinkedResult : IResultFilter//, IAsyncResultFilter
        {
            private readonly ILinkSynchronizer synchronizer;

            public LinkedResult(IUltimatr ultimatr)
            {
                synchronizer = ultimatr.GetService<ILinkSynchronizer>();
            }

            public void OnResultExecuting(ResultExecutingContext context)
            {
                var preresult = context.Result;
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
                synchronizer.AcquireResult();
                var result = context.Result;
                synchronizer.ReleaseResult();
            }

            //public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
            //{
            //    // Do something before the action executes.
            //    var preresult = context.Result;
            //    // next() calls the action method.
            //    var resultContext = await next();
            //    var result = resultContext.Result;
            //    // resultContext.Result is set.
            //    // Do something after the action executes.
            //}
        }
    }
}

