// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.UI.Converters
{
    using System;
    using System.Globalization;

    /// <summary>
    /// 
    /// </summary>
    public interface ISupportGroupConverter
    {
        /// <inheritdoc cref="IValueConverter.Convert"/>
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
