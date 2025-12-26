// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Abstractions.Supports
{
    /// <summary>
    /// Support converting this instance into debuggable string
    /// </summary>
    public interface ISupportDebugDisplayName
    {
        /// <summary>
        /// Convert instance to string with debug information
        /// </summary>
        string ToDebugDisplayName();
    }
}
