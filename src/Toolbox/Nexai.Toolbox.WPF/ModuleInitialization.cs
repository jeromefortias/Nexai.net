// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF
{
    using Nexai.Toolbox.WPF.Abstractions;
    using Nexai.Toolbox.WPF.Abstractions.Services;
    using Nexai.Toolbox.WPF.Builders;
    using Nexai.Toolbox.WPF.Services;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using System;

    /// <summary>
    /// Initializtion of tool box services
    /// </summary>
    public static class ModuleInitialization
    {
        /// <summary>
        /// Setups the view relations.
        /// </summary>
        public static void SetupView<TViewManager>(this IServiceCollection services,
                                                   Action<IViewBuilder> builders,
                                                   TViewManager? inst = null)
            where TViewManager : class, IViewManager
        {
            if (inst is not null)
            {
                services.AddSingleton<IViewManager>(inst);
                services.AddSingleton<TViewManager>(inst);
            }
            else
            {
                services.AddSingleton<IViewManager, TViewManager>();
                services.AddSingleton<TViewManager, TViewManager>(p => (TViewManager)p.GetRequiredService<IViewManager>());
            }

            var service = new ServiceViewBuilder(services);
            builders?.Invoke(service);
        }
    }
}
