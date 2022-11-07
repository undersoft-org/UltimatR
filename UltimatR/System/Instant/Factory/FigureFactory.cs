
// <copyright file="FigureFactory.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>

/// <summary>
/// The Instant namespace.
/// </summary>
namespace System.Instant
{
    using Series;
    using Uniques;

    /// <summary>
    /// Class FigureFactory.
    /// </summary>
    public static class FigureFactory
    {
        #region Fields

        /// <summary>
        /// The cache
        /// </summary>
        public static IDeck<Figure> Cache = new Catalog<Figure>();

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified mode.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        private static Figure Create<T>(FigureMode mode = FigureMode.Derived)
        {
            return Create(typeof(T), mode);
        }
        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        private static Figure Create(Type type, FigureMode mode = FigureMode.Derived)
        {
            return Create(type, type.UniqueKey32(), mode);
        }
        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="key">The key.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        private static Figure Create(Type type, uint key, FigureMode mode = FigureMode.Derived)
        {
            if (!Cache.TryGet(key, out Figure figure))
            {
                Cache.Add(key, figure = new Figure(type, mode));
            }
            return figure;
        }

        /// <summary>
        /// Gets the figure.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        public static Figure GetFigure(this object item, FigureMode mode = FigureMode.Derived)
        {
            var t = item.GetType();
            var key = t.UniqueKey32();
            if (!Cache.TryGet(key, out Figure figure))
            {
                Cache.Add(key, figure = new Figure(t, mode));
            }
            figure.Combine();
            return figure;
        }
        /// <summary>
        /// Gets the figure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        public static Figure GetFigure<T>(this T item, FigureMode mode = FigureMode.Derived)
        {
            var t = typeof(T);
            var key = t.UniqueKey32();
            if (!Cache.TryGet(key, out Figure figure))
            {
                Cache.Add(key, figure = new Figure(t, mode));
            }
            figure.Combine();
            return figure;
        }

        /// <summary>
        /// Generates the specified mode.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        public static Figure Generate<T>(FigureMode mode = FigureMode.Derived)
        {
            var figure = Create<T>(mode);
            figure.Combine();
            return figure;
        }
        /// <summary>
        /// Generates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        public static Figure Generate(Type type, FigureMode mode = FigureMode.Derived)
        {            
            var figure = Create(type);
            figure.Combine();
            return figure;
        }
        /// <summary>
        /// Generates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        public static Figure Generate(object item, FigureMode mode = FigureMode.Derived)
        {
            var figure = GetFigure(item);
            figure.Combine();
            return figure;
        }
        /// <summary>
        /// Generates the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>Figure.</returns>
        public static Figure Generate<T>(T item, FigureMode mode = FigureMode.Derived)
        {
            var figure = GetFigure<T>(item);
            figure.Combine();
            return figure;
        }

        /// <summary>
        /// Converts to figure.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>IFigure.</returns>
        public static IFigure ToFigure(this object item, FigureMode mode = FigureMode.Derived)
        {
            return Combine(item);
        }
        /// <summary>
        /// Converts to figure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>IFigure.</returns>
        public static IFigure ToFigure<T>(this T item, FigureMode mode = FigureMode.Derived)
        {
            Type t = typeof(T);
            if (t.IsInterface)
                return Combine((object)item);

            return Combine(item);
        }
        /// <summary>
        /// Converts to figure.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>IFigure.</returns>
        public static IFigure ToFigure(this Type type, FigureMode mode = FigureMode.Derived)
        {
            return Combine(type.New());
        }

        /// <summary>
        /// Combines the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>IFigure.</returns>
        public static IFigure Combine(object item, FigureMode mode = FigureMode.Derived) 
        {
            var t = item.GetType();
            if (t.IsAssignableTo(typeof(IFigure)))
                return (IFigure)item;

            var key = t.UniqueKey32();
            if (!Cache.TryGet(key, out Figure figure))
                Cache.Add(key, figure = new Figure(t, mode));          
            
            return figure.Combine();
        }
        /// <summary>
        /// Combines the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>IFigure.</returns>
        public static IFigure Combine<T>(T item, FigureMode mode = FigureMode.Derived)
        {
            var t = typeof(T);
            if (t.IsAssignableTo(typeof(IFigure)))
                return (IFigure)item;

            var key = t.UniqueKey32();
            if (!Cache.TryGet(key, out Figure figure))
                Cache.Add(key, figure = new Figure(t, mode));

            return figure.Combine();
        }

        #endregion
    }
}
