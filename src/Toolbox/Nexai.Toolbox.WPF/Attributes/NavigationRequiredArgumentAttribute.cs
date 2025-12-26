// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.Attributes
{
    using System;

    /// <summary>
    /// Define the required argument on navigation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class NavigationRequiredArgumentAttribute<TAttr> : Attribute
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRequiredArgumentAttribute{TAttr}"/> class.
        /// </summary>
        public NavigationRequiredArgumentAttribute(string key, string? description = null)
        {
            this.Key = key;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; }

        #endregion
    }
}
