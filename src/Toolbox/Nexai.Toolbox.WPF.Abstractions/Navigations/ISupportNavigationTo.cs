// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.Abstractions.Navigations
{
    using System.Threading.Tasks;

    /// <summary>
    /// View model that need to be informed when navigate to view occured
    /// </summary>
    public interface ISupportNavigationTo
    {
        /// <summary>
        /// Called when view is navigate to
        /// </summary>
        ValueTask OnNavigateToAsync(NavigationArguments? arguments);
    }
}
