// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Enums
{
    using Nexai.Toolbox.Abstractions.Attributes;

    /// <summary>
    /// Enum expositing logical relation
    /// </summary>
    public enum LogicEnum
    {
        None,

        [DescriptionWithAlias('&')]
        And,

        [DescriptionWithAlias('|')]
        Or,

        [DescriptionWithAlias('^')]
        ExclusiveOr,

        [DescriptionWithAlias('!')]
        Not
    }
}
