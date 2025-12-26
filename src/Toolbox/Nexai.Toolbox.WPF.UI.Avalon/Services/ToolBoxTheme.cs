// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Nexai.Toolbox.WPF.UI.Avalon.Services
{
    using AvalonDock.Themes;

    using System;

    internal sealed class ToolBoxTheme : Theme
    {
        public override Uri GetResourceUri()
        {
            return new Uri("/Nexai.Toolbox.WPF.UI.Avalon;component/Resources/AvalonThemeBrushes.xaml", UriKind.Relative);
        }
    }
}
