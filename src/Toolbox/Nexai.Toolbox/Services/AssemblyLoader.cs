// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Services
{
    using Nexai.Toolbox.Abstractions.Services;
    using Nexai.Toolbox.Extensions;

    using System.IO;
    using System.Reflection;

    /// <inheritdoc cref="IAssemblyLoader"/>
    /// <seealso cref="IAssemblyLoader" />
    public sealed class AssemblyLoader : IAssemblyLoader
    {
        #region Methods

        /// <inheritdoc />
        public Assembly Load(string path)
        {
            return Assembly.LoadFile(path);
        }

        /// <inheritdoc />
        public Assembly Load(Stream stream)
        {
            using (stream)
            {
                return Assembly.Load(stream.ReadAll());
            }
        }

        /// <inheritdoc />
        public Assembly Load(AssemblyName assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        #endregion
    }
}
