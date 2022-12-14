/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Link.cs
   
   @project: Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UltimatR
{
    public class DsoSetToSetRelation<TOrigin, TTarget, TMiddle> : DsoRelation<TOrigin, TTarget, TMiddle> where TOrigin : class, IIdentifiable where TMiddle : class, IIdentifiable where TTarget : class, IIdentifiable
    {
        private Expression<Func<TTarget, object>> targetKey;
        private Func<TMiddle, object> middleKey;
        private Func<TOrigin, IEnumerable<TMiddle>> middleSet;

        public DsoSetToSetRelation() : base()
        {
        }
        public DsoSetToSetRelation(Expression<Func<TOrigin, IEnumerable<TMiddle>>> middleset,
                                   Expression<Func<TMiddle, object>> middlekey,
                                   Expression<Func<TTarget, object>> targetkey) : base()
        {
            Towards = Towards.SetToSet;
            MiddleSet = middleset;
            MiddleKey = middlekey;
            TargetKey = targetkey;

            middleKey = middlekey.Compile();
            targetKey = targetkey;
            middleSet = middleset.Compile();

            Predicate = (o) =>
            {
                var ids = (IEnumerable<TMiddle>)o[MiddleSet.GetMemberName()];

                return LinqExtension.GetWhereInExpression(TargetKey, ids?.Select(middleKey));
            };
        }

        public override Expression<Func<TTarget, bool>> CreatePredicate(object entity)
        {
            var ids = (IEnumerable<TMiddle>)((IEntity)entity)[MiddleSet.GetMemberName()];

            return LinqExtension.GetWhereInExpression(TargetKey, ids?.Select(middleKey));
        }

    }



   
}
