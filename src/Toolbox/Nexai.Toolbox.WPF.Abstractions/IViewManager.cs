// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.Abstractions
{
    using Nexai.Toolbox.WPF.Abstractions.Navigations;

    /// <summary>
    /// Define a manager in charge to display requested user interface
    /// </summary>
    public interface IViewManager
    {
        /// <summary>
        /// Shows <typeparamref name="TView"/> view.
        /// </summary>
        void Show<TView>(NavigationArguments? arguments = null, bool dialog = false, string? specializedId = null);

        /// <summary>
        /// Navigate to view associate with <paramref name="key"/>.
        /// </summary>
        void NavigateTo(string key, NavigationArguments? arguments = null, string? specializedId = null);

        /// <summary>
        /// Navigate to view associate with view model <typeparamref name="TViewViewModel"/>.
        /// </summary>
        void NavigateTo<TViewViewModel>(NavigationArguments? arguments = null, string? specializedId = null);
    }
}