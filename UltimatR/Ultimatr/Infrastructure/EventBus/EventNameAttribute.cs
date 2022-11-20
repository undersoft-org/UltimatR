﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Castle.Core.Internal;
using JetBrains.Annotations;
namespace UltimatR
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventNameAttribute : Attribute, IEventNameProvider
    {
        public virtual string Name { get; }

        public EventNameAttribute([DisallowNull] string name)
        {
            Name = name;
        }

        public static string GetNameOrDefault<TEvent>()
        {
            return GetNameOrDefault(typeof(TEvent));
        }

        public static string GetNameOrDefault([NotNull] Type eventType)
        {           
            return eventType
                       .GetCustomAttributes(true)
                       .OfType<IEventNameProvider>()
                       .FirstOrDefault()
                       ?.GetName(eventType)
                   ?? eventType.FullName;
        }

        public string GetName(Type eventType)
        {
            return Name;
        }
    }
}