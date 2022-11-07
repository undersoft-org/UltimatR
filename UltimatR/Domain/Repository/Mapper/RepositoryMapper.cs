﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Internal;

namespace UltimatR
{
    public class RepositoryMapper : IRepositoryMapper
    {
        #region Fields
        private static object buildHolder = new object();
        private static MapperConfigurationExpression expression = new ();
        private static MapperConfiguration configuration = new (c => {});
        private static Profile[] profiles = new Profile[0];

        public static void AddProfiles(params Profile[] profiles)
        {
            RepositoryMapper.profiles = RepositoryMapper.profiles.Concat(profiles).ToArray();
            expression.AddProfiles(profiles);
        }

        #endregion

        #region Constructors

        public RepositoryMapper(params Profile[] profiles)
        {
            if (profiles.Length > 0)           
                AddProfiles(profiles);    
            
            Build();
        }
        public RepositoryMapper()
        {
            mapper = configuration.CreateMapper();
        }

        #endregion

        #region Methods

        private IMapper mapper;

        public MapperConfigurationExpression MapperExtension => expression;

        public IConfigurationProvider ConfigurationProvider => mapper.ConfigurationProvider;

        public Func<Type, object> ServiceCtor => mapper.ServiceCtor;

        public static IRepositoryMapper GetMapper()
        {
            return new RepositoryMapper();
        }

        public bool TryCreateMap<TSource, TDestination>(bool reverse = true)
        {
            return TryCreateMap(new TypePair(typeof(TSource), typeof(TDestination)));
        }
        public bool TryCreateMap<TDestination>(object source,  bool reverse = true)
        {
            return TryCreateMap(new TypePair(source.GetType(), typeof(TDestination)));
        }
        public bool TryCreateMap(Type source, Type destination, bool reverse = true)
        {
            return TryCreateMap(new TypePair(source, destination));
        }
        public bool TryCreateMap<TDestination>(IQueryable source, bool reverse = true)
        {
            return TryCreateMap(new TypePair(source.ElementType, typeof(TDestination)));
        }
        public bool TryCreateMap(TypePair pair, bool reverse = true)
        {
            var _pair = GetValidTypePair(pair);
            if (MapNotExist(_pair))
            {
                if (expression.Mappers.Any(m => m.IsMatch((_pair))))
                    return true;

                if (reverse)
                    expression.CreateMap(_pair.SourceType, _pair.DestinationType).ReverseMap();
                else
                    expression.CreateMap(_pair.SourceType, _pair.DestinationType);

                return true;
            }

            return false;
        }

        public TypePair GetValidTypePair(TypePair pair)
        {
            return new TypePair(GetValidType(pair.SourceType), GetValidType(pair.DestinationType));
        }
        public Type GetValidType(Type type)
        {
            var ifaceTypes = type.GetInterfaces();
            if (ifaceTypes.Contains((typeof(ICollection))))
            {
                if (type.IsGenericType)
                    return type.GetGenericArguments().FirstOrDefault();
                return type.GetElementType();
            }

            return type;
        }

        public bool MapExist<TSource, TDestination>()
        {
            if (configuration.FindTypeMapFor<TSource, TDestination>() == null) 
                return false;
            return true;
        }
        public bool MapExist<TDestination>(object source)
        {
            TypePair tp = new TypePair(source.GetType(), typeof(TDestination));
            if (configuration.FindTypeMapFor(tp) == null)
                return false;
            return true;
        }
        public bool MapExist(TypePair pair)
        {
            if (configuration.FindTypeMapFor(pair) == null)
                return false;
            return true;
        }
        public bool MapExist(Type source, Type destination)
        {
            TypePair tp = new TypePair(source, destination);
            if (configuration.FindTypeMapFor(tp) == null)
                return false;
            return true;
        }
        public bool MapExist(IQueryable source, Type destination)
        {
            TypePair tp = new TypePair(source.ElementType, destination);
            if (configuration.FindTypeMapFor(tp) == null)
                return false;
            return true;
        }
        public bool MapExist<TDestination>(IQueryable source)
        {
            TypePair tp = new TypePair(source.ElementType, typeof(TDestination));
           
            if (configuration.FindTypeMapFor(tp) == null)
                return false;
            return true;
        }

        public bool MapNotExist(TypePair pair)
        {
            return !MapExist(pair);
        }
        public bool MapNotExist(Type source, Type destination)
        {
            return !MapExist(source, destination);
        }
        public bool MapNotExist<TSource, TDestination>()
        {
            return !MapExist<TSource, TDestination>();
        }
        public bool MapNotExist<TDestination>(object source)
        {
            return !MapExist<TDestination>(source);
        }

        public IDataMapper Build()
        {
            lock (buildHolder)
            {
                configuration = new MapperConfiguration(expression);
                mapper = configuration.CreateMapper();
            }
            return this;
        }

        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions<object, TDestination>> opts)
        {
            if (TryCreateMap<TDestination>(source))
                Build();
            return mapper.Map(source, opts);
        }
        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            if (TryCreateMap<TSource, TDestination>())
                Build();
            return mapper.Map(source, opts);
        }
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            if (TryCreateMap<TSource, TDestination>())
                Build();
            return mapper.Map(source, destination, opts);
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            if (TryCreateMap(sourceType, destinationType))
                Build();
            return mapper.Map(source, sourceType, destinationType, opts);
        }
        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            if (TryCreateMap(sourceType, destinationType))
                Build();
            return mapper.Map(source, destination, sourceType, destinationType, opts);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            if (TryCreateMap<TDestination>(source))
                Build();
            return mapper.ProjectTo(source, parameters, membersToExpand);
        }
        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand)
        {
            if (TryCreateMap<TDestination>(source))
                Build();
            return mapper.ProjectTo<TDestination>(source, parameters, membersToExpand);
        }
        public IQueryable ProjectTo(IQueryable source, Type destinationType, IDictionary<string, object> parameters = null, params string[] membersToExpand)
        {
            if (TryCreateMap(source.ElementType, destinationType))
                Build();
            return mapper.ProjectTo(source, destinationType, parameters, membersToExpand);
        }

        public TDestination Map<TDestination>(object source)
        {
            if (TryCreateMap<TDestination>(source))
                Build();
            return mapper.Map<TDestination>(source);
        }
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            if (TryCreateMap<TSource, TDestination>())
                Build();
            return mapper.Map<TSource, TDestination>(source);
        }
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (TryCreateMap<TSource, TDestination>())
                Build();
            return mapper.Map(source, destination);
        }
        public object Map(object source, Type sourceType, Type destinationType)
        {
            if (TryCreateMap(sourceType, destinationType))
                Build();
            return mapper.Map(source, sourceType, destinationType);
        }
        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            if (TryCreateMap(sourceType, destinationType))
                Build();
            return mapper.Map(source, destination, sourceType, destinationType);
        }

        #endregion
    }
}
