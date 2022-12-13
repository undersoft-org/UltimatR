using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Series;

namespace UltimatR
{
    public class GetDso<TStore, TEntity> : IRequest<IDeck<TEntity>> where TEntity : Entity where TStore : IDataStore
    {
        public SortExpression<TEntity> Sort { get; }
        public Expression<Func<TEntity, object>>[] Expanders { get; }

        public GetDso(params Expression<Func<TEntity, object>>[] expanders)
        {
            Expanders = expanders;
        }

        public GetDso(SortExpression<TEntity> sortTerms, params Expression<Func<TEntity, object>>[] expanders)
        {
            Sort = sortTerms;
            Expanders = expanders;
        }
    }
}
