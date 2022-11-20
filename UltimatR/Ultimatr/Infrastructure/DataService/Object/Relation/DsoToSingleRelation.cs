/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Link.cs
   
   @project: Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/
using System.Uniques;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Instant;
using System.Linq;
using System.Linq.Expressions;
using System.Series;
using NLog.Targets;
using System.Reflection;
using Microsoft.OData.Client;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace UltimatR
{  
    public class DsoToSingleRelation<TOrigin, TTarget> : DsoRelation<TOrigin, TTarget, TTarget> where TOrigin : class, IIdentifiable where TTarget : class, IIdentifiable
    {

        private Func<TTarget, object> targetKey;
        private Func<TOrigin, object> originKey;

        public DsoToSingleRelation() : base()
        {
        }
        public DsoToSingleRelation(Expression<Func<TOrigin, object>> originkey,
                                   Expression<Func<TTarget, object>> targetkey)
                                   : base()
        {
            Towards = Towards.ToSingle;
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
