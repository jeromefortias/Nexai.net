// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Nexai.Toolbox.WPF.Abstractions.Views
{
    using System;

    /// <summary>
    /// View relation between view and view model
    /// </summary>
    public sealed record ViewRelation(Guid Uid, Type View, Type ViewModel, ViewRelationInfo Info);
}
