// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Check equality beetween value and enum value pass in argument
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public sealed class EnumEqualToBoolConverter : IValueConverter, ISupportGroupConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum && parameter is Enum)
                return (int)value == (int)parameter;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
