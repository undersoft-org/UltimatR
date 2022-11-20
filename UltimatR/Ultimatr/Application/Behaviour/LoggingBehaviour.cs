﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Logs;
using System.Collections;

namespace UltimatR
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IDataIO where TResponse : IDataIO
    {
        public LoggingBehaviour()
        {           
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            request.Info<Apilog>($"Request data entry", request.Input);

            var response = await next();
         
            response.Info<Apilog>($"Response data result", response.Output);
           
            return response;
        } 
    }
} 