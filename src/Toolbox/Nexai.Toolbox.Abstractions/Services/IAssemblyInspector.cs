// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Services
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Inspector used to navigate through assembly and type info
    /// </summary>
    public interface IAssemblyInspector
    {
        /// <summary>
        /// Gets the assembly attributes type <see cref="TAttribute"/> or inheriting from.
        /// </summary>
        IReadOnlyCollection<TAttribute> GetAssemblyAttributes<TAttribute>(Assembly assembly)
            where TAttribute : Attribute;

        /// <summary>
        /// Searches the assembly that have the attribute <typeparamref name="TAttribute"/>.
        /// </summary>
        IReadOnlyCollection<(Assembly assembly, TAttribute attribute)> SearchAssembliesWithAttribute<TAttribute>(params Assembly[] excludes)
            where TAttribute : Attribute;

        /// <summary>
        /// Registers an assembly and it's dependencies in the inspector.
        /// Attention <paramref name="loadDependencies"/> will if needed try loading the assembly in the current execution context.
        /// </summary>
        /// <remarks>
        ///     This call is thread safe and execute only once by assembly
        /// </remarks>
        void RegisterAssemblyAndDependencies(Assembly assembly, bool loadDependencies = false);
    }
}
