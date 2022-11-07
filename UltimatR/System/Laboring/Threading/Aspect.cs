
// <copyright file="Aspect.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Laboring namespace.
/// </summary>
namespace System.Laboring
{
    using Collections.Generic;
    using Series;
    using Linq;

    /// <summary>
    /// Class Aspect.
    /// Implements the <see cref="System.Series.Catalog{System.Laboring.Labor}" />
    /// Implements the <see cref="System.Laboring.ILaborator" />
    /// </summary>
    /// <seealso cref="System.Series.Catalog{System.Laboring.Labor}" />
    /// <seealso cref="System.Laboring.ILaborator" />
    public class Aspect : Catalog<Labor>, ILaborator
    {
        /// <summary>
        /// Gets or sets the case.
        /// </summary>
        /// <value>The case.</value>
        public LaborCase Case { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Aspect" /> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        public Aspect(string Name)
        {
            this.Name = Name;
            LaborersCount = 1;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Aspect" /> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="LaborList">The labor list.</param>
        public Aspect(string Name, IEnumerable<Labor> LaborList) : this(Name)
        {
            foreach (Labor labor in LaborList)
            {
                labor.Case = Case;
                labor.Aspect = this;
                Put(labor);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Aspect" /> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="MethodList">The method list.</param>
        public Aspect(string Name, IEnumerable<IDeputy> MethodList) 
            : this(Name, MethodList.Select(m => new Labor(m))) { }

        /// <summary>
        /// Gets or sets the laborers count.
        /// </summary>
        /// <value>The laborers count.</value>
        public int LaborersCount { get; set; }

        /// <summary>
        /// Gets or sets the laborator.
        /// </summary>
        /// <value>The laborator.</value>
        public Laborator Laborator { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Labor.</returns>
        public override Labor Get(object key)
        {           
            TryGet(key, out Labor result);
            return result;
        }

        /// <summary>
        /// Includes the specified labor.
        /// </summary>
        /// <param name="labor">The labor.</param>
        /// <returns>Labor.</returns>
        public Labor Include(Labor labor)
        {
            labor.Case = Case;
            labor.Aspect = this;
            Put(labor);
            return labor;
        }
        /// <summary>
        /// Includes the specified deputy.
        /// </summary>
        /// <param name="deputy">The deputy.</param>
        /// <returns>Labor.</returns>
        public Labor Include(IDeputy deputy)
        {            
            Labor labor = new Labor(deputy);
            labor.Case = Case;
            labor.Aspect = this;
            Put(labor);
            return labor;
        }

        /// <summary>
        /// Includes the specified labors.
        /// </summary>
        /// <param name="labors">The labors.</param>
        /// <returns>Aspect.</returns>
        public Aspect Include(IEnumerable<Labor> labors)
        {
            foreach(Labor labor in labors)
            {
                labor.Case = Case;
                labor.Aspect = this;
                Put(labor);
            }
            return this;
        }
        /// <summary>
        /// Includes the specified methods.
        /// </summary>
        /// <param name="methods">The methods.</param>
        /// <returns>Aspect.</returns>
        public Aspect Include(IEnumerable<IDeputy> methods)
        {
            foreach (IDeputy method in methods)
            {
                Labor labor = new Labor(method);
                labor.Case = Case;
                labor.Aspect = this;
                Put(labor);
            }
            return this;
        }

        /// <summary>
        /// Includes this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Aspect.</returns>
        public virtual Aspect Include<T>() where T : class
        {
            var deputy = new Deputy<T>();
            Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            return this;
        }
        /// <summary>
        /// Includes the specified arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns>Aspect.</returns>
        public virtual Aspect Include<T>(Type[] arguments) where T : class
        {
            var deputy = new Deputy<T>(arguments);
            Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            return this;
        }
        /// <summary>
        /// Includes the specified consrtuctor parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="consrtuctorParams">The consrtuctor parameters.</param>
        /// <returns>Aspect.</returns>
        public virtual Aspect Include<T>(params object[] consrtuctorParams) where T : class
        {
            var deputy = new Deputy<T>(consrtuctorParams);
            Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            return this;
        }
        /// <summary>
        /// Includes the specified arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <param name="consrtuctorParams">The consrtuctor parameters.</param>
        /// <returns>Aspect.</returns>
        public virtual Aspect Include<T>(Type[] arguments, params object[] consrtuctorParams) where T : class
        {
            var deputy = new Deputy<T>(arguments, consrtuctorParams);
            Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            return this;
        }

        /// <summary>
        /// Includes the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The method.</param>
        /// <returns>Aspect.</returns>
        public virtual Aspect Include<T>(Func<T, string> method) where T : class
        {
            var deputy = new Deputy<T>(method);
            Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            return this;
        }
        /// <summary>
        /// Includes the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>Aspect.</returns>
        public virtual Aspect Include<T>(Func<T, string> method, params Type[] arguments) where T : class
        {
            var deputy = new Deputy<T>(method, arguments);
            Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            return this;
        }
        /// <summary>
        /// Includes the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The method.</param>
        /// <param name="consrtuctorParams">The consrtuctor parameters.</param>
        /// <returns>Aspect.</returns>
        public virtual Aspect Include<T>(Func<T, string> method, params object[] consrtuctorParams) where T : class
        {
            var deputy = new Deputy<T>(method, consrtuctorParams);
            Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            return this;
        }

        /// <summary>
        /// Operations this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Labor.</returns>
        public virtual Labor Operation<T>() where T : class
        {            
            if (!TryGet(Deputy.GetName<T>(), out Labor labor))
            {
                var deputy = new Deputy<T>();
                return Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            }
            return labor;
        }
        /// <summary>
        /// Operations the specified arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns>Labor.</returns>
        public virtual Labor Operation<T>(Type[] arguments) where T : class
        {
            if (!TryGet(Deputy.GetName<T>(arguments), out Labor labor))
            {
                var deputy = new Deputy<T>();
                return Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            }
            return labor;
        }
        /// <summary>
        /// Operations the specified consrtuctor parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="consrtuctorParams">The consrtuctor parameters.</param>
        /// <returns>Labor.</returns>
        public virtual Labor Operation<T>(params object[] consrtuctorParams) where T : class
        {
            if (!TryGet(Deputy.GetName<T>(), out Labor labor))
            {
                var deputy = new Deputy<T>(consrtuctorParams);
                return Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            }
            return labor;
        }
        /// <summary>
        /// Operations the specified arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <param name="constructorParams">The constructor parameters.</param>
        /// <returns>Labor.</returns>
        public virtual Labor Operation<T>(Type[] arguments, params object[] constructorParams) where T : class
        {
            if (!TryGet(Deputy.GetName<T>(arguments), out Labor labor))
            {
                var deputy = new Deputy<T>(arguments, constructorParams);
                return Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            }
            return labor;
        }

        /// <summary>
        /// Operations the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The method.</param>
        /// <returns>Labor.</returns>
        public virtual Labor Operation<T>(Func<T, string> method) where T : class
        {
            var deputy = new Deputy<T>(method);
            if (!TryGet(deputy.Name, out Labor labor))
            {
                return Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            }
            return labor;
        }
        /// <summary>
        /// Operations the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The method.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>Labor.</returns>
        public virtual Labor Operation<T>(Func<T, string> method, params Type[] arguments) where T : class
        {
            var deputy = new Deputy<T>(method, arguments);
            if (!TryGet(deputy.Name, out Labor labor))
            {               
                return Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            }
            return labor;
        }
        /// <summary>
        /// Operations the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The method.</param>
        /// <param name="consrtuctorParams">The consrtuctor parameters.</param>
        /// <returns>Labor.</returns>
        public virtual Labor Operation<T>(Func<T, string> method, params object[] consrtuctorParams) where T : class
        {
            var deputy = new Deputy<T>(method, consrtuctorParams);
            if (!TryGet(deputy.Name, out Labor labor))
            {               
                return Include(Case.Methods.SureGet(deputy, k => deputy).Value);
            }
            return labor;
        }

        /// <summary>
        /// Gets or sets the <see cref="Labor" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Labor.</returns>
        public override Labor this[object key]
        {
            get => base[key];
            set
            {
                value.Case = Case;
                value.Aspect = this;
                base[key] = value;
            }
        }

        /// <summary>
        /// Closes the specified safe close.
        /// </summary>
        /// <param name="SafeClose">if set to <c>true</c> [safe close].</param>
        public void Close(bool SafeClose)
        {
            Laborator.Close(SafeClose);
        }

        /// <summary>
        /// Allocates the specified laborers count.
        /// </summary>
        /// <param name="laborersCount">The laborers count.</param>
        /// <returns>Aspect.</returns>
        public Aspect Allocate(int laborersCount = 1)
        {
            Laborator.Allocate(laborersCount);
            return this;
        }

        /// <summary>
        /// Runs the specified labor.
        /// </summary>
        /// <param name="labor">The labor.</param>
        public void Run(Labor labor)
        {
            Laborator.Run(labor);
        }

        /// <summary>
        /// Resets the specified laborers count.
        /// </summary>
        /// <param name="laborersCount">The laborers count.</param>
        public void Reset(int laborersCount = 1)
        {
            Laborator.Reset(laborersCount);
        }
    }
}
