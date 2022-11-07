
// <copyright file="IDeputy.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



using System.Reflection;
using System.Threading.Tasks;

/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    #region Interfaces




    /// <summary>
    /// Interface IDeputy
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IUnique" />
    public interface IDeputy : IUnique
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the qualified.
        /// </summary>
        /// <value>The name of the qualified.</value>
        string QualifiedName { get; set; }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        /// <value>The target object.</value>
        object TargetObject { get; set; }




        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        MethodInfo Info { get; set; }




        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        ParameterInfo[] Parameters { get; set; }




        /// <summary>
        /// Gets or sets the parameter values.
        /// </summary>
        /// <value>The parameter values.</value>
        object[] ParameterValues { get; set; }

        /// <summary>
        /// Gets the method deputy.
        /// </summary>
        /// <value>The method deputy.</value>
        MethodDeputy MethodDeputy { get; }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        Delegate Method { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Publishes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Task.</returns>
        Task Publish(params object[] parameters);
        /// <summary>
        /// Publishes the specified first as target.
        /// </summary>
        /// <param name="firstAsTarget">if set to <c>true</c> [first as target].</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Task.</returns>
        Task Publish(bool firstAsTarget, object target, params object[] parameters);






        /// <summary>
        /// Executes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        object Execute(params object[] parameters);
        /// <summary>
        /// Executes the specified first as target.
        /// </summary>
        /// <param name="firstAsTarget">if set to <c>true</c> [first as target].</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        object Execute(bool firstAsTarget, object target, params object[] parameters);






        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        Task<object> ExecuteAsync(params object[] parameters);
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="firstAsTarget">if set to <c>true</c> [first as target].</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        Task<object> ExecuteAsync(bool firstAsTarget, object target, params object[] parameters);







        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> ExecuteAsync<T>(params object[] parameters);
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstAsTarget">if set to <c>true</c> [first as target].</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> ExecuteAsync<T>(bool firstAsTarget, object target, params object[] parameters);

        #endregion
    }

    #endregion
}
