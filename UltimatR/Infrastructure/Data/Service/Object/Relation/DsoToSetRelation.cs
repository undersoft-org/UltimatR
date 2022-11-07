using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace UltimatR
{
    public class DsoToSetRelation<TOrigin, TTarget> : DsoRelation<TOrigin, TTarget, DsoToSet<TTarget>> where TOrigin : class, IIdentifiable where TTarget : class, IIdentifiable
    {
        private Func<TTarget, object> targetKey;
        private Func<TOrigin, object> originKey;

        public DsoToSetRelation() : base()
        {
        }
        public DsoToSetRelation(Expression<Func<TOrigin, object>> originkey,
                                Expression<Func<TTarget, object>> targetkey)
                                    : base()
        {
            Towards = Towards.ToSet;
            OriginKey = originkey;
            TargetKey = targetkey;

            originKey = originkey.Compile();
            targetKey = targetkey.Compile();
        }

        public override Expression<Func<TTarget, bool>> CreatePredicate(object entity)
        {
            return LinqExtension.GetEqualityExpression(TargetKey, originKey, (TOrigin)entity);
        }
    }
}
