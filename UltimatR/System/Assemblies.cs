/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Class Assemblies.
    /// </summary>
    public static class Assemblies
    {
        #region Properties

        /// <summary>
        /// Gets the assembly code.
        /// </summary>
        /// <value>The assembly code.</value>
        public static string AssemblyCode
        {
            get
            {
                object[] attributes;

                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly is null)
                    attributes = Assembly.GetCallingAssembly()
                        .GetCustomAttributes(typeof(GuidAttribute), false);
                else
                    attributes = entryAssembly
                        .GetCustomAttributes(typeof(GuidAttribute), false);

                if (attributes.Length == 0)
                    return string.Empty;

                return ((GuidAttribute) attributes[0]).Value.ToUpper();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds the type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>Type.</returns>
        public static Type FindType(string name, string nameSpace = null)
        {
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            var namespaceFirstBlock = AppDomain.CurrentDomain
                                               .FriendlyName
                                               .Split('.').First();
            foreach (var asm in asms)
            {
                if (!asm.IsDynamic)
                {
                    var extypes = asm.GetExportedTypes();

                    foreach (var extype in extypes)
                    {
                        if (namespaceFirstBlock.Equals(
                            extype.Namespace.Split('.').First()) &&
                            (nameSpace == null ||
                            extype.Namespace == nameSpace))
                        {
                            if (extype.Name.Equals(name))
                                return extype;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the type.
        /// </summary>
        /// <param name="argumentType">Type of the argument.</param>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>Type.</returns>
        public static Type FindType(Type argumentType, object argumentValue, Type attributeType = null,
            string nameSpace = null)
        {
            var asms = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var asm in asms)
            {
                Type[] types = nameSpace != null
                    ? asm.GetTypes().Where(n => n.Namespace == nameSpace).ToArray()
                    : asm.GetTypes();

                if (attributeType != null)
                {
                    foreach (var type in types)
                        if (type.GetCustomAttributesData().Where(a => a.AttributeType == attributeType)
                            .Any(s => s.ConstructorArguments
                                .Any(o => o.ArgumentType == argumentType &&
                                          o.Value.Equals(argumentValue))))
                            return type;
                }
                else
                    foreach (var type in types)
                        if (type.GetCustomAttributesData().Any(s => s.ConstructorArguments
                            .Any(o => o.ArgumentType == argumentType &&
                                      o.Value.Equals(argumentValue))))
                            return type;
            }

            return null;
        }

        /// <summary>
        /// Resolves the executing.
        /// </summary>
        public static void ResolveExecuting()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                String resourceName = "AssemblyLoadingAndReflection." + new AssemblyName(args.Name).Name + ".dll";
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }

        #endregion
    }
}
