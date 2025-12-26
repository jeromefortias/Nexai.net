// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Serializations
{
    using System;

    /// <summary>
    /// Define a service able validate if the convertions could apply following the algorithme setups
    /// </summary>
    internal interface IElvexJsonObjectConverterConditions
    {
        /// <summary>
        /// Validates the conditions
        /// </summary>
        bool Validate(Func<string, Tuple<bool, object?>> getJsonValue, Type objectType, object? existingValue);
    }
}
