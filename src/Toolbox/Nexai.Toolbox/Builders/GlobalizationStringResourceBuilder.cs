// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Builders
{
    using Nexai.Toolbox.Abstractions.Services;
    using Nexai.Toolbox.Services;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class GlobalizationStringResourceBuilder : IGlobalizationStringResourceBuilder
    {
        #region Fields
        
        private readonly IServiceCollection _serviceDescriptors;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalizationStringResourceBuilder"/> class.
        /// </summary>
        public GlobalizationStringResourceBuilder(IServiceCollection serviceDescriptors)
        {
            this._serviceDescriptors = serviceDescriptors;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add RESXs the string resource.
        /// </summary>
        public IGlobalizationStringResourceBuilder RESXStringResource<TResource>()
        {
            this._serviceDescriptors.TryAddEnumerable(ServiceDescriptor.Singleton<IGlobalizationStringResourceSourceProvider, GlobalizationStringResourceFromRESXSourceProvider<TResource>>());
            return this;
        }

        #endregion
    }
}
