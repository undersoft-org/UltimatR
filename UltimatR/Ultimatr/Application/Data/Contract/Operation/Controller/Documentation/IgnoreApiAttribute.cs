using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace UltimatR
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class IgnoreApiAttribute : ActionFilterAttribute { }
}

