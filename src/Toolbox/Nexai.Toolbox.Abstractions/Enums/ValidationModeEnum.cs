// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Enums
{
    /// <summary>
    /// Define the type of logical validation between validator of the same group collection
    /// </summary>
    public enum ValidationModeEnum
    {
        None = 0,
        All,
        AtLeastOne,
        NoOne
    }
}
