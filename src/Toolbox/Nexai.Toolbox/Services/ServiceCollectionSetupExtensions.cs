// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

// KEEP : Microsoft.Extensions.DependencyInjection
namespace Microsoft.Extensions.DependencyInjection
{
    using Nexai.Toolbox.Abstractions.Services;
    using Nexai.Toolbox.Builders;
    using Nexai.Toolbox.Services;

    using Microsoft.Extensions.DependencyInjection.Extensions;

    using System;

    public static class ServiceCollectionSetupExtensions
    {
        /// <summary>
        /// Setups the globalization resources.
        /// </summary>
        public static IServiceCollection SetupGlobalizationResources(this IServiceCollection services, Action<IGlobalizationStringResourceBuilder> resourceBuilder)
        {
            services.TryAddSingleton<IGlobalizationStringResourceProvider, GlobalizationStringResourceProvider>();

            var inst = new GlobalizationStringResourceBuilder(services);
            resourceBuilder(inst);
            return services;
        }
    }
}
