using Microsoft.OData.Edm;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UltimatR
{
    public static class DsModelBuilderExtensions
    { 
        public static IEdmModel LinkSetToSet<TOrigin, TMiddle, TTarget>(this IEdmModel builder,
                                                                 Expression<Func<TOrigin, IEnumerable<TMiddle>>> middleSet,
                                                                 Expression<Func<TMiddle, object>> middlekey,
                                                                 Expression<Func<TTarget, object>> targetkey)
                                                              where TOrigin : class, IIdentifiable
                                                              where TTarget : class, IIdentifiable
                                                               where TMiddle : class, IIdentifiable 
        {           
            new DsoSetToSetRelation<TOrigin, TTarget, TMiddle>(middleSet, middlekey, targetkey);
            return builder;
        }    

        public static IEdmModel LinkToSet<TOrigin, TTarget>(this IEdmModel builder,
                                                                 Expression<Func<TOrigin, object>> originkey,
                                                                 Expression<Func<TTarget, object>> targetkey)
                                                             where TOrigin : class, IIdentifiable
                                                             where TTarget : class, IIdentifiable
        {
            new DsoToSetRelation<TOrigin, TTarget>(originkey, targetkey);
            return builder;
        }      

        public static IEdmModel LinkToSingle<TOrigin, TTarget>(this IEdmModel builder,
                                                                Expression<Func<TOrigin, object>> originkey,
                                                                 Expression<Func<TTarget, object>> targetkey)
                                                            where TOrigin : class, IIdentifiable
                                                            where TTarget : class, IIdentifiable
        {
            new DsoToSingleRelation<TOrigin, TTarget>(originkey, targetkey);
            return builder;
        }       

    }
}

 