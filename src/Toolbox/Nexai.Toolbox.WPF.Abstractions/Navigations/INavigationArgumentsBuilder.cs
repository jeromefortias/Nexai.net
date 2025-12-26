// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.Abstractions.Navigations
{
    /// <summary>
    /// 
    /// </summary>
    public interface INavigationArgumentsBuilder
    {
        /// <summary>
        /// Adds specified value in the navigation arguments
        /// </summary>
        INavigationArgumentsBuilder Add<TValue>(string key, TValue? value);
    }
}
