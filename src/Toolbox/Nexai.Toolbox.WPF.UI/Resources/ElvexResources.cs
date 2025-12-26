// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.UI.Resources
{
    using System.Windows;

    public static class ElvexResources
    {
        #region Ctor        

        /// <summary>
        /// Initializes the <see cref="ElvexResources"/> class.
        /// </summary>
        static ElvexResources()
        {
            Global = new Uri("pack://application:,,,/Nexai.Toolbox.WPF.UI;component/Resources/Global.xaml");
            Converter = new Uri("pack://application:,,,/Nexai.Toolbox.WPF.UI;component/Resources/Converters.xaml");
            DarkTheme = new Uri("pack://application:,,,/Nexai.Toolbox.WPF.UI;component/Resources/Themes/Dark/DarkThemeResources.xaml");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the global.
        /// </summary>
        public static Uri Global { get; }

        /// <summary>
        /// Gets the converter.
        /// </summary>
        public static Uri Converter { get; }

        /// <summary>
        /// Gets the dark theme.
        /// </summary>
        public static Uri DarkTheme { get; }

        #endregion
    }
}
